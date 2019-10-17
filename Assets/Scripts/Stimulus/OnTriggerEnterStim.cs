using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerEnterStim : Stimulus
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        receptor.Invoke();
    }
}
