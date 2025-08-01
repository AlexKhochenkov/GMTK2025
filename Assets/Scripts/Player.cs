using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 9f;
    [SerializeField] private float jumpHoldForce = 5f;
    [SerializeField] private float maxJumpTime = 0.35f;
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private float jumpBufferTime = 0.1f;
    [SerializeField] private float fallMultiplier = 2.5f;

    [Header("Visual Feedback")]
    [SerializeField] private ParticleSystem jumpParticles;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D _rb;
    private Animator _animator;
    private AudioSource _audioSource;
    private bool _jumpSoundPlayed = false;
    private bool _isJumping;
    private float _jumpTimeCounter;
    private float _coyoteTimeCounter;
    private float _jumpBufferCounter;

    public event Action Died;

    public static PlayerController Player { get; private set; }

    void Start()
    {
        if (Player is null)
        {
            Player = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _rb.freezeRotation = true;
    }

    void Update()
    {
        HandleCoyoteTime();
        HandleJumpBuffer();
        HandleJump();
        HandleJumpSound();
        HandleGravity();
        UpdateAnimations();
    }

    private void FixedUpdate()
    {
        CheckGrounded();
    }

    private void HandleCoyoteTime()
    {
        if (IsGrounded())
        {
            _coyoteTimeCounter = coyoteTime;
        }
        else
        {
            _coyoteTimeCounter -= Time.deltaTime;
        }
    }

    private void HandleJumpBuffer()
    {
        if (Input.GetButtonDown("Jump"))
        {
            _jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            _jumpBufferCounter -= Time.deltaTime;
        }
    }

    private void HandleJump()
    {
        if (_jumpBufferCounter > 0 && _coyoteTimeCounter > 0)
        {
            _isJumping = true;
            _jumpTimeCounter = maxJumpTime;
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
            _jumpBufferCounter = 0;
            if (!_jumpSoundPlayed)
            {
                _audioSource.Play();
                _jumpSoundPlayed = true;
            }
            if (jumpParticles) jumpParticles.Play();
        }

        if (Input.GetButton("Jump") && _isJumping)
        {
            if (_jumpTimeCounter > 0)
            {
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpHoldForce);
                _jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                _isJumping = false;
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            _isJumping = false;
        }
    }

    private void HandleGravity()
    {
        if (_rb.linearVelocity.y < 0)
        {
            _rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }

    private void UpdateAnimations()
    {
        _animator.SetBool("Grounded", IsGrounded());
        _animator.SetFloat("VerticalVelocity", _rb.linearVelocity.y);
    }

    private void CheckGrounded()
    {
        _animator.SetBool("Grounded", IsGrounded());
    }

    private void HandleJumpSound()
    {
        if (IsGrounded() && _jumpSoundPlayed)
        {
            _jumpSoundPlayed = false;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    // Visualize ground check radius
    private void OnDrawGizmosSelected()
    {
        if (groundCheck)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    public void Die()
    {
        Died?.Invoke();
        Debug.Log("Died");
    }
}