using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour{

    public static GameManager instance;
    public int best;
    public TMP_Text scoreText;
    public TMP_Text bestText;

    public Transform player;

    public GameObject newBestScore;

    public bool devModeWasEnabled;

    PlayerData playerData;

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

            playerData = JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString("PlayerData"));
            
            best = playerData.bestScore;
            
        }else{

            if(PlayGamesManager.instance.playerSignedIn){

                playerData = PlayGamesManager.instance.playerData;
                best = playerData.bestScore;

            }else{

                best = 0;
                playerData = new PlayerData();

            }

        }

        PlayerPrefs.SetString("PlayerData", JsonUtility.ToJson(playerData));
        PlayerPrefs.Save();

        

    }

    public void StartGame(){

        if(PlayerPrefs.HasKey("FirstLaunch") && PlayerPrefs.GetInt("FirstLaunch") == 0){
            
            FirstLaunch();
            return;
            
        }

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

        if(!devModeWasEnabled) PlayGamesManager.instance.playerData.totalScore += score;
        //SurvivalTimer.instance.lastScoreInvertUpdate = 200;
        SurvivalTimer.instance.lastScoreVibrationUpdate = 100;
        SurvivalTimer.instance.score = 0;
        SurvivalTimer.instance.timer = 0f;
        SurvivalTimer.instance.scoreMultiplier = 0f;

        if(score > best){

            best = score;

            if(devModeWasEnabled) return;

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

            if(devModeWasEnabled) return;

            AchievementsCollector.instance.useTimer = false;
            AchievementsCollector.instance.OnDeath();

        }

        if(PlayerPrefs.HasKey("Review") && PlayerPrefs.GetInt("Review") == 0 && Time.time > 300f){

            UIController.instance.ShowSelected(9);

        }

    }

    public void GameOver(){

        if(PlayGamesManager.instance.playerSignedIn && !devModeWasEnabled) PlayGamesManager.instance.LeaderboardUpdate(GPGSIds.leaderboard_tilt_legends, SurvivalTimer.instance.score);

        ShowGameOver(SurvivalTimer.instance.score);
        SurvivalTimer.instance.glitch.SetActive(false);
        ResetPlayerPosition();

        if(devModeWasEnabled) return;

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

    void FirstLaunch(){

        UIController.instance.ShowSelected(8);
        Invoke("HideTooltip", 3f);

    }

    void HideTooltip(){

        PlayerPrefs.SetInt("FirstLaunch", 1);
        PlayerPrefs.Save();

        if(devModeWasEnabled) return;

        if(PlayGamesManager.instance != null){

            if(PlayGamesManager.instance.playerSignedIn) PlayGamesManager.instance.SaveGameData();

        }

        PlayerData p = JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString("PlayerData"));
        
        PlayerPrefs.SetString("PlayerData", JsonUtility.ToJson(p));
        PlayerPrefs.Save();

        UIController.instance.HideSelected(8);
        StartGame();

    }

}