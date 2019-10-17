using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationLerper : TransformLerper
{

    protected override void Lerp(float t)
    {
        transform.rotation = Quaternion.Euler(Vector3.Lerp(origin, destination, t));
    }

    protected override Vector3 GetOrigin()
    {
        return transform.rotation.eulerAngles;
    }

    protected override void SetToDestination()
    {
        transform.rotation = Quaternion.Euler(destination);
    }

    protected override void SetToOrigin()
    {
        transform.rotation = Quaternion.Euler(origin);
    }

}
