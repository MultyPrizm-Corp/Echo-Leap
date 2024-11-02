using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyAgent : MonoBehaviour
{
    [SerializeField] private IMoveController moveCotroller;
    [SerializeField] private Transform player;
    [SerializeField] private float eyeContactRadius; // Радіус зору
    [SerializeField] private bool eyeContact;
    [SerializeField] private float radiusMeleeAttack; // Радіус атаки

    [SerializeField] private bool meleeAttack;

    //
    private Vector2 myVecotrDir;// для зручнішого виклику дравРей
    [SerializeField] private LayerMask allowedLayers; //layers that NOT ignored

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform; // Зберігаємо посилання на гравця
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = null; // Скидаємо посилання, якщо гравець виходить
        }
    }

    private void Update()
    {
        CheckEyeContact();
        CheckMeleeAttack();
    }

    private void CheckEyeContact()
    {
        if (player != null)
        {
            Vector3 offSet = new Vector3(0, 0.4f, 0);
            Vector2 direction = (player.position + offSet) - transform.position;
            myVecotrDir = direction;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, eyeContactRadius, allowedLayers);
            //
            if (hit.collider != null)
            {
                //Debug.Log($"contact with: {hit.transform.tag}");
                eyeContact = hit.collider.CompareTag("Player");
            }
            else
            {
                eyeContact = false;
            }

#if DEBUG
            DebugCheckEyeContact(hit);
#endif
        }
        else
        {
            eyeContact = false;
        }
    }
#if DEBUG
    private void DebugCheckEyeContact(RaycastHit2D hit)
    {
        Color rayColor = hit.transform != null && hit.transform.CompareTag("Player") ? Color.red : Color.green;
        Debug.DrawRay(transform.position, (myVecotrDir).normalized * eyeContactRadius, rayColor);
    }
#endif

    private void CheckMeleeAttack()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            meleeAttack = distance < radiusMeleeAttack;
        }
        else
        {
            meleeAttack = false;
        }
    }
}