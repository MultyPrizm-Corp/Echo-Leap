using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MoveController : MonoBehaviour, IMoveController
{
    [SerializeField] private Rigidbody2D _rb;

    [SerializeField] private float _speed;

    // 
    [SerializeField] private float _jumpPower;
    [SerializeField] private float _jumpDefaultGravity;
    [SerializeField] private float _jumpPowerGravity;
    [SerializeField] private Vector2 passedPosition;
    [SerializeField] private bool permissionDoubleJump;
    [SerializeField] private bool readinessJump = true;

    [SerializeField] private bool readinessDoubleJump;

    // animator controller 
    [SerializeField] private PlayerAnimationController PlayerAnimationMove;

    // raycast check 
    [SerializeField] private float castDistance;
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private bool rayTouchingGround;

    public void Move(float axisVector)
    {
        var vector2 = _rb.velocity;
        vector2.x = axisVector * _speed;
        _rb.velocity = vector2;

        PlayerAnimationMove.Move(axisVector);
    }

    public void Jump()
    {
        if (readinessDoubleJump && permissionDoubleJump)
        {
            Debug.Log("2st Jump");
            readinessDoubleJump = false;
            //_rb.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpPower);
            // мб переробити джампПовер і зробити все на велосіті, мб переробити на JumpCount++
        }


        if (readinessJump)
        {
            Debug.Log("1st Jump");
            readinessJump = false;
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpPower);
            //_rb.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
            // оскільки в повітрі
            readinessDoubleJump = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Ground"))
        {
            // _iPlayerMove.JumpAnimOff();
            // readinessJump = true;
            // readinessDoubleJump = false;
            CheckPlatform();
        }
        
    }

    private void SwitchGravity()
    {
        _rb.gravityScale = _rb.gravityScale * (-1f);
    }

    private void CheckPlatform()
    {
        if (!readinessJump)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, castDistance);
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    Debug.Log("Platform in bottom Ground");
                    readinessJump = true;
                    readinessDoubleJump = false;
                    rayTouchingGround = true;
                }
            }
            else
            {
                Debug.Log("touching nothing");
                rayTouchingGround = false;
            }
        }
    }

    private void Update()
    {
        //CheckPlatform();
    }

    private void FixedUpdate()
    {
    }
}