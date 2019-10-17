using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Stimulus : MonoBehaviour
{
    //public Object receptor;

    public UnityEvent receptor;

    void Awake()
    {
        if (receptor == null)
            receptor = new UnityEvent();
    }

}
