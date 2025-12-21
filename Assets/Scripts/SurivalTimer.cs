using UnityEngine;
using TMPro;

public class SurvivalTimer : MonoBehaviour{

    public TMP_Text scoreText;
    int score;
    bool isAlive = true;
    float timer;

    void Update(){

        if(!isAlive) return;

        timer += Time.deltaTime;

        score = Mathf.RoundToInt(timer);
        scoreText.text = score.ToString();

    }

}