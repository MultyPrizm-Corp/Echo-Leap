using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowsInput : MonoBehaviour
{
    private IMoveController moveInterface;
    public bool isActive;

    private void Start()
    {
        // Знаходимо компонент MoveController
        moveInterface = GetComponent<IMoveController>();
    }

    private void MovementHandy()
    {
        float horizontal = 0f;
        if (Input.GetKey(KeyCode.A))
        {
            horizontal = -1f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            horizontal = 1f;
        }

        moveInterface.Move(horizontal);
    }

    private void Jumping()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            moveInterface.Jump();
        }
    }

    private void Update()
    {
        MovementHandy();
        Jumping();
    }
}