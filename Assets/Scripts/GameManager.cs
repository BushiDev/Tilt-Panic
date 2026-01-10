using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour{

    public static GameManager instance;
    public int best;
    public TMP_Text scoreText;
    public TMP_Text bestText;

    public Transform player;

    public GameObject newBestScore;

    void Awake(){

        if(instance != null && instance != this){

            Destroy(gameObject);

        }else{

            instance = this;
            DontDestroyOnLoad(gameObject);

        }

    }

    void Start(){

        SurvivalTimer.instance.isAlive = false;
        ObstacleSpawner.instance.isPaused = true;     

        if(PlayerPrefs.HasKey("PlayerData")){
            
            best = JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString("PlayerData")).bestScore;
            
        }else{

            best = 0;
            PlayerPrefs.SetString("PlayerData", JsonUtility.ToJson(new PlayerData()));
            PlayerPrefs.Save();

        }

    }

    public void StartGame(){

        SurvivalTimer.instance.isAlive = true;
        ObstacleSpawner.instance.isPaused = false;
        ObstacleSpawner.instance.Reset();
        SurvivalTimer.instance.score = 0;
        SurvivalTimer.instance.timer = 0f;
        AchievementsCollector.instance.useTimer = true;

    }

    public void ShowGameOver(int score){

        SurvivalTimer.instance.invert.SetActive(false);

        newBestScore.SetActive(score > best);

        PlayGamesManager.instance.playerData.totalScore += score;
        SurvivalTimer.instance.lastScoreInvertUpdate = 200;
        SurvivalTimer.instance.lastScoreVibrationUpdate = 100;

        if(score > best){

            best = score;

            if(PlayGamesManager.instance != null){

                PlayGamesManager.instance.playerData.bestScore = best;

                if(PlayGamesManager.instance.playerSignedIn) PlayGamesManager.instance.SaveGameData();

            }

            PlayerData p = JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString("PlayerData"));
            p.bestScore = best;

            PlayerPrefs.SetString("PlayerData", JsonUtility.ToJson(p));
            PlayerPrefs.Save();
            Debug.Log("New score!!!!");

        }

        if(UIController.instance != null){

            UIController.instance.ShowSelected(2);

        }

        scoreText.text = score.ToString();
        bestText.text = best.ToString();
            
        if(Shield.instance != null) Shield.instance.isActive = false;

        if(AchievementsCollector.instance != null){

            AchievementsCollector.instance.useTimer = false;
            AchievementsCollector.instance.OnDeath();

        }

    }

    public void GameOver(){

        if(PlayGamesManager.instance.playerSignedIn) PlayGamesManager.instance.LeaderboardUpdate(GPGSIds.leaderboard_top, SurvivalTimer.instance.score);

        ShowGameOver(SurvivalTimer.instance.score);
        ResetPlayerPosition();
        

        if(Random.Range(0, 100) > 60){

            AdMob.instance.ShowFullscreenAd();

        }

    }

    void ResetPlayerPosition(){

        player.position = new Vector3(0f, -2f, 0f);

    }

    public void RemoveAllObstacles(){

        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach(GameObject go in obstacles){

            Destroy(go);

        }

        GameObject[] shields = GameObject.FindGameObjectsWithTag("Shield");
        foreach(GameObject go in shields){

            Destroy(go);

        }

    }

}