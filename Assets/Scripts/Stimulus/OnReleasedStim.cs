using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnReleasedStim : Stimulus
{
    private void OnEnable()
    {
        GetComponent<Grapplable>().onReleased += receptor.Invoke;
    }

    private void OnDisable()
    {
        GetComponent<Grapplable>().onReleased -= receptor.Invoke;
    }
}
