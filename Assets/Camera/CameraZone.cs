using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CameraZone : MonoBehaviour {

    public float playerFocus = .5f; //how much the camera follows the player (higher = follow player, lower = stationary)
    public float followSpeed = .5f; //how snappy the camera is
    private Camera frame; //the camera shot to pan to

    private void Start()
    {
        frame = gameObject.GetComponentInChildren<Camera>();
        playerFocus = Mathf.Clamp(playerFocus, 0, 1);
        followSpeed = Mathf.Clamp(followSpeed, 0, 1);
    }

    //Calculates the camera location when the player is inside of this zone.
    //This is compounded with the results from all other zones the player is inside.
    //The final camera location calculation is done inside the PlayerCamera script attached to the actual camera.
    public CameraSettings calculateResultingSettings(GameObject player)
    {
        CameraSettings result = CameraSettings.extractSettings( frame );
        result.x = (player.transform.position.x * playerFocus) + (result.x * (1 - playerFocus));
        result.y = (player.transform.position.y * playerFocus) + (result.y * (1 - playerFocus));
        return result;
    }

}
