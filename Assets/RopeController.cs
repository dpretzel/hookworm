using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeController : MonoBehaviour {

    public GameObject player;

    public float maxLength = 20f;

    public GameObject constraintPrefab;
    private GameObject constraint;
    private float dist;
    private GameObject hookedGO;

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

        if(hookedGO){
            lineRenderer.SetPosition(0, player.transform.position);
            lineRenderer.SetPosition(1, hookedGO.transform.position);
        }
    }

    /*private void FixedUpdate()
    {
        
    }*/
    
    //As the name suggests, this fires the rope in the direction of the mouse,
    //and if it hits a collider it will create the rope with an anchor.
    void Fire()
    {
        Vector3 position = player.transform.position;
        Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - position;

        RaycastHit2D hit = Physics2D.Raycast(position, direction, maxLength, 1 << LayerMask.NameToLayer("Hook"));

        if (hit.collider != null)
        {
            hookedGO = hit.collider.gameObject;
            constraint.transform.parent = hookedGO.transform;

            constraint.SetActive(true);
            lineRenderer.enabled = true;

            dist = Mathf.Sqrt(Mathf.Pow(hookedGO.transform.position.x - position.x, 2) + Mathf.Pow(hookedGO.transform.position.y - position.y, 2));
            dist = 1 + dist * 2f;
            constraint.transform.position = hookedGO.transform.position;
            constraint.transform.localScale = new Vector3(dist / hookedGO.transform.localScale.x, dist / hookedGO.transform.localScale.y, 0); //adjust for hooked object's scale
        }
    }

    //Destroys the Rope
    void Release()
    {
        if (hookedGO)
        {
            constraint.SetActive(false);
            lineRenderer.enabled = false;
            hookedGO = null;
        }
    }
}
