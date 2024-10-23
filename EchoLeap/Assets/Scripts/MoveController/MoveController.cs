using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour, IMoveController
{
    [SerializeField] private Rigidbody2D _rb;

    [SerializeField] private float _speed;

    // ще не юзав те що знизу в імплентації
    [SerializeField] private float _jumpPower;
    [SerializeField] private float _jumpDefaultGravity;
    [SerializeField] private float _jumpPowerGravity;
    [SerializeField] private Vector2 passedPosition;
    [SerializeField] private bool permissionDoubleJump;
    [SerializeField] private bool readinessJump = true;

    [SerializeField] private bool readinessDoubleJump;

    // animator controller 
    [SerializeField]private PlayerAnimationController _iPlayerMove;


    public void Move(float axisVector)
    {
        var vector2 = _rb.velocity;
        vector2.x = axisVector * _speed;
        _rb.velocity = vector2;
        // rotation
        // if (axisVector > 0)
        // {
        //     var rotation = transform.rotation;
        //     rotation.y = 180f;
        //     transform.rotation = rotation;
        // }
        //
        // if (axisVector < 0)
        // {
        //     var rotation = transform.rotation;
        //     rotation.y = 0f;
        //     transform.rotation = rotation;
        // }
        
        _iPlayerMove.Move(axisVector);
    }

    public void Jump()
    {
        if ( readinessDoubleJump && permissionDoubleJump)
        {
            // _iPlayerMove.Jump();
            //
            readinessDoubleJump = false;
            //_rb.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
            _rb.velocity = new Vector2(_rb.velocity.x, 3f);
            //_rb.velocity.y = 3f ;
            // мб переробити джампПовер і зробити все на велосіті, мб переробити на JumpCount++
        }


        if (readinessJump)
        {
            // _iPlayerMove.Jump();
            //
            readinessJump = false;
            _rb.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
            // оскільки в повітрі
            readinessDoubleJump = true;
            //UnityEngine.Debug.Log("I jumped");
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Ground"))
        {
            // _iPlayerMove.JumpAnimOff();
            readinessJump = true;
            readinessDoubleJump = false;
        }
    }

    private void SwitchGravity()
    {
        _rb.gravityScale = _rb.gravityScale * (-1f);
    }
}