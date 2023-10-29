using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikeFeeder : buttonReciever
{
    spikeReciever cacheSpike;
    public override void activate()
    {
        cacheSpike.activate();
        base.activate();
    }
    public override void deactivate()
    {
        cacheSpike.deactivate();
        base.activate();
    }
    private void Start()
    {
        cacheSpike = gameObject.GetComponent<spikeReciever>();
    }
}
