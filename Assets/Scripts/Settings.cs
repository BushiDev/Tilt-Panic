using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour{

    public static Settings instance;
    public Toggle musicToggle;
    public Toggle effectsToggle;
    public Toggle vibrationsToggle;

    public SettingsData settingsData;

    void Awake(){

        instance = this;

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

    }

    public void ToggleMusic(bool value){

        settingsData.music = value;
        AudioManager.instance.UpdateSettings();

    }

    public void ToggleEffects(bool value){

        settingsData.effects = value;
        AudioManager.instance.UpdateSettings();

    }

    public void ToggleVibrations(bool value){

        settingsData.vibrations = value;

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

    public SettingsData(){

        this.music = true;
        this.effects = true;
        this.vibrations = true;

    }

}