using UnityEngine;

public class ConfigManager : MonoBehaviour
{
    public static ConfigManager instance;
    public float volumeValue = 1f;
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateVolume(float volume)
    {
        volumeValue = volume;
        AudioListener.volume = volume;
    }

}
