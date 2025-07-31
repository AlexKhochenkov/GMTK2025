using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 9f;

    [SerializeField] private float jumpFallForce = 2.5f;
    private Rigidbody2D _rb;
    private Animator _animator;
    private bool isGrounded = true;
    private bool debug = false;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _rb.freezeRotation = true;
    }

    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");
        _rb.linearVelocity = new Vector2(moveInput * moveSpeed, _rb.linearVelocity.y);
        _animator.SetFloat("Run", Math.Abs(moveInput));
        _animator.SetBool("Jump", !isGrounded);
        if (moveInput != 0)
        {
            transform.localScale = new Vector3(Math.Sign(moveInput) * Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
            isGrounded = false;
        }
        if (_rb.linearVelocity.y < 0)
        {
            _rb.linearVelocity += Vector2.up * Physics2D.gravity.y * jumpFallForce * Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}