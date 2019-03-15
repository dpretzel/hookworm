using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeController : MonoBehaviour {

    public GameObject player;

    public GameObject constraintPrefab;
    private GameObject constraint;
    private float dist;
    private Vector3 hookLoc;

    public LineRenderer lineRenderer;

    void Start()
    {
        constraint = Instantiate<GameObject>(constraintPrefab);
    }


    // Update is called once per frame
    // Finds whether or not the left mouse button is pressed 
    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Release();
        }
        lineRenderer.SetPosition(0, player.transform.position);
        lineRenderer.SetPosition(1, hookLoc);
    }

    /*private void FixedUpdate()
    {
        
    }*/
    
    //As the name suggests, this fires the rope in the direction of the mouse,
    //and if it hits a collider it will create the rope with an anchor.
    void Fire()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 position = player.transform.position;
        Vector3 direction = mousePosition - position;

        RaycastHit2D hit = Physics2D.Raycast(position, direction, Mathf.Infinity);

        if (hit.collider != null)
        {
            dist = Mathf.Sqrt(Mathf.Pow(mousePosition.x - position.x, 2) + Mathf.Pow(mousePosition.y - position.y, 2));
            dist *= 2.3f;
            if (dist < 40) {
                constraint.SetActive(true);
                hookLoc = new Vector3(mousePosition.x, mousePosition.y, 0);
                constraint.transform.position = hookLoc;
                constraint.transform.localScale = new Vector3(dist, dist, 0);
                lineRenderer.enabled = true;
            }
        }
    }

    //Destroys the Rope
    void Release()
    {
        //constraint.transform.position = new Vector3(-100, -100, 0);
        constraint.SetActive(false);
        lineRenderer.enabled = false;
    }
}
