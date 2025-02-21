using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] public bool canMove = true;
    [Tooltip(("If your character does not jump, ignore all below 'Jumping' Character"))]
    [SerializeField] private bool doesCharacterJump = false;

    [Header("Base / Root")]
    [SerializeField] private Rigidbody2D baseRB;
    [SerializeField] private float hSpeed = 10f;
    [SerializeField] private float vSpeed = 6f;
    [Range(0, 1.0f)]
    [SerializeField] float movementSmooth = 0.5f;

    [Header("'Jumping' Character")]
    [SerializeField] private Rigidbody2D charRB;
    [SerializeField] private float jumpVal = 10f;
    [SerializeField] private int possibleJumps = 1;
    [SerializeField] private int currentJumps = 0;
    [SerializeField] private bool onBase = false;
    [SerializeField] private Transform jumpDetector;
    [SerializeField] private float detectionDistance;
    [SerializeField] private LayerMask detectLayer;
    [SerializeField] private float jumpingGravityScale;
    [SerializeField] private float fallingGravityScale;
    private bool jump = false;

    private bool facingRight = true;
    private Vector3 velocity = Vector3.zero;

    PlayerInput input;
    Controls controls = new Controls();
    public CombatTester combatTester;

    // 
    private Vector3 charDefaultRelPos, baseDefPos;

    // Start is called before the first frame update
    private void Awake()
    {
        input = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        charDefaultRelPos = charRB.transform.localPosition;
    }

    private void Update()
    {
        controls = input.GetInput();
        if (controls.JumpState && currentJumps < possibleJumps & combatTester.isPunching == false)
        {
            PlayerStates.StateInstance.GetSetPlayerState = PlayerStates.StatesOfPlayer.Jumping;
            Debug.Log("Jumping");
            jump = true;
        }
    }

    private void FixedUpdate()
    {
        if (combatTester.isPunching == false)
        {
            Move();
        }
        else
        {
            charRB.velocity = Vector2.zero;
            baseRB.velocity = Vector2.zero;
        }
    }

    private void Move()
    {
        if (!onBase && doesCharacterJump && charRB.velocity.y < 0)
        {
            detectBase();
        }

        if (canMove)
        {
            Vector3 targetVelocity = new Vector2(controls.HorizontalMove * hSpeed, controls.VerticalMove * vSpeed);

            Vector2 _velocity = Vector3.SmoothDamp(baseRB.velocity, targetVelocity, ref velocity, movementSmooth);
            baseRB.velocity = _velocity;

            //----- 
            if (doesCharacterJump)
            {
                if (onBase)
                {

                    // charRB.velocity = baseRB.velocity;
                    charRB.velocity = Vector2.zero;

                    // vertical check
                    if (charRB.transform.localPosition != charDefaultRelPos)
                    {
                        var charTransform = charRB.transform;
                        charTransform.localPosition = new Vector2(charTransform.localPosition.x,
                            charDefaultRelPos.y);
                    }
                }
                else
                {
                    // falling
                    // if (charRB.velocity.y < 0)
                    // {
                    //     // charRB.gravityScale = fallingGravityScale;
                    // }
                    // else
                    // { // moving upward from jump
                    //     // charRB.gravityScale = jumpingGravityScale;
                    // }

                    charRB.velocity = new Vector2(_velocity.x, charRB.velocity.y);
                }

                if (jump)
                {
                    charRB.isKinematic = false;
                    charRB.AddForce(Vector2.up * jumpVal, ForceMode2D.Impulse);
                    charRB.gravityScale = jumpingGravityScale;
                    jump = false;
                    currentJumps++;
                    onBase = false;
                }

                // --- horizontal position check
                if (charRB.transform.localPosition != charDefaultRelPos)
                {
                    //print("pos diff- local: " + charRB.transform.localPosition + "  --default: " + charDefaultRelPos);
                    var charTransform = charRB.transform;
                    charTransform.localPosition = new Vector2(charDefaultRelPos.x,
                        charTransform.localPosition.y);
                }
            }
            // --- 

            // rotate if we're facing the wrong way
            if (controls.HorizontalMove > 0 && !facingRight)
            {
                flip();
            }
            else if (controls.HorizontalMove < 0 && facingRight)
            {
                flip();
            }
        }
    }

    private void flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    private void detectBase()
    {
        RaycastHit2D hit = Physics2D.Raycast(jumpDetector.position, -Vector2.up, detectionDistance, detectLayer);
        if (hit.collider != null)
        {
            onBase = true;
            charRB.isKinematic = true;
            currentJumps = 0;
            // charRB.velocity = Vector2.zero;
            // baseRB.velocity = Vector2.zero;

            Debug.Log("setting velocity to zero");
            PlayerStates.StateInstance.GetSetPlayerState = PlayerStates.StatesOfPlayer.Moving;
        }
    }

    private void OnDrawGizmos()
    {
        if (doesCharacterJump)
        {
            Gizmos.DrawRay(jumpDetector.transform.position, -Vector3.up * detectionDistance);
        }
    }
}


/*using System.Collections;
using System.Collections.Generic;
//using System.Collections.Specialized;
//using System.ComponentModel;
//using System.Security.Cryptography;
//using System.Threading;
using UnityEngine;


public class CharacterMovement : MonoBehaviour
{
    
    [SerializeField] private float hSpeed = 10f;
    [SerializeField] private float vSpeed = 6f;
    private Rigidbody2D rb2D;
    [SerializeField] private bool canMove = true;

    private bool facingRight = true;

    [Range(0, 1.0f)]
    [SerializeField] float movementSmooth = 0.5f;
    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    public void Move(float hMove, float vMove, bool jump)
    {
        if (canMove)
        {
            Vector3 targetVelocity = new Vector2(hMove * hSpeed, vMove * vSpeed);
            rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, targetVelocity, ref velocity, movementSmooth);

            // rotate if facing the wrong way
            if (hMove > 0 && !facingRight)
            {
                flip();
            }
            else if (hMove < 0 && facingRight)
            {
                flip();
            }
        }
    }

    private void flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }
}*/

/*public class PlayerMove : MonoBehaviour
{
    public int runSpeed = 1;

    float horizontal;
    float vertical;
    bool facingRight;

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(horizontal * runSpeed, vertical * runSpeed, 0.0f);
        transform.position = transform.position + movement * Time.deltaTime;
        Flip(horizontal);
    }

    private void Flip(float horizontal)
    {
        if (horizontal < 0 && !facingRight || horizontal > 0 && facingRight)
        {
            facingRight = !facingRight;

            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
}*/
