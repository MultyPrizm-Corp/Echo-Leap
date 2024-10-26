using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboView : MonoBehaviour
{
    [Header("Genetal")]
    [SerializeField] private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

    [Header("Icons")]
    [SerializeField] private Sprite defaultIcon;
    [SerializeField] private Sprite basicAttack;
    [SerializeField] private Sprite heavyAttack;
    [SerializeField] private Sprite skillAttack;

    public void ViewCombo(List<string> combo)
    {
        int iconIndex = 0;
        foreach(string i in combo)
        {
            if (i == "basic")
            {
                SetViewIcon(iconIndex, basicAttack);
            }
            if (i == "heavy")
            {
                SetViewIcon(iconIndex, heavyAttack);
            }
            if (i == "skill")
            {
                SetViewIcon(iconIndex, skillAttack);
            }

            iconIndex += 1;
        }

        ClearIcons(iconIndex);

        if (transform.rotation.y != 0)
        {
            transform.rotation = new Quaternion(transform.rotation.x, 0, transform.rotation.z, transform.rotation.w);
        }
    }

    private void SetViewIcon(int index, Sprite icon)
    {
        if (index <= spriteRenderers.Count - 1)
        {
            spriteRenderers[index].sprite = icon;
        }
    }

    private void ClearIcons(int index)
    {
        while (index <= spriteRenderers.Count - 1)
        {
            spriteRenderers[index].sprite = defaultIcon;
            index += 1;
        }
    }
}
