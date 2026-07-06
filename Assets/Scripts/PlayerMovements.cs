using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovements : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;

    [SerializeField] AudioClip dieSFX;
    [SerializeField] AudioSource MusicSource;
    public AudioClip backgroundMusic;

    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    float gravityScaleAtStart;

    BoxCollider2D myFeetCollider;

    bool isAlive = true;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        gravityScaleAtStart = myRigidbody.gravityScale;
        myFeetCollider = GetComponent<BoxCollider2D>();

        MusicSource.clip = backgroundMusic;
        Debug.Log(backgroundMusic);
        MusicSource.Play();
    }

    [Obsolete]
    void Update()
    {
        if (!isAlive) { return; }
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    void OnMove(InputValue value)
    {
        if (!isAlive) { return; }

        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }
        if (value.isPressed)
        {
            myRigidbody.linearVelocity += new Vector2(0f, jumpSpeed);
        }
    }

    void Run()
    {
        Vector2 playervelocity = new Vector2(moveInput.x * runSpeed, myRigidbody.linearVelocity.y);
        myRigidbody.linearVelocity = playervelocity;

        bool hasHorizontal = Mathf.Abs(myRigidbody.linearVelocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", hasHorizontal);

    }

    void FlipSprite()
    {
        bool hasHorizontal = Mathf.Abs(myRigidbody.linearVelocity.x) > Mathf.Epsilon;

        if (hasHorizontal)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.linearVelocity.x), 1f);
        }
    }

    void ClimbLadder()
    {
        if (!myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myRigidbody.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("isClimbing", false);
            // myAnimator.speed = 1; // reset animation

            return;
        }
        myRigidbody.gravityScale = 0f;

        Vector2 climbvelocity = new Vector2(myRigidbody.linearVelocity.x, moveInput.y * climbSpeed);
        myRigidbody.linearVelocity = climbvelocity;

        bool hasVerticalSpeed = Mathf.Abs(myRigidbody.linearVelocity.y) > Mathf.Epsilon;
        // Always stay in climbing state when on ladder
        myAnimator.SetBool("isClimbing", hasVerticalSpeed);

        // if (hasVerticalSpeed)
        // {
        //     myAnimator.speed = 1;// play animation
        // }
        // else
        // {
        //     myAnimator.speed = 0;// pause animation (no weird pose)
        // }
    }

    void OnAttack(InputValue value)
    {
        if (!isAlive) return;

        if (value.isPressed)
        {
            myAnimator.SetTrigger("isAttacking");

            Instantiate(bullet, gun.position, transform.rotation);
        }
    }

    [Obsolete]
    void Die()
    {
        if (!isAlive) return;

        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            myRigidbody.linearVelocity = new Vector2(0f, jumpSpeed);
            StartCoroutine(HandleDeath());

        }
    }

    [Obsolete]
    IEnumerator HandleDeath()
    {
        AudioSource.PlayClipAtPoint(dieSFX, Camera.main.transform.position);

        yield return new WaitForSeconds(1f);

        FindAnyObjectByType<GameSession>().processPlayerDeath();
    }

}
