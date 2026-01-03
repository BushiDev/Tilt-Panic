using UnityEngine;

public class AudioManager : MonoBehaviour{

    public static AudioManager instance;
    public AudioSource music;
    public AudioSource effects;

    void Awake(){

        instance = this;

    }

    public void UpdateSettings(){

        music.volume = Settings.instance.settingsData.music ? 0.5f : 0f;
        effects.volume = Settings.instance.settingsData.effects ? 0.5f : 0f;

    }

}