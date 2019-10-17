using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crane : MonoBehaviour
{

    float speed = .1f;

    private void Move(float dir)
    {
        transform.position += Vector3.right * dir;
    }
    public void MoveLeft() { Move(-speed); }
    public void MoveRight() { Move(speed); }

    public void Fire()
    {
        print("Firing");
    }
}
