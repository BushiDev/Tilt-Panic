using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour{

    public static AudioManager instance;
    public AudioSource music;
    public AudioSource[] effects;

    void Awake(){

        instance = this;

    }

    void Start(){

        UpdateSettings();

    }

    public void UpdateSettings(){

        music.volume = Settings.instance.settingsData.music ? 0.5f : 0f;
        foreach(AudioSource aS in effects){

            aS.volume = Settings.instance.settingsData.effects ? 0.7f : 0f;

        }

    }

}