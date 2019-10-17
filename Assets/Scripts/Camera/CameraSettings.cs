using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is used to easily extract, manipulate, and pass various settings used in Cameras such as x, y (position), and size (zoom).
//This makes it easy to average the settings of various CameraZones.
public class CameraSettings {

    public float x, y, size;

    public CameraSettings() { x = y = size = 0; }

    public CameraSettings(float x, float y, float size)
    {
        this.x = x;
        this.y = y;
        this.size = size;
    }

    //Adds another CameraSettings' values to this.
    public CameraSettings addTo(CameraSettings otherCamSet)
    {
        x += otherCamSet.x;
        y += otherCamSet.y;
        size += otherCamSet.size;
        return this;
    }

    //Divides this 
    public CameraSettings divBy(float div)
    {
        x /= div;
        y /= div;
        size /= div;
        return this;
    }

    //Given a Camera, takes its 2D position and size and returns a CameraSettings object.
    public static CameraSettings extractSettings(Camera cam) { return new CameraSettings( cam.transform.position.x, cam.transform.position.y, cam.orthographicSize); }

    //Blends the a List of CameraZones' settings and returns the composite
    public static CameraSettings blendSettings(List<CameraZone> list, GameObject player)
    {
        CameraSettings camSetSum = new CameraSettings();
        foreach ( CameraZone zone in list )
            camSetSum.addTo( zone.calculateResultingSettings( player ) );
        return camSetSum.divBy( list.Count );
    }

}
