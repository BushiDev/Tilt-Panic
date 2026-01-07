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

    void Update() {
    if (Time.timeScale == 0f) {
        Debug.Break(); // zatrzymuje edytor dokładnie w tym momencie
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

        Debug.Log("ShowGameOver START");

        Debug.Log("UIController: " + (UIController.instance != null));
        Debug.Log("scoreText: " + (scoreText != null));
        Debug.Log("bestText: " + (bestText != null));
        Debug.Log("AchievementsCollector: " + (AchievementsCollector.instance != null));

        newBestScore.SetActive(score > best);

        PlayGamesManager.instance.playerData.totalScore += score;

        if(score > best){

            best = score;

            PlayGamesManager.instance.playerData.bestScore = best;
            PlayGamesManager.instance.playerData.totalScore += score;

            if(PlayGamesManager.instance != null && PlayGamesManager.instance.playerSignedIn){

                PlayGamesManager.instance.SaveGameData();

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
        

        if(Random.Range(0, 100) > 75){

            AdMob.instance.ShowFullscreenAd();
            Random.seed = Random.Range(int.MinValue, int.MaxValue);

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