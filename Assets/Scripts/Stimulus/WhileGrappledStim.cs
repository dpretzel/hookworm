using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhileGrappledStim : Stimulus
{
    private void OnEnable()
    {
        Grapplable g = GetComponent<Grapplable>();
        g.onGrappled += StartGrapple;
        g.onReleased += StopGrapple;
    }

    private void OnDisable()
    {
        Grapplable g = GetComponent<Grapplable>();
        g.onGrappled -= StartGrapple;
        g.onReleased -= StopGrapple;
    }

    private Coroutine routine;
    private void StartGrapple() { routine = StartCoroutine(Grappling()); }
    private void StopGrapple() { StopCoroutine(routine); }

    private IEnumerator Grappling()
    {
        while (true)
        {
            receptor.Invoke();
            yield return null;
        }
    }
}
