using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine.SocialPlatforms;
using System.Text;

public class PlayGamesManager : MonoBehaviour{

    public static PlayGamesManager instance;
    public PlayerData playerData;

    public bool playerSignedIn;

    const string SAVE_NAME = "TiltPanic_Save";

    void Start(){

        if(instance != null && instance != this){

            Destroy(gameObject);

        }else{

            instance = this;

        }

        //PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();

        //PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
        Login();

    }

    public void Login(){

        PlayGamesPlatform.Instance.Authenticate((success) => {

            if(success == SignInStatus.Success){

                playerSignedIn = true;
                UIController.instance.HideAll();
                UIController.instance.ShowSelected(0);
                LoadGameData();

            }else{

                playerSignedIn = false;
                UIController.instance.HideAll();
                UIController.instance.ShowSelected(3);

            }

        });

    }

    public void LeaderboardUpdate(string id, int value){

        if(!playerSignedIn) return;

        Social.ReportScore(value, id, (bool success) => {

            Debug.Log(id + " : " + value);

        });

    }

    public void ShowLeaderboards(){

        Social.ShowLeaderboardUI();

    }

    public void ShowAchievements(){

        Social.ShowAchievementsUI();

    }

    public void CollectAchievement(string id){

        PlayGamesPlatform.Instance.ReportProgress(id, 100.0f, (bool success) => {

            Debug.Log("ACHIEVEMNT collected");

        });

    }

    public void SaveGameData(){

        if(!playerSignedIn) return;

        byte[] data = Encoding.UTF8.GetBytes(JsonUtility.ToJson(playerData));

        PlayGamesPlatform.Instance.SavedGame.OpenWithAutomaticConflictResolution(SAVE_NAME, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, (status, game) => {
    
            if(status != SavedGameRequestStatus.Success) return;

            SavedGameMetadataUpdate update = new SavedGameMetadataUpdate.Builder().Build();
            PlayGamesPlatform.Instance.SavedGame.CommitUpdate(game, update, data, (commitStatus, commitGame) => {

                Debug.Log("Saved");

            });

        });

    }

    public void LoadGameData(){

        if(!playerSignedIn) return;

        PlayGamesPlatform.Instance.SavedGame.OpenWithAutomaticConflictResolution(SAVE_NAME, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, (status, game) => {

            if(status != SavedGameRequestStatus.Success) return;

            PlayGamesPlatform.Instance.SavedGame.ReadBinaryData(game, (readStatus, data) => {

                if(data.Length == 0 || data == null){

                    playerData = new PlayerData();
                    PlayerPrefs.SetString("PlayerData", JsonUtility.ToJson(playerData));
                    PlayerPrefs.Save();
                    
                }

                if(readStatus != SavedGameRequestStatus.Success) return;

                string loaded = Encoding.UTF8.GetString(data);
                playerData = JsonUtility.FromJson<PlayerData>(loaded);
                PlayerPrefs.SetString("PlayerData", loaded);
                PlayerPrefs.Save();

            });

        });

    }

}

[System.Serializable]
public class PlayerData{

    public int bestScore;

    public PlayerData(){

        bestScore = 0;

    }

}