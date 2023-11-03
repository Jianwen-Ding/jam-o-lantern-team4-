using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonScript : rewindBase
{
    public bool isActivated = false;
    public bool isTimed;
    public bool isToggle;
    [SerializeField]
    float timeUntilDeactivate;
    float timeUntilDeactivateLeft;

    // rewind vars
    float timeUntilDeactivateSet;
    bool isActivatedSet;

    // button events var
    public delegate void buttonActivate();
    public event buttonActivate activateEvent;

    public delegate void buttonDeactivate();
    public event buttonDeactivate deactivateEvent;

    // activates the button
    public void activateButton()
    {
        if (isTimed)
        {
            timeUntilDeactivateLeft = timeUntilDeactivate; 
        }
        isActivated = true;
        if (activateEvent != null)
        {
            activateEvent();
        }
    }

    // deactivates the button
    public void deactivateButton()
    {
        isActivated = false;
        if (deactivateEvent != null)
        {
            deactivateEvent();
        }
    }

    // rewinds the button
    public override void rewindObject()
    {
        base.rewindObject();
        isActivated = isActivatedSet;
        isActivated = isActivatedSet;
    }

    // sets the button
    public override void setObject()
    {
        base.setObject();
        timeUntilDeactivateSet = timeUntilDeactivateLeft;
        isActivatedSet = isActivated;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (isTimed)
        {
            if (timeUntilDeactivateLeft >= 0)
            {
                timeUntilDeactivateLeft -= Time.deltaTime;
                if(timeUntilDeactivateLeft <= 0)
                {
                    deactivateButton();
                }
            }
        }
    }
}
