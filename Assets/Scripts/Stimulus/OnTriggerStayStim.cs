using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerStayStim : Stimulus
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        receptor.Invoke();
    }
}
