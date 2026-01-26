using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour{

    public static Settings instance;
    public Toggle musicToggle;
    public Toggle effectsToggle;
    public Toggle vibrationsToggle;

    public Slider sensitivitySlider;

    public SettingsData settingsData;

    void Awake(){

        if(instance != null && instance != this){

            Destroy(gameObject);

        }else{

            instance = this;
            DontDestroyOnLoad(gameObject);

        }

    }

    void Start(){

        LoadSettings();

    }

    public void LoadSettings(){

        if(PlayerPrefs.HasKey("Settings")){

            settingsData = JsonUtility.FromJson<SettingsData>(PlayerPrefs.GetString("Settings"));

        }else{

            settingsData = new SettingsData();
            PlayerPrefs.SetString("Settings", JsonUtility.ToJson(settingsData));
            SaveSettings();

        }

        musicToggle.isOn = settingsData.music;
        effectsToggle.isOn = settingsData.effects;
        vibrationsToggle.isOn = settingsData.vibrations;
        sensitivitySlider.value = settingsData.sensitivity;

        if(AudioManager.instance != null) AudioManager.instance.UpdateSettings();

    }

    public void ToggleMusic(bool value){

        settingsData.music = value;
        if(AudioManager.instance != null) AudioManager.instance.UpdateSettings();

    }

    public void ToggleEffects(bool value){

        settingsData.effects = value;
        if(AudioManager.instance != null) AudioManager.instance.UpdateSettings();

    }

    public void ToggleVibrations(bool value){

        settingsData.vibrations = value;

    }

    public void SetSensitivity(float value){

        settingsData.sensitivity = value;

    }

    public void SaveSettings(){

        PlayerPrefs.SetString("Settings", JsonUtility.ToJson(settingsData));
        PlayerPrefs.Save();

    }

}

[System.Serializable]
public class SettingsData{

    public bool music;
    public bool effects;
    public bool vibrations;
    public float sensitivity;

    public SettingsData(){

        this.music = true;
        this.effects = true;
        this.vibrations = true;
        this.sensitivity = 1f;

    }

}