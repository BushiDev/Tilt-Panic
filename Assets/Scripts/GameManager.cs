using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour{

    public static GameManager instance;
    int best;
    public TMP_Text scoreText;
    public TMP_Text bestText;

    void Awake(){

        if(instance != null && instance != this){

            Destroy(gameObject);

        }else{

            instance = this;

        }

    }

    void Start(){

        SurvivalTimer.instance.isAlive = false;
        ObstacleSpawner.instance.isPaused = true;
        //UIController.instance.HideAll();
       // UIController.instance.ShowSelected(0);

    }

    public void StartGame(){

        SurvivalTimer.instance.isAlive = true;
        ObstacleSpawner.instance.isPaused = false;
        SurvivalTimer.instance.score = 0;
        SurvivalTimer.instance.timer = 0f;

    }

    public void ShowGameOver(int score){

        if(score > best){

            best = score;

        }

        UIController.instance.HideAll();
        UIController.instance.ShowSelected(2);
        scoreText.text = score.ToString();
        bestText.text = best.ToString();

    }

    public void GameOver(){

        SurvivalTimer.instance.isAlive = false;
        ObstacleSpawner.instance.isPaused = true;

        PlayeGamesManager.instance.LeaderboardUpdate(GPGSIds.leaderboard_top, SurvivalTimer.instance.score);

        ShowGameOver(SurvivalTimer.instance.score);
        ResetPlayerPosition();
        RemoveAllObstacles();

    }

    void ResetPlayerPosition(){

        GameObject.Find("Player").transform.position = Vector3.zero;

    }

    void RemoveAllObstacles(){

        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach(GameObject go in obstacles){

            Destroy(go);

        }

    }

}