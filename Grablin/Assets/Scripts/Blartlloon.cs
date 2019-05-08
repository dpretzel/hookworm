using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blartlloon : MonoBehaviour
{
    public float lift = -1;
    public bool randomColor = true;
    public Color color;

    private void Start()
    {
        if (randomColor) { color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)); }
        gameObject.GetComponent<SpriteRenderer>().color = color;
        gameObject.GetComponent<Rigidbody2D>().gravityScale = -lift;
    }
}
