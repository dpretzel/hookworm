using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnParriedStim : Stimulus
{
    private void OnEnable()
    {
        GetComponent<Parryable>().onParry += receptor.Invoke;
    }

    private void OnDisable()
    {
        GetComponent<Parryable>().onParry -= receptor.Invoke;
    }
}
