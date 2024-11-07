using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MoveController : MonoBehaviour, IMoveController
{
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private float _speed;
    [SerializeField] private bool isActiveToMove = true;

    // 
    [SerializeField] private float _jumpPower;
    [SerializeField] private float jumpDefaultGravity;
    [SerializeField] private float jumpPowerGravity;
    [SerializeField] private Vector2 passedPosition;
    [SerializeField] private bool permissionDoubleJump;
    [SerializeField] private bool readinessJump = true;

    [SerializeField] private bool readinessDoubleJump;

    // animator controller 
    [SerializeField] private PlayerAnimationController PlayerAnimationMove;

    // raycast check 
    [SerializeField] private float castDistance;

    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * castDistance, Color.cyan);
    }

    public void Move(float axisVector)
    {
        if (isActiveToMove)
        {
            var vector2 = rb.velocity;
            vector2.x = axisVector * _speed;
            rb.velocity = vector2;

            PlayerAnimationMove.Move(axisVector);
        }
    }

    public void Jump()
    {
        if (isActiveToMove)
        {
            if (readinessDoubleJump && permissionDoubleJump)
            {
                readinessDoubleJump = false;
                rb.velocity = new Vector2(rb.velocity.x, _jumpPower);
            }


            if (readinessJump)
            {
                readinessJump = false;
                rb.velocity = new Vector2(rb.velocity.x, _jumpPower);
                readinessDoubleJump = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        CheckPlatform();
    }

    private void SwitchGravity()
    {
        float directionY = (transform.position.y - passedPosition.y); // Визначаємо напрямок руху по осі Y

        if (directionY > 0) // Рух вгору
        {
            rb.gravityScale = jumpDefaultGravity; // Встановлюємо гравітацію для підйому
        }
        else if (directionY < 0) // Рух вниз
        {
            rb.gravityScale = jumpPowerGravity; // Встановлюємо гравітацію для падіння
        }
    }

    private void CheckPlatform()
    {
        if (!readinessJump)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, castDistance, ~LayerMask.GetMask("Player"));
            if (hit.collider != null)
            {
                readinessJump = true;
                readinessDoubleJump = false;
            }
        }
    }

    public void Lock(bool canMove)
    {
        this.isActiveToMove = canMove;
    }


    private void Start()
    {
        passedPosition = transform.position;
    }

    void Update()
    {
        SwitchGravity();
        passedPosition = transform.position; // Оновлюємо минулу позицію
    }
}