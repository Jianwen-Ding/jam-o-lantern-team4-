using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pressurePlate : buttonScript
{
    [SerializeField]
    Material defaultMat;
    [SerializeField]
    Material activatedMat;
    [SerializeField]
    MeshRenderer renderPlate;
    List<GameObject> withinHand = new List<GameObject>();
    private int isWithinHand(GameObject checkObject)
    {
        int returnInt = -1;
        for (int i = 0; i < withinHand.Count; i++)
        {
            if (checkObject == withinHand[i] && checkObject != gameObject)
            {
                returnInt = i;
                break;
            }
        }
        return returnInt;
    }

    private bool hasWeightedObject()
    {
        for (int i = 0; i < withinHand.Count; i++)
        {
            if (withinHand[i].GetComponent<WeightedObject>() != null)
            {
                return true;
            }
        }
        return false;
    }


    private void OnTriggerEnter(Collider other)
    {
        int getIndex = isWithinHand(other.gameObject);
        if (getIndex == -1)
        {
            withinHand.Add(other.gameObject);
            if (hasWeightedObject())
            {
                activateButton();
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        int getIndex = isWithinHand(other.gameObject);
        if (getIndex == -1)
        {
            withinHand.Add(other.gameObject);
            if (hasWeightedObject())
            {
                activateButton();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        int getIndex = isWithinHand(other.gameObject);
        if (getIndex != -1)
        {
            withinHand.Remove(other.gameObject);
            if (!hasWeightedObject())
            {
                deactivateButton();
            }
        }
    }

    private void Start()
    {
        renderPlate = gameObject.GetComponent<MeshRenderer>();
    }

    public override void Update()
    {
        base.Update();
        if (isActivated)
        {
            renderPlate.material = activatedMat;
        }
        else
        {
            renderPlate.material = defaultMat;
        }
    }
}
