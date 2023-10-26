using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handController : MonoBehaviour
{
    #region vars
    playerCam getAngle;
    List<GameObject> withinHand = new List<GameObject>();

    //Player controllers
    public bool hasGrabCommand = false;
    public bool hasThrownCommand = false;
    public bool hasPunchCommand = false;
    [SerializeField]
    bool isBeingControlledByPlayer;
    [SerializeField]
    GameObject player;
    [SerializeField]
    stateCapture stateCapturer;

    //Grab Vars
    [SerializeField]
    float grabTime;
    float grabTimeLeft = 0;
    [SerializeField]
    float letGoStrength;
    [SerializeField]
    float reversalSpeedUp;
    bool hasGrabInput = false;
    int grabbedLayer = 7;
    bool hasGrabbed;
    GameObject grabbedObject;
    Rigidbody grabbedPhysics;
    int originalLayer;
    [SerializeField]
    float adjustForce;
    [SerializeField]
    float distanceUntilBreak;
    [SerializeField]
    float distanceToAdjustRatio;

    //Throw Vars
    [SerializeField]
    float throwStrength;
    bool hasThrownInput = false;

    //Punch Vars
    [SerializeField]
    float punchTime;
    float punchTimeLeft;
    [SerializeField]
    float punchCooldownTime;
    float punchCooldownTimeLeft;
    [SerializeField]
    float punchStrength;
    bool hasPunchInput = false;
    #endregion
    private int isWithinHand(GameObject checkObject)
    {
        int returnInt = -1;
        for(int i = 0; i < withinHand.Count; i++)
        {
            if (checkObject == withinHand[i] && checkObject != gameObject)
            {
                returnInt = i;
                break;
            }
        }
        return returnInt;
    }

    private void OnTriggerEnter(Collider other)
    {
        int getIndex = isWithinHand(other.gameObject);
        if(getIndex == -1 && other.gameObject != player)
        {
            withinHand.Add(other.gameObject);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        int getIndex = isWithinHand(other.gameObject);
        if (getIndex == -1 && other.gameObject != player)
        {
            withinHand.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        int getIndex = isWithinHand(other.gameObject);
        if (getIndex != -1)
        {
            withinHand.Remove(other.gameObject);
        }
    }

    #region grabFuns
    // Grab 
    public void grabCommand(Vector2 angle)
    {
        if (hasGrabbed)
        {
            throwObject(letGoStrength, angle);
        }
        else
        {
            grabTimeLeft = grabTime;
        }
    }

    // Grabs that single object
    private void grabObject(GameObject grabbed)
    {
        grabbedObject = grabbed;
        grabbedPhysics = grabbed.GetComponent<Rigidbody>();
        originalLayer = grabbed.layer;
        grabbedPhysics.useGravity = false;
        grabbed.layer = grabbedLayer;
        hasGrabbed = true;
    }
    // Checks if an object within range can be grabbed
    // Once a grabbable object can be found it grabs it
    private bool attemptGrab()
    {
        for(int i = 0; i < withinHand.Count; i++)
        {
            if (withinHand[i].tag == "GrabObject")
            {
                grabObject(withinHand[i]);
                return true;
            }
        }
        return false;
    }
    #endregion
    #region throwFuns
    public void throwCommand(Vector2 angle)
    {
        if (hasGrabbed)
        {
            throwObject(throwStrength, angle);
        }
    }

    // Throws a currently grabbed object
    private void throwObject(float throwStrength , Vector2 angle)
    {
        grabTimeLeft = 0;
        grabbedObject.layer = originalLayer;
        hasGrabbed = false;
        Vector3 pushDir = mathHelper.getVectorFromAngle(throwStrength, angle);
        grabbedPhysics.velocity = Vector3.zero;
        grabbedPhysics.AddForce(pushDir, ForceMode.Impulse);
        grabbedPhysics.useGravity = true;
    }
    #endregion
    // Punches in front of the player
    public void punchCommand(Vector2 angle)
    {
        if (!hasGrabbed && grabTimeLeft <= 0)
        {
            checkPunch(angle);
        }
    }
    private void checkPunch(Vector2 angle)
    {
        grabTimeLeft = 0;
        punchCooldownTimeLeft = punchCooldownTime;
        for (int i = 0; i < withinHand.Count; i++)
        {
            punchObject(withinHand[i], angle);
        }
    }
    private void punchObject(GameObject givenObject,Vector2 angle)
    {
        Rigidbody givenPhysics = givenObject.GetComponent<Rigidbody>();
        if (givenPhysics != null)
        {
            Vector3 pushDir = mathHelper.getVectorFromAngle(punchStrength, angle);
            givenPhysics.AddForce(pushDir, ForceMode.Impulse);
        }
    }
    void Start()
    {
        getAngle = Camera.main.GetComponent<playerCam>();
    }
    // Update is called once per frame
    void Update()
    {
        debugValueSys.display("list", debugValueSys.listToString(withinHand));

        if (isBeingControlledByPlayer)
        {
            hasGrabCommand = false;
            float grabInput = Input.GetAxisRaw("Grab");
            hasThrownCommand = false;
            float throwInput = Input.GetAxisRaw("Throw");
            hasPunchCommand = false;
            float punchInput = Input.GetAxisRaw("Punch");

            // Grab Input
            if (grabInput != 0)
            {
                if (!hasGrabInput)
                {
                    hasGrabCommand = true;
                    grabCommand(getAngle.getAngleVec());
                }
                hasGrabInput = true;
            }
            else if (grabInput == 0)
            {
                hasGrabInput = false;
            }

            // Throw Input
            if (throwInput != 0)
            {
                if (!hasThrownInput)
                {
                    hasThrownCommand = true;
                    throwCommand(getAngle.getAngleVec());
                }
                hasThrownInput = true;
            }
            else if (throwInput == 0)
            {
                hasThrownInput = false;
            }

            // Punch Input
            if (punchInput != 0)
            {
                if (!hasPunchInput)
                {
                    hasPunchCommand = true;
                    punchCommand(getAngle.getAngleVec());
                }
                hasPunchInput = true;
            }
            else if (punchInput == 0)
            {
                hasPunchInput = false;
            }
        }

        // Grab lingering hitbox
        if(grabTimeLeft > 0)
        {
            grabTimeLeft -= Time.deltaTime;
            if (attemptGrab())
            {
                grabTimeLeft = 0;
            }
        }

        // Punch cooldown
        if(punchCooldownTimeLeft > 0)
        {
            punchCooldownTimeLeft -= Time.deltaTime;
        }

        // Punch lingering hitbox
        if(punchTimeLeft > 0)
        {
            punchTimeLeft -= Time.deltaTime;
            checkPunch(getAngle.getAngleVec());
        }

        // Grab 
        if (hasGrabbed)
        {
            // lock movement if close enough
            float distance = mathHelper.distance(grabbedObject.transform.position, gameObject.transform.position);
            if (distance > 0.2)
            {
                // initial acceleration
                Vector3 direction = mathHelper.getVectorFromAngle(adjustForce * Mathf.Pow(distance, distanceToAdjustRatio), mathHelper.getAngleBetweenVec(grabbedObject.transform.position, gameObject.transform.position));

                // reversal speedup
                if ((direction.x > 0) != (grabbedPhysics.velocity.x > 0))
                {
                    direction.x *= reversalSpeedUp;
                    //grabbedPhysics.velocity = new Vector3(0, grabbedPhysics.velocity.y, grabbedPhysics.velocity.z);
                }
                if ((direction.y > 0) != (grabbedPhysics.velocity.y > 0))
                {
                    direction.y *= reversalSpeedUp;
                    //grabbedPhysics.velocity = new Vector3(grabbedPhysics.velocity.x, 0, grabbedPhysics.velocity.z);
                }
                if ((direction.z > 0) != (grabbedPhysics.velocity.z > 0))
                {
                    direction.z *= reversalSpeedUp;
                    //grabbedPhysics.velocity = new Vector3(grabbedPhysics.velocity.x, grabbedPhysics.velocity.y, grabbedPhysics.velocity.z);
                }
                grabbedPhysics.AddForce(direction, ForceMode.Acceleration);
                if (distance > distanceUntilBreak)
                {
                    throwObject(0, Vector2.zero);
                }
            }
            else
            {
                grabbedPhysics.velocity = grabbedPhysics.velocity / 2;
                grabbedPhysics.MovePosition(gameObject.transform.position);
            }  
        }
    }
}
