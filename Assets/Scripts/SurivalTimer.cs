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

    public GameObject invert;

    void Awake(){

        if(instance != null && instance != this){

            Destroy(gameObject);

        }else{

            instance = this;
            DontDestroyOnLoad(gameObject);

        }

    }

    public int lastScoreInvertUpdate = 200;
    public int lastScoreVibrationUpdate = 100;

    void Update(){

        if(isAlive){

            timer += Time.deltaTime + scoreMultiplier;
            scoreMultiplier += Time.deltaTime * t;

            score = Mathf.RoundToInt(timer);
            scoreText.text = score.ToString();

            if(score > lastScoreVibrationUpdate){

                lastScoreVibrationUpdate += 100;

                if(Settings.instance.settingsData.vibrations) RDG.Vibration.Vibrate(150);

            }

            if(score > lastScoreInvertUpdate){

                lastScoreInvertUpdate += 200;

                invert.SetActive(!invert.activeSelf);

            }

        }

    }


}