using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : Parryable
{
    private float timeToRegenerate = 1;

    public override bool Parry(Transform initiator)
    {
        bool success = base.Parry(initiator);
        if (state == ParryFSM.PARRYABLE)
        {
            // pop particles / sound?
            StartCoroutine(regenerate());
        }
        return success;
    }

    private IEnumerator regenerate()
    {
        state = ParryFSM.UNPARRYABLE;
        for (float i = 0; i < timeToRegenerate; i+=Time.deltaTime)
        {
            transform.localScale = Vector2.one * (i / timeToRegenerate);
            yield return null;
        }
        state = ParryFSM.PARRYABLE;
    }
}
