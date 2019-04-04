using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullPatrol : MonoBehaviour
{
    public float speed; //speed of the boi
    public float distance; //distance of the ray detecting collision
    public string movedir; //initial direction of movement so far can either be "up" or "right"
    public string coldir; //direction of the ray to detect for collision

    private bool movingInitialDir = true;
    private bool movelef = true, movedown = false, moveright = false, moveup = false;

    public Transform groundDetection;
    public Vector2 snail = new Vector2(-1,0);

    Vector2 downlef;
    Vector2 uplef;
    Vector2 downrig;

    void Start()
    {
        downlef = new Vector2(-1,-1);
        uplef = new Vector2(-1,1);
        downrig = new Vector2(1,-1);
    }

    void FixedUpdate()
    {
        if (movedir.Equals("right"))
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);

            if (coldir.Equals("down"))
            {
                RaycastHit2D groundBoi = Physics2D.Raycast(groundDetection.position, Vector2.down, distance);
                if (groundBoi.collider == false)
                {
                    if (movingInitialDir == true)
                    {
                        transform.eulerAngles = new Vector3(0, -180, 0);
                        movingInitialDir = false;
                    }
                    else
                    {
                        transform.eulerAngles = new Vector3(0, 0, 0);
                        movingInitialDir = true;
                    }
                }
            }
            else if (coldir.Equals("up"))
            {
                RaycastHit2D groundBoi = Physics2D.Raycast(groundDetection.position, Vector2.up, distance);
                if (groundBoi.collider == false)
                {
                    if (movingInitialDir == true)
                    {
                        transform.eulerAngles = new Vector3(0, -180, 0);
                        movingInitialDir = false;
                    }
                    else
                    {
                        transform.eulerAngles = new Vector3(0, 0, 0);
                        movingInitialDir = true;
                    }
                }
            }
        }
        else if (movedir.Equals("up"))
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime);

            if (coldir.Equals("right"))
            {
                RaycastHit2D groundBoi = Physics2D.Raycast(groundDetection.position, Vector2.right, distance);
                if (groundBoi.collider == false)
                {
                    if (movingInitialDir == true)
                    {
                        transform.eulerAngles = new Vector3(-180, 0, 0);
                        movingInitialDir = false;
                    }
                    else
                    {
                        transform.eulerAngles = new Vector3(0, 0, 0);
                        movingInitialDir = true;
                    }
                }
            }
            else if (coldir.Equals("left"))
            {
                RaycastHit2D groundBoi = Physics2D.Raycast(groundDetection.position, Vector2.left, distance);
                if (groundBoi.collider == false)
                {
                    if (movingInitialDir == true)
                    {
                        transform.eulerAngles = new Vector3(-180, 0, 0);
                        movingInitialDir = false;
                    }
                    else
                    {
                        transform.eulerAngles = new Vector3(0, 0, 0);
                        movingInitialDir = true;
                    }
                }
            }
        }
        else if (movedir.Equals("rotate"))
        {
                RaycastHit2D groundBoi = Physics2D.Raycast(groundDetection.position, Vector2.down, distance);
                RaycastHit2D sideBoi = Physics2D.Raycast(groundDetection.position, Vector2.right, distance);
                RaycastHit2D upBoi = Physics2D.Raycast(groundDetection.position, Vector2.up, distance);
                RaycastHit2D leftBoi = Physics2D.Raycast(groundDetection.position, Vector2.left, distance);
                RaycastHit2D diagonalley = Physics2D.Raycast(groundDetection.position, Vector2.one, distance);
                RaycastHit2D downleft = Physics2D.Raycast(groundDetection.position, downlef, distance);
                RaycastHit2D upleft = Physics2D.Raycast(groundDetection.position, uplef, distance);
                RaycastHit2D downright = Physics2D.Raycast(groundDetection.position, downrig, distance);

                if(groundBoi.collider)
                {
                    transform.Translate(Vector2.left * speed * Time.deltaTime);
                }
                else if (!groundBoi.collider)
                {
                    if (sideBoi.collider || downright.collider)
                    {
                        transform.Translate(Vector2.down * speed * Time.deltaTime);
                    }
                    else if(!sideBoi.collider && diagonalley.collider || upBoi.collider)
                    {
                      transform.Translate(Vector2.right * speed * Time.deltaTime);
                    }
                    else if(!upBoi.collider && upleft.collider && !diagonalley.collider)
                    {
                        transform.Translate(Vector2.up * speed * Time.deltaTime);
                    }
                    else if(!upleft.collider && downleft.collider)
                    {
                      transform.Translate(Vector2.left * speed * Time.deltaTime);
                    }
                }

        }
        else if (movedir.Equals("experiment"))
        {
          LayerMask c = new LayerMask();
          c = 1 << LayerMask.NameToLayer("SnailLayer");
          RaycastHit2D groundBoi = Physics2D.Raycast(groundDetection.position, Vector2.down, distance, c);
          RaycastHit2D sideBoi = Physics2D.Raycast(groundDetection.position, Vector2.right, distance, c);
          RaycastHit2D upBoi = Physics2D.Raycast(groundDetection.position, Vector2.up, distance, c);
          RaycastHit2D leftBoi = Physics2D.Raycast(groundDetection.position, Vector2.left, distance, c);
          transform.Translate(snail * speed * Time.deltaTime);
          if(!groundBoi.collider && movingInitialDir)
          {
              snail = new Vector2(0, -1);
              for(int x = 0; x <= 2; x++)
              {
                transform.Translate(snail * speed * Time.deltaTime);
              }
              movingInitialDir = false;
              movedown = true;
          }
          else if(movedown && !sideBoi.collider)
          {
            snail = new Vector2(1, 0);
            for(int x = 0; x <= 2; x++)
            {
              transform.Translate(snail * speed * Time.deltaTime);
            }
            movedown = false;
            moveright = true;
          }
          else if(moveright && !upBoi.collider)
          {
            snail = new Vector2(0, 1);
            for(int x = 0; x <= 2; x++)
            {
              transform.Translate(snail * speed * Time.deltaTime);
            }
            moveright = false;
            moveup = true;
          }
          else if(moveup && !leftBoi.collider)
          {
            snail = new Vector2(-1, 0);
            for(int x = 0; x <= 2; x++)
            {
              transform.Translate(snail * speed * Time.deltaTime);
            }
            moveup = false;
            movingInitialDir = true;
          }
        }

    }
  }
