using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static stateCapture;

public class rewindPoint : MonoBehaviour
{
    GameObject player;
    // activation vars
    bool inActivation = false;
    [SerializeField]
    float activateForce;
    [SerializeField]
    float reversalRatio;
    Rigidbody playerPhysics;
    // cloning vars
    bool isActivated = false;
    playerController playerActivated = null;
    [SerializeField]
    int maxClones;
    [SerializeField]
    GameObject clonePrefab;

    // rewind event vars
    public delegate void rewindAction();
    public static event rewindAction rewindCommand;

    public delegate void setAction();
    public static event setAction setCommand;

    //These two variables need to have the exact same count
    List<StatePlaythroughCaptured> storedStates = new List<StatePlaythroughCaptured>();
    List<inputFeeder> clones = new List<inputFeeder>();

    // uses a captured state in order to create a new clone, returns false if there are too many clones
    public bool clonePlayer(StatePlaythroughCaptured capturedStates)
    {
        if(storedStates.Count < maxClones )
        {
            // creates the new clone
            GameObject newClone = Instantiate(clonePrefab, gameObject.transform.position, gameObject.transform.rotation);
            GameObject baseClone = newClone.transform.GetChild(0).gameObject;
            inputFeeder baseFeeder = baseClone.GetComponent<inputFeeder>();
            baseFeeder.stateRecording = capturedStates.dulplicate();
            StatePlaythroughCaptured seperateStateRecord = capturedStates.dulplicate();
            storedStates.Add(seperateStateRecord);
            clones.Add(baseFeeder);
            // resets all clones to original position
            rewind();
            return true;
        }
        else
        {
            return false;
        }
    }

    // resets all clones to their original positions and states
    public void rewind()
    {
        for(int i = 0; i < clones.Count; i++)
        {
            debugValueSys.display(i + "","Clone " + i + " time: " + storedStates[i].statesTime());
            clones[i].stateRecording = storedStates[i].dulplicate();
        }
        if(rewindCommand != null)
        {
            rewindCommand();
        }
    }

    // when the player destroys the last clone
    public void destroyLastClone()
    {
        if(clones.Count > 0)
        {
            Destroy(clones[clones.Count - 1].transform.parent.gameObject);
            storedStates.RemoveAt(clones.Count - 1);
            clones.RemoveAt(clones.Count - 1);
        }
        rewind();
    }

    // when the player stops using the rewind point
    public void disconnect()
    {
        for (int i = 0; i < clones.Count; i++)
        {
            Destroy(clones[i].gameObject.transform.parent.gameObject);
        }
        storedStates.Clear();
        clones.Clear();
        isActivated = false;
        inActivation = false;
    }

    private void connect()
    {
        player.transform.position = gameObject.transform.position;
        playerPhysics.velocity = Vector3.zero;
        isActivated = true;
        playerActivated = player.GetComponent<playerController>();
        playerActivated.reachPoint(this);
        if(setCommand != null)
        {
            setCommand();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player"&& !isActivated)
        {
            player = other.gameObject;
            playerPhysics = player.GetComponent<Rigidbody>();
            inActivation = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!isActivated && inActivation)
        {
            Vector3 direction = mathHelper.getVectorFromAngle(activateForce, mathHelper.getAngleBetweenVec(player.transform.position, gameObject.transform.position));
            // reversal speedup
            if ((direction.x > 0) != (playerPhysics.velocity.x > 0))
            {
                direction.x *= reversalRatio;
                playerPhysics.velocity = new Vector3(0, playerPhysics.velocity.y, playerPhysics.velocity.z);
            }
            if ((direction.y > 0) != (playerPhysics.velocity.y > 0))
            {
                direction.y *= reversalRatio;
                playerPhysics.velocity = new Vector3(playerPhysics.velocity.x, 0, playerPhysics.velocity.z);
            }
            if ((direction.z > 0) != (playerPhysics.velocity.z > 0))
            {
                direction.z *= reversalRatio;
                playerPhysics.velocity = new Vector3(playerPhysics.velocity.x, playerPhysics.velocity.y, playerPhysics.velocity.z);
            }
            playerPhysics.AddForce(direction * Time.deltaTime, ForceMode.Acceleration);
            if (mathHelper.distance(player.transform.position, gameObject.transform.position) < 0.05)
            {
                connect();
            }
        }
    }
}
