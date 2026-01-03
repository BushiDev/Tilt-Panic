using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour{

    public static GameManager instance;
    public int best;
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

    }

    public void ShowGameOver(int score){

        if(score > best){

            best = score;
            PlayGamesManager.instance.playerData.bestScore = score;
            PlayGamesManager.instance.SaveGameData();

        }

        UIController.instance.HideAll();
        UIController.instance.ShowSelected(2);
        scoreText.text = score.ToString();
        bestText.text = best.ToString();

    }

    public void GameOver(){

        Debug.Log("Time.timeScale przed: " + Time.timeScale);


        SurvivalTimer.instance.isAlive = false;
        ObstacleSpawner.instance.isPaused = true;

        PlayGamesManager.instance.LeaderboardUpdate(GPGSIds.leaderboard_top, SurvivalTimer.instance.score);

        ShowGameOver(SurvivalTimer.instance.score);
        ResetPlayerPosition();
        RemoveAllObstacles();

        if(Random.Range(0, 100) > 75){

            AdMob.instance.ShowFullscreenAd();
            Random.seed = Random.Range(int.MinValue, int.MaxValue);

        }

    }

    void ResetPlayerPosition(){

        GameObject.Find("Player").transform.position = new Vector3(0f, -2f, 0f);

    }

    void RemoveAllObstacles(){

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