using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script attaches to a trigger collider and communicates with the FocusManager to apply this class' settings to the player's Camera.
[ExecuteAlways]
[System.Serializable]
public class Focuser : MonoBehaviour
{
    public float zoom = 30; // how zoomed in the camera should be
    public float playerFocus = .5f; //how much the camera follows the player (higher = follow player, lower = stationary)
    public float snappiness = .5f; //how snappy the camera is

    /* Calculates the camera location when the player is inside of this FocusZone.
     * This is compounded with the results from all other FocusZones the player is inside.
     * The final camera location calculation is done inside the FocusManager script attached to the Managers GO in the "RequiredElements" prefab.*/
    public Vector3 CalculateResultingPosition()
    {
        Vector3 result = new Vector3();
        Vector3 playerPos = PlayerManager.instance.getPlayer().transform.position;

        result.x = (playerPos.x * playerFocus) + (result.x * (1 - playerFocus));
        result.y = (playerPos.y * playerFocus) + (result.y * (1 - playerFocus));
        result.z = -16;
        return result;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            FocusManager.instance.AddFocuser(this);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            FocusManager.instance.RemoveFocuser(this);
    }
}
