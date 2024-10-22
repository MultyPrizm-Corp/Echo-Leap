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
    [SerializeField] private float _jumpPowertGravity;
    [SerializeField] private Vector2 passedPosition;
    [SerializeField] private bool permissionDoubleJump;
    [SerializeField] private bool readinessJump;
    [SerializeField] private bool readinessDoubleJump;
    // animator controll 
    private PlayerAnimationController _iPlayerMove;
    
   

    public void Move(float axisVector)
    {
        _rb.velocity =new Vector2(axisVector * _speed ,0);
    }

    public void Jump()
    {
        UnityEngine.Debug.Log("I jumped");
    }

    private void SwitchGravity()
    {
        
    }
}
