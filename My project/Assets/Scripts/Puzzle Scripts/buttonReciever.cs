using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonReciever : MonoBehaviour
{
    [SerializeField]
    buttonScript givenButton;

    // connects activate and deactivate functions to button events
    void OnEnable()
    {
        givenButton.activateEvent += activate;
        givenButton.deactivateEvent += deactivate;
    }
    void OnDisable()
    {
        givenButton.activateEvent -= activate;
        givenButton.deactivateEvent -= deactivate;
    }

    // when button is activated
    public virtual void activate()
    {
    }

    // when button is deactivated
    public virtual void deactivate()
    {
    }
}
