using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float maxLength = 20f;

    public GameObject constraintPrefab;
    private GameObject constraint;
    private float dist;
    private GameObject hookedGO;

    public LineRenderer lineRenderer;

    void setupRope()
    {
        gameObject.GetComponent<LineRenderer>().positionCount = 2;
    }

    void setupConstraint()
    {
        constraint = Instantiate<GameObject>(constraintPrefab);
    }

    void Start()
    {
        setupRope();
        setupConstraint();
    }

    void checkInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            Release();
        }
        //if(Input.GetButtonDown)
    }

    // Update is called once per frame
    void Update () {

        checkInput();
        
        if(hookedGO){
            lineRenderer.SetPosition(0, gameObject.transform.position);
            lineRenderer.SetPosition(1, hookedGO.transform.position);
        }
    }

    void Fire()
    {
        Vector3 position = gameObject.transform.position;
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
