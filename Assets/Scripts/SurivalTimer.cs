using UnityEngine;
using TMPro;

public class SurvivalTimer : MonoBehaviour{

    public static SurvivalTimer instance;

    public TMP_Text scoreText;
    public int score;
    public bool isAlive;
    public float timer;

    void Awake(){

        if(instance != null && instance != this){

            Destroy(gameObject);

        }else{

            instance = this;

        }

    }

    void Update(){

        if(isAlive){

            timer += Time.deltaTime;

            score = Mathf.RoundToInt(timer);
            scoreText.text = score.ToString();

        }

    }

}