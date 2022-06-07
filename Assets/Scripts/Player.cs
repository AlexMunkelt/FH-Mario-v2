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
    public float doubleJumpMult = 2.5f;
    public float jumpOnEnemyMult = 2f;
    public LayerMask layerMask;

    public bool canMove = true;
    public bool canJump = true;
    public bool canDoubleJump = true;

    public enum Playertype { Player1, Player2 };
    private enum State { Idle, Running, Jumping };

    private State state = State.Idle;
    private Rigidbody rb;
    private bool isGrounded = false;
    private bool isJumping = false;
    private bool isDoubleJumping = false;
    private bool touchedLeftWall = false;
    private bool touchedRightWall = false;

    private int runIndex = 0;

    private Vector3 move = Vector3.zero;
    private Vector3 startPos;
    private GameController controller;

    void Start()
    {
        if (ToggleCoop.instance)
        {
            if (!ToggleCoop.instance.coop && playertype == Playertype.Player2)
            {
                this.gameObject.SetActive(false);
            }
        }

        rb = this.GetComponent<Rigidbody>();
        startPos = this.transform.position;
        controller = GameController.instance;

        if (this.gameObject.activeSelf)
        {
            StartCoroutine(Animation());
        }

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
            canJump = true;
            isJumping = false;
            isDoubleJumping = false;
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
            if (Input.GetKey(KeyCode.W) && playertype == Playertype.Player1)
            {
                if (!isJumping && isGrounded)
                {
                    canJump = false;
                    Jump();
                }
                else if (canDoubleJump && !isDoubleJumping)
                {
                    canJump = false;
                    isDoubleJumping = true;

                    Jump(doubleJumpMult);
                }
            }

            if (Input.GetKey(KeyCode.UpArrow) && playertype == Playertype.Player2)
            {
                if (!isJumping && isGrounded)
                {
                    canJump = false;
                    Jump();
                }
                else if (canDoubleJump && !isDoubleJumping)
                {
                    canJump = false;
                    isDoubleJumping = true;

                    Jump(doubleJumpMult);
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.W) && playertype == Playertype.Player1)
        {
            canJump = true;
        }

        if (Input.GetKeyUp(KeyCode.UpArrow) && playertype == Playertype.Player2)
        {
            canJump = true;
        }
    }

    void ResetYVelocity()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.angularVelocity = new Vector3(rb.angularVelocity.x, 0f, rb.angularVelocity.z);
    }

    public void Jump(float mult = 1f)
    {
        ResetYVelocity();
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectible"))
        {
            NoteCollectible tmp = other.gameObject.GetComponent<NoteCollectible>();

            switch (tmp.note)
            {
                case NoteCollectible.Note.A:
                    controller.count_colectables++;
                    controller.collectables++;
                    Destroy(other.gameObject);
                    break;
                case NoteCollectible.Note.B:
                    controller.count_colectables += 2;
                    controller.collectables++;
                    Destroy(other.gameObject);
                    break;
                case NoteCollectible.Note.C:
                    controller.count_colectables += 3;
                    controller.collectables++;
                    Destroy(other.gameObject);
                    break;
                case NoteCollectible.Note.D:
                    controller.count_colectables += 4;
                    controller.collectables++;
                    Destroy(other.gameObject);
                    break;
                case NoteCollectible.Note.E:
                    controller.count_colectables += 5;
                    controller.collectables++;
                    Destroy(other.gameObject);
                    break;
                case NoteCollectible.Note.F:
                    controller.count_colectables += 6;
                    controller.collectables++;
                    Destroy(other.gameObject);
                    break;
                default:
                    break;
            }


        }
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
