using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour, IMoveController
{
    private static readonly int RunState = Animator.StringToHash("RunState");
    [SerializeField] private Animator _animator;
    public void Move(float axisVector)
    {
        if (axisVector != 0)
        {
            _animator.SetFloat(RunState, 0.4f);
        }
        else
        {
            _animator.SetFloat(RunState, 0.0f);
        }
        if (axisVector > 0)
        {
            var rotation = transform.rotation;
            rotation.y = 180f;
            transform.rotation = rotation;
        }
        
        if (axisVector < 0)
        {
            var rotation = transform.rotation;
            rotation.y = 0f;
            transform.rotation = rotation;
        }
    }

    public void Jump()
    {
    }
}
