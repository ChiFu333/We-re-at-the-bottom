using DG.Tweening;
using UnityEngine;

public class PlatformerPlayer : MonoBehaviour
{
    [SerializeField] private Sprite goodSprite, badSprite;
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheckPoint;

    [Header("Head Rotation")]
    private SpriteRenderer head;

    private Rigidbody2D rb;
    private float horizontalInput;
    private bool isGrounded;
    private bool isFacingRight = true;
    
    private Vector2 startPos;
    
    private void Awake()
    {
        startPos = transform.position;
        head = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Input
        
        
        // Jump
        if (Input.GetKeyDown(KeyCode.J) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            transform.DOPunchScale(Vector3.right * 0.3f, 0.5f);
            G.AudioManager.PlayWithRandomPitch(R.Audio.jumpSound, 0.2f);
        }

        // Flip character
        if (horizontalInput > 0f && !isFacingRight)
        {
            Flip(true);
        }
        else if (horizontalInput < 0f && isFacingRight)
        {
            Flip(false);
        }

        if (transform.position.y < -7)
        {
            G.AudioManager.PlaySound(R.Audio.fallOut);
            transform.position = startPos;
        }
    }

    private void FixedUpdate()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);
        
        // Movement
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    private void Flip(bool b)
    {
        isFacingRight = b;
        head.flipX = !b;
    }

    public void ChangeToGood(bool b)
    {
        head.sprite = b ? goodSprite : badSprite;
    }
}
