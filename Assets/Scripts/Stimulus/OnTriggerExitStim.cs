using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerExitStim : Stimulus
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        receptor.Invoke();
    }
}
