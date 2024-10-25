using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthSliderUI : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private int maxHealth = 10;

    public void SetHealth(int health)
    {
        text.text = $"{health}/{maxHealth}";

        slider.value = health / maxHealth;
    }

    public void SetMaxHealth(int health)
    {
        maxHealth = health;
    }
}
