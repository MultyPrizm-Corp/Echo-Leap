using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowsInput : MonoBehaviour
{
    private IMoveController moveInterface;
    private PlayerAttackController playerAttackController;
    public bool isActive;

    private void Start()
    {
        // Знаходимо компонент MoveController
        moveInterface = GetComponent<IMoveController>();
        playerAttackController = GetComponent<PlayerAttackController>();
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

    private void AttackButtons()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && playerAttackController != null)
        {
            playerAttackController.BasicAttack();
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && playerAttackController != null)
        {
            playerAttackController.HeavyAttack();
        }
    }

    private void Update()
    {
        MovementHandy();
        Jumping();
        AttackButtons();
    }
}