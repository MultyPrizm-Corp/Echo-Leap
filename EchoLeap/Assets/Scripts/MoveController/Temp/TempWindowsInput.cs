using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempWindowsInput : MonoBehaviour
{
    private IMoveController moveInterface;
    private void Start()
    {
        // Знаходимо компонент MoveController
        moveInterface = GetComponent<IMoveController>();
    }
    // input A+D = 0f
    // private void Movement()
    // {
    //     var dir = Input.GetAxisRaw("Horizontal");
    //     moveInterface.Move(dir);
    // }
    // Можна у Інтерфейсі оголосити віртюал + оверайднути його в МовеКонтролер
    // public void Move2D(Vector2 vector2D)
    // {
    //     _rb.velocity = vector2D * _speed;
    // }

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
