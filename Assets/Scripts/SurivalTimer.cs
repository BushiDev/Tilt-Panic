using UnityEngine;
using TMPro;

public class SurvivalTimer : MonoBehaviour{

    public static SurvivalTimer instance;

    public TMP_Text scoreText;
    public TMP_Text shieldPointsText;
    public int score;
    public bool isAlive;
    public float timer;

    public float t;

    public float scoreMultiplier;

    public GameObject invert;
    public GameObject glitch;

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

            if(score > lastScoreInvertUpdate - 2){

                glitch.SetActive(true);

            }else if(score > lastScoreInvertUpdate - 198){


                glitch.SetActive(false);

            }

            //  

            if(score > lastScoreInvertUpdate){

                lastScoreInvertUpdate += 200;

                invert.SetActive(!invert.activeSelf);

            }

        }

    }

    public void AddShieldPoints(){

        timer += 5f;
        shieldPointsText.gameObject.SetActive(true);
        Color c = Shop.instance != null ? Shop.instance.customs.shieldColors[PlayGamesManager.instance.playerData.shieldColor].color : new Color(1f, 1f, 1f, 0.2f);
        c.a = 1f;
        shieldPointsText.color = c;
        Invoke("HideShieldPoints", 1f);

    }

    void HideShieldPoints(){

        shieldPointsText.gameObject.SetActive(false);

    }


}