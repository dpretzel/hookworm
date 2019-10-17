using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGrappledStim : Stimulus
{
    private void OnEnable()
    {
        GetComponent<Grapplable>().onGrappled += receptor.Invoke;
    }

    private void OnDisable()
    {
        GetComponent<Grapplable>().onGrappled -= receptor.Invoke;
    }
}
