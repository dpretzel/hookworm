using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHook : MonoBehaviour
{
    //public parameters
    public bool swinging { get; set; }
    public float minRopeLength = 2;
    public float maxRopeLength = 20;
    private float idealHookDistance; //maybe set this to last constraint scale? (keep rope length ~same)
    public float expandContractRate = .025f; //how fast the rope will contract/loosen when holding up/down
    private float pullRate = .1f; //to avoid losing velocity when contracting rope, this is the amount the player will "jump" to not get stuck inside circle constraint

    //prefabs to instantiate
    public GameObject ropeConstraintPrefab;
    public GameObject hookableZonePrefab;

    //components
    private GameObject ropeConstraint;
    private LineRenderer lineRenderer;
    private CircleCollider2D hookableZone;
    private Rigidbody2D rb;
    private ConstantForce2D force;

    //hooks and stuff
    private Collider2D[] hookables = new Collider2D[10]; //too few?
    private float dist;
    private GameObject hookedGO;
    //private GameObject oldBest;

    void setupHookableZone()
    {
        hookableZone = Instantiate<GameObject>(hookableZonePrefab).GetComponent<CircleCollider2D>();
        hookableZone.radius = maxRopeLength;
    }

    void setupLineRenderer()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        gameObject.GetComponent<LineRenderer>().positionCount = 2;
    }

    void setupRopeConstraint() { ropeConstraint = Instantiate<GameObject>(ropeConstraintPrefab); }
    void setupRigidBody() { rb = gameObject.GetComponent<Rigidbody2D>(); }
    void setupForce() { force = GetComponent<ConstantForce2D>(); }

    void Start()
    {
        setupLineRenderer();
        setupRopeConstraint();
        setupHookableZone();
        setupRigidBody();
        setupForce();
        idealHookDistance = maxRopeLength * .5f;
    }

    private void Update()
    {
        hookableZone.transform.position = gameObject.transform.position;
        /*GameObject newBest = getBestHook(Direction.getDpadDirection()).gameObject;
        print(newBest);
        if(!newBest.Equals(oldBest))
        {
            oldBest.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
            newBest.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
            oldBest = newBest;
        }*/
        resetSwing();
    }

    //Returns an array of Collider2Ds of GameObjects within maxRopeLength
    private Collider2D[] getHookables()
    {
        hookableZone.transform.position = gameObject.transform.position;
        ContactFilter2D c = new ContactFilter2D();
        c.SetLayerMask(1 << LayerMask.NameToLayer("Hook"));
        hookableZone.OverlapCollider(c, hookables);
        return hookables;
    }

    //Assigns a float value based on the direction we're aiming
    private float rateHook( Collider2D hook, EDirection aimDir)
    {
        float distRating = 1 / Vector2.Distance( new Vector2(transform.position.x, transform.position.y)
                                                    + Direction.getDirectionVector(aimDir) * idealHookDistance,
                                                    hook.transform.position);
        return distRating;
    }

    //Returns the best hook (Collider2D) for a particular aiming direction
    private Collider2D getBestHook( EDirection aimDir )
    {
        Collider2D best = null;
        float bestRating = 0;
        foreach(Collider2D hook in getHookables())
        {
            if (!hook)
                continue;

            float rating = rateHook(hook, aimDir);
            if(rating > bestRating)
            {
                best = hook;
                bestRating = rating;
            }
        }
        return best;
    }


    public void fire( EDirection dir )
    {

        Vector3 position = gameObject.transform.position;

        Collider2D hit = getBestHook( dir );
        if (hit) {
            swinging = true;
            hookedGO = hit.gameObject;
            ropeConstraint.transform.parent = hookedGO.transform;

            ropeConstraint.SetActive(true);
            lineRenderer.enabled = true;

            dist = Mathf.Sqrt(Mathf.Pow(hookedGO.transform.position.x - position.x, 2) + Mathf.Pow(hookedGO.transform.position.y - position.y, 2));
            dist = 1 + dist * 2f;
            ropeConstraint.transform.position = hookedGO.transform.position;
            ropeConstraint.transform.localScale = new Vector3(dist / hookedGO.transform.localScale.x, dist / hookedGO.transform.localScale.y, 0); //adjust for hooked object's scale

            StartCoroutine("updateRope");
        }
    }

    public void release()
    {
        if (hookedGO)
        {
            swinging = false;
            resetSwing();
            ropeConstraint.SetActive(false);
            lineRenderer.enabled = false;
            hookedGO = null;
            StopCoroutine("updateRope");
        }
    }

    private void swing(EDirection dir) { force.force = Direction.getDirectionVector(dir) * 10; }
    public void swingRight() { swing(EDirection.RIGHT); }
    public void swingLeft() { swing(EDirection.LEFT); }
    public void resetSwing() { force.force = Vector2.zero; }

    public void contract()
    {
        rb.position += new Vector2(0, pullRate);
        float newScale = Mathf.Clamp(ropeConstraint.transform.localScale.x - expandContractRate, minRopeLength, maxRopeLength);
        ropeConstraint.transform.localScale = new Vector3(newScale, newScale, 0);
    }

    public void loosen()
    {
        rb.position -= new Vector2(0, pullRate);
        float newScale = Mathf.Clamp(ropeConstraint.transform.localScale.x + expandContractRate, minRopeLength, maxRopeLength);
        ropeConstraint.transform.localScale = new Vector3(newScale, newScale, 0);
    }

    private IEnumerator updateRope()
    {
        while(true)
        {
            lineRenderer.SetPosition(0, gameObject.transform.position);
            lineRenderer.SetPosition(1, hookedGO.transform.position);
            yield return null;
        }
    }
}
