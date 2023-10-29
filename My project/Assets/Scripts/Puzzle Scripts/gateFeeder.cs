using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gateFeeder : buttonReciever
{
    gateReciever cacheGate;
    public override void activate()
    {
        cacheGate.activate();
        base.activate();
    }
    public override void deactivate()
    {
        cacheGate.deactivate();
        base.activate();
    }
    private void Start()
    {
        cacheGate = gameObject.GetComponent<gateReciever>();
    }
}
