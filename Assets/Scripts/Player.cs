using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject model;

    public Playertype playertype;
    public float speed = 1f;
    public float acceleration = 1f;
    public float jumpStrength = 1f;
    public LayerMask layerMask;

    public enum Playertype { Player1, Player2 };
    private enum State { Idle, Running, Jumping};

    private State state = State.Idle;
    private Rigidbody rb;
    private bool isGrounded = false;
    private bool touchedLeftWall = false;
    private bool touchedRightWall = false;
    private bool canMove = true;

    private Vector3 move = Vector3.zero;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        #region HorizontalMovement

        float hor;

        if (playertype == Playertype.Player1)
        {
            hor = Input.GetAxis("Player1");
        }
        else
        {
            hor = Input.GetAxis("Player2");
        }

        move = rb.velocity;

        if (canMove)
        {
            if (hor > 0)
            {
                model.transform.rotation = Quaternion.Euler(0, -90, 0);
            } else if (hor < 0)
            {
                model.transform.rotation = Quaternion.Euler(0, 90, 0);
            }

            move.x = hor * speed;
        }

        #endregion

        #region Jumping

        if (Input.GetKey(KeyCode.W) && isGrounded && playertype == Playertype.Player1)
        {
            move.y = jumpStrength;
        }

        if (Input.GetKey(KeyCode.UpArrow) && isGrounded && playertype == Playertype.Player2)
        {
            move.y = jumpStrength;
        }

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A) && touchedLeftWall && playertype == Playertype.Player1)
        {
            move.y = jumpStrength;
            move.x = speed;

            StartCoroutine(CanMoveAgain(0.5f));
        }

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D) && touchedRightWall && playertype == Playertype.Player1)
        {
            move.y = jumpStrength;
            move.x = -speed;

            StartCoroutine(CanMoveAgain(0.5f));
        }

        rb.velocity = Vector3.Lerp(rb.velocity, move, 1f);

        #endregion

        #region GroundedCheck

        RaycastHit hit;

        if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), transform.TransformDirection(Vector3.down), out hit, 1f, ~layerMask))
        {
            Debug.DrawRay(transform.position + new Vector3(0, 1, 0), transform.TransformDirection(Vector3.down) * hit.distance, Color.red);
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        #endregion

        #region WalljumpingCheck

        if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), transform.TransformDirection(Vector3.left), out hit, 1f, ~layerMask))
        {
            Debug.DrawRay(transform.position + new Vector3(0, 2, 0), transform.TransformDirection(Vector3.left) * hit.distance, Color.red);
            touchedLeftWall = true;
        }
        else
        {
            touchedLeftWall = false;
        }

        if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), transform.TransformDirection(Vector3.right), out hit, 1f, ~layerMask))
        {
            Debug.DrawRay(transform.position + new Vector3(0, 1, 0), transform.TransformDirection(Vector3.right) * hit.distance, Color.red);
            touchedRightWall = true;
        }
        else
        {
            touchedRightWall = false;
        }

        #endregion
    }

    private IEnumerator CanMoveAgain(float timeUntilMoveAgain)
    {
        canMove = false;

        yield return new WaitForSeconds(timeUntilMoveAgain);

        canMove = true;
    }
}
