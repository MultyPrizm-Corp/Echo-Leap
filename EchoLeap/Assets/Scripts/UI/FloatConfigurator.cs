using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class FloatConfigurator : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private string fieldName;
    public static UnityAction floatConfigEvent;

    private void Start()
    {
        slider.value = PlayerPrefs.GetFloat(fieldName);
    }

    public void SetField()
    {
        PlayerPrefs.SetFloat(fieldName, slider.value);
        floatConfigEvent.Invoke();
    }
}
