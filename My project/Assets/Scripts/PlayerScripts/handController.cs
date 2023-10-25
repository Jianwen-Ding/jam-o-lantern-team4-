using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handController : MonoBehaviour
{
    #region vars
    List<GameObject> withinHand = new List<GameObject>();

    //Grab Vars
    [SerializeField]
    float grabTime;
    float grabTimeLeft = 0;
    [SerializeField]
    float letGoStrength;
    bool hasGrabInput = false;
    int grabbedLayer = 7;
    bool hasGrabbed;
    GameObject grabbedObject;
    int originalLayer;

    //Throw Vars
    [SerializeField]
    float throwStrength;
    bool hasThrownInput = false;

    //Punch Vars
    [SerializeField]
    float punchTime;
    float punchTimeLeft;
    [SerializeField]
    float punchStrength;
    bool hasPunchInput = false;
    #endregion
    private int isWithinHand(GameObject checkObject)
    {
        int returnInt = -1;
        for(int i = 0; i < withinHand.Count; i++)
        {
            if (checkObject == withinHand[i])
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
        if(getIndex == -1)
        {
            withinHand.Add(other.gameObject);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        int getIndex = isWithinHand(other.gameObject);
        if (getIndex == -1)
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

    // Grabs that single object
    private void grabObject(GameObject grabbed)
    {
        grabbedObject = grabbed;
        originalLayer = grabbed.layer;
        grabbed.GetComponent<Collider>().isTrigger = true;
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

    // Throws a currently grabbed object
    private void throwObject(float throwStrength)
    {
        grabTimeLeft = 0;
        grabbedObject.GetComponent<Collider>().isTrigger = false;
        grabbedObject.layer = grabbedLayer;
        hasGrabbed = false;
    }

    // Punches in front of the player
    private void punch()
    {
        grabTimeLeft = 0;
        punchTimeLeft = punchTime;
    }
    // Update is called once per frame
    void Update()
    {
        debugValueSys.display("wow", withinHand.ToString());
        float grabInput = Input.GetAxisRaw("Grab");
        float throwInput = Input.GetAxisRaw("Throw");
        float punchInput = Input.GetAxisRaw("Punch");

        // Grab Input
        if (grabInput != 0)
        {
            if (!hasGrabInput)
            {
                if (hasGrabbed)
                {
                    throwObject(letGoStrength);
                }
                else
                {
                    grabTimeLeft = grabTime;
                }
            }
            hasGrabInput = true;
        }
        else if (grabInput == 0)
        {
            hasGrabInput = false;
        }

        // Throw Code
        if(throwInput != 0)
        {
            if (!hasThrownInput)
            {
                if (hasGrabbed)
                {
                    throwObject(throwStrength);
                }
            }
            hasThrownInput = true;
        }
        else if(throwInput == 0)
        {
            hasThrownInput = false;
        }

        // Punch Code
        if(punchInput != 0)
        {
            if (!hasPunchInput) {
                if (!hasGrabbed && grabTimeLeft <= 0)
                {
                    punch();
                }
            }
            hasPunchInput = true;
        }
        else if(punchInput == 0)
        {
            hasPunchInput = false;
        }

        if(grabTimeLeft > 0)
        {
            grabTimeLeft -= Time.deltaTime;
            if (attemptGrab())
            {
                grabTimeLeft = 0;
            }
        }

        if(punchTimeLeft > 0)
        {
            punchTimeLeft -= Time.deltaTime;
        }

        if (hasGrabbed)
        {
            grabbedObject.transform.position = gameObject.transform.position;
            grabbedObject.transform.rotation = gameObject.transform.rotation;
        }
    }
}
