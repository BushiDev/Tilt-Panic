using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine.SocialPlatforms;
using System.Text;

public class PlayeGamesManager : MonoBehaviour{

    public static PlayeGamesManager instance;
    public PlayerData playerData;

    public bool playerSignedIn;

    const string SAVE_NAME = "TiltPanic_Save";

    void Start(){

        if(instance != null && instance != this){

            Destroy(gameObject);

        }else{

            instance = this;

        }

        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
        Login();

    }

    public void Login(){

        PlayGamesPlatform.Instance.Authenticate((success) => {

            if(success == SignInStatus.Success){

                playerSignedIn = true;
                UIController.instance.HideAll();
                UIController.instance.ShowSelected(0);

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

        PlayGamesPlatform.Instance.SavedGame.OpenWithAutomaticConfiltResolution(SAVE_NAME, DataSource.ReadCacheOrNetwork, ConfigResolutionStrategy.UseLongestPlaytime, (status, game) => {
    
            if(status != SavedGameRequestStatus.success) return;

            SavedGameMetaDataUpdate update = new SavedGameMetaDataUpdate.Builder().Build();
            PlayGamesPlatform.Instance.SavedGame.CommitUpdate(game, update, data, (commitStatus, commitGame) => {

                Debug.Log("Saved");

            });

        });

    }

    public void LoadGameData(){

        if(!playerSignedIn) return;

        PlayGamesPlatform.Instance.SavedGame.OpenWithAutomaticConfiltResolution(SAVE_NAME, DataSource.ReadCacheOrNetwork, ConfigResolutionStrategy.UseLongestPlaytime, (status, game) => {

            if(status != SavedGameRequestStatus.Success) return;

            PlayGamesPlatform.Instance.SavedGame.ReadBinaryData(game, (readStatus, data) => {

                if(readStatus != SavedGameRequestStatus.Success || data.length == 0) return;

                string loaded = Encoding.UTF8.GetString(data);
                playerData = JsonUtility.FromJson<PlayerData>(loaded);

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