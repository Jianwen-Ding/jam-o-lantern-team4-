using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rewindPhysics : rewindBase
{
    public Vector3 originalVelocity;
    public Rigidbody baseRigid;

    // sets velocity to original
    public override void rewindObject()
    {
        base.rewindObject();
        baseRigid.velocity = originalVelocity;
    }

    // records current velocity and rigidbody
    public override void setObject()
    {
        base.setObject();
        baseRigid = gameObject.GetComponent<Rigidbody>();
        originalVelocity = baseRigid.velocity;
    }
}
