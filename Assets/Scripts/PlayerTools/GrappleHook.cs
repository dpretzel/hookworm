using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHook : MonoBehaviour
{
    enum GrappleHookFSM
    {
        HOOKED,
        UNHOOKED
    }

    /*
     * delegates that hold function to be called
     * the <Grapplable> means that these functions take a Grapplable object as a parameter
     * these return void
     */
    public Action<Grapplable> onSuccessfulGrapple;
    public Action whileGrappled;
    public Action<Grapplable> onRelease;

    public GameObject hookMasterPrefab;
    private HookPicker hookPicker;

    private LineRenderer lineRenderer;

    private ConstantForce2D force;
    private Rigidbody2D rb;
    private DistanceJoint2D ropeJoint;

    private GrappleHookFSM state = GrappleHookFSM.UNHOOKED;
    public float minRopeLength = 3;
    public float maxRopeLength = 35;
    public float contractSpeed = .2f;
    public float loosenSpeed = .15f;
    private Grapplable hookedOn;

    private void Awake()
    {
        force = GetComponent<ConstantForce2D>();
        rb = GetComponent<Rigidbody2D>();
        ropeJoint = GetComponent<DistanceJoint2D>();

        lineRenderer = GetComponent<LineRenderer>();
        GetComponent<LineRenderer>().positionCount = 2;

        hookPicker = Instantiate<GameObject>(hookMasterPrefab).GetComponent<HookPicker>();
    }

    private void OnEnable()
    {
        InputManager i = InputManager.instance;
        i.onPrimaryPressed += Fire;
        i.onPrimaryReleased += Release;
    }

    private void OnDisable()
    {
        InputManager i = InputManager.instance;
        i.onPrimaryPressed -= Fire;
        i.onPrimaryReleased -= Release;
    }

    public bool IsHooked() { return state == GrappleHookFSM.HOOKED; }

    public void Fire()
    {
        hookedOn = hookPicker.getBestHook();
        if (hookedOn) {

            if (hookedOn.Grapple())
                state = GrappleHookFSM.HOOKED;
            if (state == GrappleHookFSM.UNHOOKED)
                return;

            if (onSuccessfulGrapple != null)
                onSuccessfulGrapple(hookedOn);

            // Start listening for keypresses to alter rope length.
            InputManager.instance.onUpPressed += Contract;
            InputManager.instance.onDownPressed += Loosen;

            //enable the rope physics
            ropeJoint.distance = Vector2.Distance(transform.position, hookedOn.transform.position);
            ropeJoint.connectedBody = hookedOn.GetComponent<Rigidbody2D>();
            ropeJoint.enabled = true;
            
            //draw the rope
            lineRenderer.enabled = true;
            StartCoroutine(HoldOn());

            // get rid of this
            //check and call grapple events
            /*
            Event target = hookedOn.GetComponent<Event>();
            if (target)
                if (target.type == EEventType.ON_GRAPPLE)
                    target.target.SendMessage(target.function);
            */
            //MAKE IT HANDLE MULTIPLE EVENTS ON 1 GO
        }
    }

    public void Release()
    {
        state = GrappleHookFSM.UNHOOKED;
        ropeJoint.enabled = lineRenderer.enabled = false;
        if (onRelease != null)
            onRelease(hookedOn);

        // Stop listening for keypresses to alter rope length.
        InputManager.instance.onUpPressed -= Contract;
        InputManager.instance.onDownPressed -= Loosen;

        StopCoroutine(HoldOn());
        if (hookedOn)
        {
            //get rid of this
            //check and call de-grapple events
            /*
            Event target = hookedOn.GetComponent<Event>();
            if (target)
                if(target.type == EEventType.ON_RELEASE)
                    target.target.SendMessage(target.function);
            */
            hookedOn.Release();
            hookedOn = null;
        }
    }

    private void ChangeRopeLength(float pos)
    {
        ropeJoint.distance = Mathf.Clamp(ropeJoint.distance + contractSpeed * Mathf.Sign(pos), minRopeLength, maxRopeLength / 2);
    }

    public void Contract() {

        float beforeDist = ropeJoint.distance;
        bool taut = IsTaut();

        ChangeRopeLength(-1);
        //print(ropeJoint.anchor);

        if (taut)
        {
            float strength = beforeDist - ropeJoint.distance;
            Vector2 v = Vector3.Normalize(ropeJoint.connectedBody.transform.position - this.transform.position);
            v *= strength * 2500;
            Debug.DrawRay(this.transform.position, v, Color.cyan);
            rb.AddForce(v);
        }
    }

    private bool IsTaut()
    {
        float rotateBy = Mathf.Deg2Rad * this.transform.rotation.eulerAngles.z;
        Vector2 rotatedVector = new Vector2(
                                            Mathf.Cos(rotateBy) * ropeJoint.anchor.x - Mathf.Sin(rotateBy) * ropeJoint.anchor.y,
                                            Mathf.Sin(rotateBy) * ropeJoint.anchor.x + Mathf.Cos(rotateBy) * ropeJoint.anchor.y);
        //Debug.DrawLine(this.transform.position, this.transform.position + (Vector3)connectedPoint, Color.cyan);
        Vector2 connectedPoint = (Vector2)this.transform.position + rotatedVector;
        float distFromHook = Vector2.Distance(connectedPoint, ropeJoint.connectedBody.transform.position);
        //print(String.Format("dist: {0}, ropeLength: {1}", distFromHook, ropeJoint.distance));
        return Mathf.Approximately(distFromHook, ropeJoint.distance);
    }

    public void Loosen() { ChangeRopeLength(1); }

    private IEnumerator HoldOn()
    {
        while(true)
        {
            // check if what we're hooked onto still exists (in case it gets destroyed while we're swinging)
            if (!hookedOn)
            {
                Release();
                break;
            }
            // draw
            lineRenderer.SetPosition(0, gameObject.transform.position);
            lineRenderer.SetPosition(1, hookedOn.transform.position);
            yield return null;
        }
    }
}
