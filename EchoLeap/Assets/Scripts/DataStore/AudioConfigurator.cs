using UnityEngine;

public class AudioConfigurator : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private string volumeFieldName;
    void Start()
    {
        SetField();
        FloatConfigurator.floatConfigEvent += SetField;
    }

    public void SetField()
    {
        audioSource.volume = PlayerPrefs.GetFloat(volumeFieldName);
    }
}
