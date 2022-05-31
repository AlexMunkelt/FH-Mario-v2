using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class Player : MonoBehaviour
{
    public GameObject[] runAnimations;
    public GameObject[] jumpAnimations;
    public GameObject[] idleAnimations;

    public Playertype playertype;
    public float speed = 1f;
    public float acceleration = 1f;
    public float jumpStrength = 1f;
    public float jumpOnEnemyMult = 2f;
    public LayerMask layerMask;

    public bool canMove = true;
    public bool canJump = true;

    public enum Playertype { Player1, Player2 };
    private enum State { Idle, Running, Jumping};

    private State state = State.Idle;
    private Rigidbody rb;
    private bool isGrounded = false;
    private bool isJumping = false;
    private bool touchedLeftWall = false;
    private bool touchedRightWall = false;

    private int runIndex = 0;

    private Vector3 move = Vector3.zero;
    private Vector3 startPos;
    private GameController controller;

    void Start()
    {
        if (!ToggleCoop.instance.coop && playertype == Playertype.Player2)
        {
            this.gameObject.SetActive(false);
        }

        rb = this.GetComponent<Rigidbody>();
        startPos = this.transform.position;
        controller = GameController.instance;

        StartCoroutine(Animation());

        HorizontalMovementTest();
    }

    void Update()
    {

    }

    private void FixedUpdate()
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
                this.transform.rotation = Quaternion.Euler(0, -90, 0);
            } else if (hor < 0)
            {
                this.transform.rotation = Quaternion.Euler(0, 90, 0);
            }

            move.x = hor * speed * Time.deltaTime;
        }

        if (move.x != 0 && isGrounded && !isJumping)
        {
            state = State.Running;
        }
        else if (isGrounded && !isJumping)
        {
            state = State.Idle;
        } else
        {
            state = State.Jumping;
        }

        rb.velocity = Vector3.Lerp(rb.velocity, move, 1f);

        #endregion

        #region Jumping

        JumpingBehaviour();

        #endregion

        #region GroundedCheck

        RaycastHit hit;

        if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), transform.TransformDirection(-transform.up), out hit, 1f, ~layerMask))
        {
            Debug.DrawRay(transform.position + new Vector3(0, 1, 0), transform.TransformDirection(-transform.up) * hit.distance, Color.red);
            isGrounded = true;
            isJumping = false;
        }
        else
        {
            isGrounded = false;
        }

        #endregion

        #region WalljumpingCheck

        if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), transform.TransformDirection(-transform.right), out hit, 1f, ~layerMask))
        {
            Debug.DrawRay(transform.position + new Vector3(0, 1, 0), transform.TransformDirection(-transform.right) * hit.distance, Color.red);
            touchedLeftWall = true;
        }
        else
        {
            touchedLeftWall = false;
        }

        if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), transform.TransformDirection(transform.right), out hit, 1f, ~layerMask))
        {
            Debug.DrawRay(transform.position + new Vector3(0, 1, 0), transform.TransformDirection(transform.right) * hit.distance, Color.red);
            touchedRightWall = true;
        }
        else
        {
            touchedRightWall = false;
        }

        #endregion
    }

    public void JumpingBehaviour()
    {
        if (canJump)
        {
            if (Input.GetKey(KeyCode.W) && isGrounded && playertype == Playertype.Player1)
            {
                if (!isJumping)
                {
                    Jump();
                }
                //move.y = jumpStrength;
            }

            if (Input.GetKey(KeyCode.UpArrow) && isGrounded && playertype == Playertype.Player2)
            {
                if (!isJumping)
                {
                    Jump();
                }
                //move.y = jumpStrength;
            }

            // Walljumping
            //if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A) && touchedLeftWall)
            //{
            //    move.y = jumpStrength;
            //    move.x = speed;

            //    StartCoroutine(CanMoveAgain(0.5f));
            //}

            //if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D) && touchedRightWall)
            //{
            //    move.y = jumpStrength;
            //    move.x = -speed;

            //    StartCoroutine(CanMoveAgain(0.5f));
            //}
        }
    }

    public void Jump(float mult = 1f)
    {
        rb.AddForce(new Vector2(0, 1 * jumpStrength * mult * Time.deltaTime), ForceMode.Impulse);

        isJumping = true;

        state = State.Jumping;
    }

    private IEnumerator Animation()
    {
        yield return new WaitForSeconds(0.1f);

        switch (state)
        {
            case State.Idle:
                ClearAnimations();
                idleAnimations[0].SetActive(true);
                break;
            case State.Running:
                yield return new WaitForSeconds(0.1f);

                ClearAnimations();

                runAnimations[runIndex].SetActive(true);

                if (runIndex == 0)
                {
                    runIndex = 1;
                }
                else
                {
                    runIndex = 0;
                }
                break;
            case State.Jumping:
                ClearAnimations();
                jumpAnimations[0].SetActive(true);
                break;
            default:
                break;
        }

        StartCoroutine(Animation());
    }

    private void ClearAnimations()
    {
        idleAnimations[0].SetActive(false);

        runAnimations[0].SetActive(false);
        runAnimations[1].SetActive(false);

        jumpAnimations[0].SetActive(false);
    }

    private IEnumerator CanMoveAgain(float timeUntilMoveAgain)
    {
        canMove = false;

        yield return new WaitForSeconds(timeUntilMoveAgain);

        canMove = true;
    }

    public void GetHit(int damage)
    {
        controller.health -= damage;
    }

    #region UnitTests

    public void HorizontalMovementTest()
    {
        try
        {
            Input.GetAxis("Player1");
            Input.GetAxis("Player2");
        }
        catch (System.Exception)
        {
            print("UNIT TEST HOR MOV: FALSE!");
            return;
        }

        if (speed > 0)
        {
            print("UNIT TEST HOR MOV: TRUE!");
        } else
        {
            print("UNIT TEST HOR MOV: FALSE!");
        }
    }

    #endregion
}
