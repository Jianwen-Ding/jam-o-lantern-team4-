using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static stateCapture;

public class rewindPoint : MonoBehaviour
{
    bool isActivated = false;
    playerController playerActivated = null;
    [SerializeField]
    int maxClones;
    [SerializeField]
    GameObject clonePrefab;

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
            print("wow: "+capturedStates.countStates());
            baseFeeder.stateRecording = new StatePlaythroughCaptured(capturedStates);
            StatePlaythroughCaptured seperateStateRecord = new StatePlaythroughCaptured(capturedStates);
            storedStates.Add(seperateStateRecord);
            clones.Add(baseFeeder);
            // resets all clones to original position
            resetClones();
            return true;
        }
        else
        {
            return false;
        }
    }

    // resets all clones to their original positions and states
    public void resetClones()
    {
        for(int i = 0; i < clones.Count; i++)
        {
            clones[i].stateRecording = storedStates[i];
        }
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player"&& !isActivated)
        {
            isActivated = true;
            playerActivated = other.GetComponent<playerController>();
            playerActivated.reachPoint(this);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < clones.Count; i++)
        {
            debugValueSys.display("" + i, clones[i].stateRecording.countStates() + "");
        }
    }
}
