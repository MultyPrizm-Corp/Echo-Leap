using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float followSpeed = 5f;
    [SerializeField, Space(5)] private Vector3 offset;

    [Header("LimitationPosition")]
    [SerializeField] private int XRigthLimitationPosition = 0;
    [SerializeField, Space(2)] private int XLeftLimitationPosition = 0;
    [SerializeField] private int YUpLimitationPosition = 0;
    [SerializeField] private int YDownLimitationPosition = 0;

    private Vector3 velocity = Vector3.zero;

    void FixedUpdate()
    {
        Vector3 targetPosition = player.position + offset;

        if(targetPosition.x < XRigthLimitationPosition && XRigthLimitationPosition != 0)
        {
            targetPosition.x = XRigthLimitationPosition;
        }
        else if (targetPosition.x > XLeftLimitationPosition && XLeftLimitationPosition != 0)
        {
            targetPosition.x = XLeftLimitationPosition;
        }

        if (targetPosition.y > YUpLimitationPosition && YUpLimitationPosition != 0)
        {
            targetPosition.y = YUpLimitationPosition;
        }
        else if (targetPosition.y < YDownLimitationPosition && YDownLimitationPosition != 0)
        {
            targetPosition.y = YDownLimitationPosition;
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.fixedDeltaTime);
    }
}
