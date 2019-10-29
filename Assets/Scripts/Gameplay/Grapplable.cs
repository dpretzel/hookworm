using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Grapplable : MonoBehaviour
{
    enum GrapplableFSM
    {
        GRAPPLABLE,
        UNGRAPPLABLE,
        GRAPPLED
    }

    // Actions that objects with a Stimulus may subscribe to
    public Action onGrappled;
    public Action onReleased;

    private GrapplableFSM state = GrapplableFSM.GRAPPLABLE;
    private static Color highlightColor = Color.cyan;

    public void ForceStateChange(bool grapplable)
    {
        if (!grapplable)
        {
            if (state == GrapplableFSM.GRAPPLED)
                PlayerManager.instance.forceRelease();
            state = GrapplableFSM.UNGRAPPLABLE;
        }
        else
            state = GrapplableFSM.GRAPPLABLE;
    }

    public bool IsGrapplable()
    {
        return state == GrapplableFSM.GRAPPLABLE;
    }

    public bool Grapple()
    {
        if (state == GrapplableFSM.GRAPPLABLE)
        {
            state = GrapplableFSM.GRAPPLED;
            if (onGrappled != null)
                onGrappled();
            return true;
        }
        else
            return false;
    }

    public void Release()
    {
        if (state == GrapplableFSM.GRAPPLED)
        {
            state = GrapplableFSM.GRAPPLABLE;
            if (onReleased != null)
                onReleased();
        }
    }

    public void Highlight()
    {
        //transform.GetComponent<SpriteRenderer>().color = highlightColor;
        //print("highlighted!");
    }

    public void Dehighlight()
    {
        //transform.GetComponent<SpriteRenderer>().color = Color.white;
        //print("dehighlighted!");
    }
}
