using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusManager : MonoBehaviour
{
    [Tooltip("When the player isn't inside a FocusZone, this is how snappy the Camera should be.")]
    public float unfocusedSnappiness = .5f;
    private float unfocusedSize; // on Start, get's value from Camera

    private List<Focuser> triggeredFocusers = new List<Focuser>();
    private Camera playerCam;

    private Vector3 lastCamPos = new Vector3();
    private float lastCamZoom;

    public static FocusManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playerCam = Camera.main;
        unfocusedSize = playerCam.orthographicSize;
    }

    private void FixedUpdate()
    {

        Vector3 targetCamPos = new Vector3();
        float targetCamZoom = 0;
        float blendedSnappiness = 0;

        if (triggeredFocusers.Count > 0)
        {
            foreach (Focuser f in triggeredFocusers)
            {
                blendedSnappiness += f.snappiness;
                targetCamPos += f.CalculateResultingPosition();
                targetCamZoom += f.zoom;
            }
            blendedSnappiness /= triggeredFocusers.Count;
            targetCamPos /= triggeredFocusers.Count;
            targetCamZoom /= triggeredFocusers.Count;
        }
        else
        {
            targetCamPos = PlayerManager.instance.getPlayer().transform.position + Vector3.back * 16;
            targetCamZoom = unfocusedSize;

            blendedSnappiness = unfocusedSnappiness;
        }


        //position
        playerCam.transform.position = lastCamPos + (targetCamPos - lastCamPos) * blendedSnappiness;
        lastCamPos = playerCam.transform.position;
        //zoom
        playerCam.orthographicSize = (lastCamZoom + (targetCamZoom - lastCamZoom) * blendedSnappiness / 10);
        lastCamZoom = playerCam.orthographicSize;
    }

    public void AddFocuser( Focuser f )
    {
        triggeredFocusers.Add(f);
    }

    public void RemoveFocuser( Focuser f)
    {
        triggeredFocusers.Remove(f);
    }

}
