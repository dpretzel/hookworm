using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBaseTrigger : MonoBehaviour {

    //private static PlayerCamera playerCameraScript;
    public float playerFocus = .5f;

    private void Start()
    {
        //playerCameraScript = Camera.main.gameObject.GetComponent<PlayerCamera>();
    }

    public Vector3 calculateResultingLocation(GameObject player)
    {
        float newX = (player.transform.position.x * playerFocus) + (gameObject.transform.position.x * (1 - playerFocus));
        float newY = (player.transform.position.y * playerFocus) + (gameObject.transform.position.y * (1 - playerFocus));
        return new Vector3(newX, newY, Camera.main.transform.position.z);
    }

    //Coroutine that snaps the camera to the player when started.
    IEnumerator followPlayer(GameObject player)
    {
        while(true)
        {
            float newX = ( player.transform.position.x * playerFocus ) + ( gameObject.transform.position.x * ( 1 - playerFocus ) );
            float newY = ( player.transform.position.y * playerFocus ) + ( gameObject.transform.position.y * ( 1 - playerFocus ) );
            Camera.main.transform.position = new Vector3(newX, newY, Camera.main.transform.position.z);
            //Camera.main.gameObject.GetComponent
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            PlayerCamera.addTrigger(this);
            //StartCoroutine( followPlayer( collision.gameObject ) );
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //StopCoroutine( followPlayer( null ) );
            //StopAllCoroutines();
            PlayerCamera.removeTrigger(this);
        }
    }

}
