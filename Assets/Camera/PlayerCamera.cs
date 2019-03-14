using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    private static List<CameraBaseTrigger> insideTriggers = new List<CameraBaseTrigger>();
    private static GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public static void addTrigger(CameraBaseTrigger trigger) { insideTriggers.Add(trigger); }
    public static  void removeTrigger(CameraBaseTrigger trigger) { insideTriggers.Remove(trigger); }

    //Return the weighted average location returned by all triggers
    private Vector2 calculateCameraLocationFromTriggers()
    {
        Vector3 locationSum = new Vector3(0, 0, 0);
        foreach (CameraBaseTrigger trigger in insideTriggers)
            locationSum += trigger.calculateResultingLocation(player);

        return locationSum / insideTriggers.Count;
    }
	
	// Update is called once per frame
	void Update () {
        //If player is not inside a trigger, camera should follow him/her/zer/zim/zoom/zibitty bob hey kids mmmmmm jell-o
		if(insideTriggers.Count > 0)
        {
            Vector2 weightedPos = calculateCameraLocationFromTriggers();
            transform.position = new Vector3( weightedPos.x, weightedPos.y, gameObject.transform.position.z );
        }
        else
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        }
	}
}
