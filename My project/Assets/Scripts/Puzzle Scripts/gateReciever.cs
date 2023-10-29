using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gateReciever : rewindBase
{
    [SerializeField]
    GameObject transformActivate;
    [SerializeField]
    GameObject transformDeactivate;
    [SerializeField]
    float adjustSpeed;
    [SerializeField]
    bool activated;
    [SerializeField]
    bool activatedSet = false;

    public void activate()
    {
        activated = true;
    }
    public void deactivate()
    {
        activated = false;
    }
    // rewind and set
    public override void rewindObject()
    {
        base.rewindObject();
        activated = activatedSet;
    }

    // sets the button
    public override void setObject()
    {
        base.setObject();
        activatedSet = activated;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentDestination;
        if (activated)
        {
            currentDestination = transformActivate.transform.position;
        }
        else
        {
            currentDestination = transformDeactivate.transform.position;
        }
        if (mathHelper.distance(gameObject.transform.position, currentDestination) > 0.05)
        {
            Vector3 direction = mathHelper.getVectorFromAngle(adjustSpeed * Time.deltaTime, mathHelper.getAngleBetweenVec(gameObject.transform.position, currentDestination));
            gameObject.transform.position += direction;
        }
        else
        {
            gameObject.transform.position = currentDestination;
        }
    }
}
