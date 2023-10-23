using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handController : MonoBehaviour
{
    GameObject grabbedObject;
    List<GameObject> withinHand;

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
    // Update is called once per frame
    void Update()
    {
        float grabInput = Input.GetAxisRaw("Grab");
        float throwInput = Input.GetAxisRaw("Throw");
        float punchInput = Input.GetAxisRaw("Punch");
    }
}
