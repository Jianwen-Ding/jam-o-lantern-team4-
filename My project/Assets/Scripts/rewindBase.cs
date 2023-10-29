using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rewindBase : MonoBehaviour
{
    public Vector3 originalPos;
    public Vector3 originalEulerRot;

    // adds and removes command from events
    private void OnEnable()
    {
        rewindPoint.rewindCommand += rewindObject;
        rewindPoint.setCommand += setObject;
    }

    private void OnDisable()
    {
        rewindPoint.rewindCommand -= rewindObject;
        rewindPoint.setCommand -= setObject;
    }

    // returns object to their original state
    public virtual void rewindObject()
    {
        gameObject.transform.position = originalPos;
        gameObject.transform.rotation = Quaternion.Euler(originalEulerRot);
    }

    // records current state of object
    public virtual void setObject()
    {
        originalPos = gameObject.transform.position;
        originalEulerRot = gameObject.transform.rotation.eulerAngles;
    }
}
