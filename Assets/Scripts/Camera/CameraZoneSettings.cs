using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoneSettings
{

    public float playerFocus, followSpeed;

    public CameraZoneSettings() { playerFocus = followSpeed = 0; }

    public CameraZoneSettings(float playerFocus, float followSpeed)
    {
        this.playerFocus = playerFocus;
        this.followSpeed = followSpeed;
    }

    public CameraZoneSettings addTo(CameraZoneSettings otherCamZoneSet)
    {
        playerFocus += otherCamZoneSet.playerFocus;
        followSpeed += otherCamZoneSet.followSpeed;
        return this;
    }

    //Divides this 
    public CameraZoneSettings divBy(float div)
    {
        playerFocus /= div;
        followSpeed /= div;
        return this;
    }

    //Given a CameraZone, takes its playerFocus and followSpeed and returns a CameraZoneSettings object.
    public static CameraZoneSettings extractSettings(CameraZone camZone) { return new CameraZoneSettings(camZone.playerFocus, camZone.followSpeed); }

    //Blends the a List of CameraZones' settings and returns the composite
    public static CameraZoneSettings blendSettings(List<CameraZone> list )
    {
        CameraZoneSettings camZoneSetSum = new CameraZoneSettings();
        foreach (CameraZone zone in list)
            camZoneSetSum.addTo( extractSettings( zone ) );
        return camZoneSetSum.divBy(list.Count);
    }

}
