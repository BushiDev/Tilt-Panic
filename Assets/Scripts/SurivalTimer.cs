using UnityEngine;
using TMPro;

public class SurvivalTimer : MonoBehaviour{

    public static SurvivalTimer instance;

    public TMP_Text scoreText;
    public TMP_Text shieldPointsText;
    public TMP_Text shieldComboText;
    public int score;
    public bool isAlive;
    public float timer;

    public float t;

    public float scoreMultiplier;

    public GameObject invert;
    public GameObject glitch;

    public int shieldCombo = 1;

    void Awake(){

        if(instance != null && instance != this){

            Destroy(gameObject);

        }else{

            instance = this;
            DontDestroyOnLoad(gameObject);

        }

    }

    //public int lastScoreInvertUpdate = 200;
    public int lastScoreVibrationUpdate = 100;

    void Update(){

        if(isAlive){

            timer += Time.deltaTime + scoreMultiplier;
            scoreMultiplier += Time.deltaTime * t;

            score = Mathf.RoundToInt(timer);
            scoreText.text = score.ToString();

            if(score > lastScoreVibrationUpdate - 3){

                glitch.SetActive(true);
                PlayerTilt.instance.saveByInvert = true;

            }else if(score > lastScoreVibrationUpdate - 198){


                glitch.SetActive(false);
                PlayerTilt.instance.saveByInvert = false;

            }

            if(score > lastScoreVibrationUpdate){

                lastScoreVibrationUpdate += 100;
                invert.SetActive(!invert.activeSelf);

                if(Settings.instance.settingsData.vibrations) RDG.Vibration.Vibrate(150);

            }

        }

    }

    public void AddShieldPoints(){

        timer += 5f * shieldCombo;
        shieldPointsText.gameObject.SetActive(true);
        if(shieldCombo > 1){
            
            shieldComboText.text = "x" + shieldCombo.ToString();
            shieldComboText.gameObject.SetActive(true);
        
        }
        Color c = Shop.instance != null ? Shop.instance.customs.shieldColors[PlayGamesManager.instance.playerData.shieldColor].color : new Color(1f, 1f, 1f, 0.2f);
        c.a = 1f;
        shieldPointsText.color = c;
        shieldComboText.color = c;
        shieldCombo++;
        Invoke("HideShieldPoints", 1f);

    }

    void HideShieldPoints(){

        shieldPointsText.gameObject.SetActive(false);
        shieldComboText.gameObject.SetActive(false);

    }


}