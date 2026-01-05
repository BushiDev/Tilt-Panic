using UnityEngine;
using TMPro;

public class SurvivalTimer : MonoBehaviour{

    public static SurvivalTimer instance;

    public TMP_Text scoreText;
    public int score;
    public bool isAlive;
    public float timer;

    public float t;

    public float scoreMultiplier;

    void Awake(){

        if(instance != null && instance != this){

            Destroy(gameObject);

        }else{

            instance = this;

        }

    }

    void Update(){

        if(isAlive){

            timer += Time.deltaTime + scoreMultiplier;
            scoreMultiplier += Time.deltaTime * t;

            score = Mathf.RoundToInt(timer);
            scoreText.text = score.ToString();

            if(score % 100 == 0){

                if(Settings.instance.settingsData.vibrations) RDG.Vibration.Vibrate(150);

            }

        }

    }

}