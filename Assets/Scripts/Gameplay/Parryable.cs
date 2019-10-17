using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parryable : MonoBehaviour
{

    public Action onParry;

    protected enum ParryFSM
    {
        PARRYABLE,
        UNPARRYABLE
    }

    protected ParryFSM state = ParryFSM.PARRYABLE;

    public virtual bool Parry(Transform initiator)
    {
        bool success = true;
        if (state == ParryFSM.PARRYABLE)
        {
            BumpInitiator(initiator.GetComponent<Rigidbody2D>());
            if (onParry != null)
                onParry();
        }
        else
            success = false;
        return success;
    }

    private void BumpInitiator(Rigidbody2D rb)
    {
        rb.velocity = Vector2.zero;
        Vector2 f = rb.transform.position - transform.position;
        f.Normalize();
        f *= 2000;
        rb.AddForce(f);
    }

    public bool IsParryable()
    {
        return state == ParryFSM.PARRYABLE;
    }
}
