using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    private float defaultSize; //how zoomed in the Camera should be when not inside a CameraZone
    private static Camera cam; //the Camera component attached to the Camera GameObject
    private static List<CameraZone> insideZones = new List<CameraZone>(); //List of all the CameraZones the player is currently inside
    private static GameObject player;

    private void Start()
    {
        cam = gameObject.GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player");
        defaultSize = cam.orthographicSize;
    }

    //Public-facing functions for interacting with insideZones
    public static void addZone(CameraZone zone) { insideZones.Add( zone ); }
    public static  void removeZone(CameraZone zone) { insideZones.Remove( zone ); }

    //Returns the weighted average of location and size returned by all CameraZones.
    private CameraSettings blendCameraZoneSettings()
    {
        CameraSettings camSetSum = new CameraSettings();
        foreach (CameraZone zone in insideZones)
            camSetSum.addTo(zone.calculateResultingSettings(player));
        return camSetSum.divBy( insideZones.Count );
    }

    // Update is called once per frame
    void Update () {
        //If player is not inside a zone, camera should follow him/her/zer/zim/zoom/zibitty bob hey kids mmmmmm jell-o
		if(insideZones.Count > 0)
        {
            CameraSettings blendedSettings = blendCameraZoneSettings();
            transform.position = new Vector3(blendedSettings.x, blendedSettings.y, -1);
            cam.orthographicSize = blendedSettings.size;
        }
        else
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
            cam.orthographicSize = defaultSize;
        }
	}
}
