using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class StaminaSliderUI : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image fillArea;

    private int maxStamin = 100;
    private bool alarming = false;

    public void SetStamina(int stamin)
    {
        slider.value = (float)stamin / (float)maxStamin;
        text.text = $"{stamin}/{maxStamin}";
    }

    public void SetMaxStamina(int stamin)
    {
        maxStamin = stamin;
    }

    public void LowStaminaAlarm()
    {
        StartCoroutine(StartLowStaminaAlarm());
    }

    private IEnumerator StartLowStaminaAlarm()
    {
        if (!alarming)
        {
            alarming = true;
            while (fillArea.color.g > 0f)
            {
                fillArea.color = new Color(fillArea.color.r, fillArea.color.g - 0.05f, fillArea.color.b, fillArea.color.a);

                yield return new WaitForSeconds(0.03f);
            }

            while (fillArea.color.g < 0.3f)
            {
                fillArea.color = new Color(fillArea.color.r, fillArea.color.g + 0.05f, fillArea.color.b, fillArea.color.a);

                yield return new WaitForSeconds(0.03f);
            }
            alarming = false;
        }
    }
}
