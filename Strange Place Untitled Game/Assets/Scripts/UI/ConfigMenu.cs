using UnityEngine;
using UnityEngine.UI;

public class ConfigMenu : MonoBehaviour
{
    public Slider audioSlider;

    void Start()
    {
        audioSlider.value = ConfigManager.instance.volumeValue;
        audioSlider.onValueChanged.AddListener(delegate {ValueChange();});

    }

    public void ValueChange()
    {
        ConfigManager.instance.UpdateVolume(audioSlider.value);
    }
}
