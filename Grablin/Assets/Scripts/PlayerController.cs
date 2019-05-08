using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private GrappleHook grappleHook;

    void Start()
    {
        grappleHook = gameObject.GetComponent<GrappleHook>();
    }

    void checkSwingingInput()
    {

        if (Input.GetButtonUp("Fire1"))
        {
            grappleHook.release();
        }

        switch (Input.GetAxis("Vertical").CompareTo(0))
        {
            case 1:
                grappleHook.contract();
                break;
            case -1:
                grappleHook.loosen();
                break;
        }

        //if (Input.GetButtonDown("Horizontal"))
        //{
            int hor = Input.GetAxis("Horizontal").CompareTo(0);
            if (hor == 1)
                grappleHook.swingRight();
            else if (hor == -1)
                grappleHook.swingLeft();
        //}

    }


    void checkFallingInput()
    {

        /*
        switch (getDpadDirection())
        {
            case (1): //right pressed
                break;
            case (4): //up-right pressed
                break;
            case (3): //up pressed
                break;
            case (2): //up-left pressed
                break;
            case (-1): //left pressed
                break;
            case (-4): //down-left pressed
                break;
            case (-3): //down pressed
                break;
            case (-2): //down-right pressed
                break;
        }*/

        if (Input.GetButtonDown("Fire1"))
        {
            grappleHook.fire( Direction.getDpadDirection() );
        }

    }


    void Update () {
        if (grappleHook.swinging)
            checkSwingingInput();
        else
            checkFallingInput();
    }
}
