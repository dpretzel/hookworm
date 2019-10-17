using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionLerper : TransformLerper
{
    protected override void Lerp(float t)
    {
        transform.position = Vector2.Lerp(origin, destination, t);
    }

    protected override Vector3 GetOrigin()
    {
        return transform.position;
    }

    protected override void SetToDestination()
    {
        transform.position = destination;
    }

    protected override void SetToOrigin()
    {
        transform.position = origin;
    }
}
