using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class TransformLerper : MonoBehaviour
{

    [Tooltip("When true, the movement begins at the start of the scene.")]
    public bool activeOnStart = true;
    public float duration = 1;

    protected Vector3 origin;
    public Vector3 offset;
    protected Vector3 destination;
    [Tooltip("When true, the offset is taken from 0,0. When false, offset is relative to the object's current transform.")]
    public bool offsetAbsolute = false;

    [Tooltip("When true, returns to origin after completing.")]
    public bool loop = false;
    protected int dir = 1; // Whether going towards dest or orig

    [Tooltip("How long to wait between transform being triggered and beginning to move (does not occur each repeat).")]
    public float startDelay = 0;
    [Tooltip("How long to wait after each leg completed.")]
    public float delay = 0;

    [Tooltip("How many times to repeat. -1 for unlimited.")]
    public int repititions = 1;
    protected int repititionsLeft;
    [Tooltip("When true, transform can be triggered again.")]
    public bool retriggerable = true;

    [Tooltip("When true, can only trigger the transform once.")]
    protected bool triggered = false; // Has the movement already been triggered?
    protected bool done = false; // Has the movement already completed?

    public UnityEvent legComplete;
    public UnityEvent loopComplete;

    private void Awake()
    {
        if (legComplete == null)
            legComplete = new UnityEvent();
        if (loopComplete == null)
            loopComplete = new UnityEvent();
    }

    protected virtual void Start()
    {
        repititionsLeft = repititions;
        if (activeOnStart)
            Transform();
    }

    public void Transform()
    {
        if ( repititionsLeft != 0 && ! triggered ) // Only refuse to fire if oneshot and already triggered
            StartCoroutine(LerpCoroutine());
    }

    protected abstract Vector3 GetOrigin();
    protected abstract void SetToDestination();
    protected abstract void SetToOrigin();
    protected abstract void Lerp(float t);

    private IEnumerator LerpCoroutine()
    {
        origin = GetOrigin();
        destination = offsetAbsolute ? offset : origin + offset; //if destinationRelative, add it to destination; otherwise leave it be
        triggered = true;
        float t = 0; // Completion ratio of 0 to 1

        yield return new WaitForSeconds(startDelay);

        while (repititionsLeft != 0)
        {

            if (t > 1) // Done with first leg
            {

                legComplete.Invoke();

                if (loop) // Turn around and go back
                {
                    yield return new WaitForSeconds(delay);
                    dir = -1;
                    t = 1;
                }
                else // Not looping
                {
                    // fix so doesn't doesn't decrement negative
                    if (--repititionsLeft != 0) // Jump back to beginning if repititions not exhausted
                    {
                        t = 0;
                    }
                    else // Repititions exhausted
                    {
                        SetToDestination();
                    }
                }

            }
            else if (t < 0) // Done with second leg (can assume looping is true)
            {

                loopComplete.Invoke();

                if (--repititionsLeft != 0)
                {
                    yield return new WaitForSeconds(delay);
                    dir = 1;
                    t = 0;
                }
                else // Finish
                {
                    SetToOrigin();
                }

            }
            else  // In the middle of movement
            {
                Lerp(t);
                t += Time.deltaTime * dir / duration;
            }

            yield return null;
        }

        triggered = false;
        if (retriggerable)
        {
            repititionsLeft = repititions;
            t = 0;
            dir = 1;
        }

    }
}
