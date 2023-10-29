using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikeReciever : rewindBase
{
    Collider cacheCollider;
    [SerializeField]
    bool reverse;
    bool originalTrigger;
    public void activate()
    {
        cacheCollider.isTrigger = !reverse;
    }
    public void deactivate()
    {
        cacheCollider.isTrigger = reverse;
    }
    // rewind and set
    public override void rewindObject()
    {
        base.rewindObject();
        cacheCollider.isTrigger = originalTrigger;
    }

    // sets the button
    public override void setObject()
    {
        base.setObject();
        originalTrigger = cacheCollider.isTrigger;
    }
    private void Start()
    {

        cacheCollider = gameObject.GetComponent<Collider>();
        cacheCollider.isTrigger = reverse;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<playerController>().death();
        }
    }
}
