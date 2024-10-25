using UnityEngine;
using TMPro;

public class HealthSlider : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private Transform slider;
    [SerializeField] private TextMeshPro text;
    [SerializeField] private int maxHealth = 10;

    public void SetHealth(int health)
    {
        text.text = $"{health}/{maxHealth}";

        slider.localScale = new Vector3((float)health / (float)maxHealth, 1, 1);
    }

    public void SetMaxHealth(int health)
    {
        maxHealth = health;
    }
}
