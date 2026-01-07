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

        Debug.Log("PLAYER DATA IS: " + PlayerPrefs.GetString("PlayerData"));

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

        Debug.Log("Trying to login...");
//PlayGamesPlatform.Instance
        Social.localUser.Authenticate((success) => {

            Debug.Log("Login status: " + success.ToString());
// == SignInStatus.Success
            if(success){

                Debug.Log("Login sucess");

                playerSignedIn = true;
                UIController.instance.HideAll();
                UIController.instance.ShowSelected(0);
                LoadGameData();

            }else{

                Debug.Log("Login failed");

                playerSignedIn = false;
                UIController.instance.HideAll();
                UIController.instance.ShowSelected(3);

                if(!PlayerPrefs.HasKey("PlayerData") ||  string.IsNullOrEmpty(PlayerPrefs.GetString("PlayerData"))){

                    playerData = new PlayerData();
                    PlayerPrefs.SetString("PlayerData", JsonUtility.ToJson(playerData));
                    PlayerPrefs.Save();

                }else{

                    playerData = JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString("PlayerData"));

                    if(playerData.version != (new PlayerData()).version){

                        playerData = RestoreVersion(playerData);

                    }

                }

                Shop.instance.LoadCustomsData();

            }

        });

    }

    PlayerData RestoreVersion(PlayerData playerData){

        PlayerData t = new PlayerData();

        t.bestScore = playerData.bestScore;
        t.totalScore = playerData.totalScore;

        t.playerColorsUnlocked = RestoreBoolArray(playerData.playerColorsUnlocked, 9);
        t.playerSkinsUnlocked  = RestoreBoolArray(playerData.playerSkinsUnlocked, 6);
        t.shieldColorsUnlocked = RestoreBoolArray(playerData.shieldColorsUnlocked, 9);
        t.shieldSkinsUnlocked  = RestoreBoolArray(playerData.shieldSkinsUnlocked, 3);

        t.playerSkinsWatchedAds = playerData.playerSkinsWatchedAds;
        t.playerColorsWatchedAds = playerData.playerColorsWatchedAds;
        t.shieldSkinsWatchedAds = playerData.shieldSkinsWatchedAds;
        t.shieldColorsWatchedAds = playerData.shieldColorsWatchedAds;

        t.playerColor = playerData.playerColor;
        t.playerSkin = playerData.playerSkin;
        t.shieldColor = playerData.shieldColor;
        t.shieldSkin = playerData.shieldSkin;

        return t;

    }

    public void LeaderboardUpdate(string id, int value){

        if(!playerSignedIn) return;

        Social.ReportScore(value, id, (bool success) => {

            Debug.Log(id + " : " + value);

        });

    }

    public void ShowLeaderboards(){

        if(!playerSignedIn){

            Login();
            return;

        }

        Social.ShowLeaderboardUI();

    }

    public void ShowAchievements(){

        if(!playerSignedIn){

            Login();
            return;

        }

        Social.ShowAchievementsUI();

    }

    public void CollectAchievement(string id){

        if(!playerSignedIn) return;

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
                    
                }else{

                    if(readStatus != SavedGameRequestStatus.Success) return;

                    string loaded = Encoding.UTF8.GetString(data);
                    playerData = JsonUtility.FromJson<PlayerData>(loaded);
                    if(playerData.version != (new PlayerData()).version){

                        playerData = RestoreVersion(playerData);

                    }
                    PlayerPrefs.SetString("PlayerData", JsonUtility.ToJson(playerData));
                    PlayerPrefs.Save();

                    Shop.instance.LoadCustomsData();

                }

            });

        });

    }

    bool[] RestoreBoolArray(bool[] oldArr, int newSize){

        bool[] arr = new bool[newSize];
        if(oldArr != null){

            for(int i = 0; i < Mathf.Min(oldArr.Length, newSize); i++){

                arr[i] = oldArr[i];

            }

        }

        return arr;
    }

}

[System.Serializable]
public class PlayerData{

    public int version;

    public int totalScore;

    public int bestScore;

    public bool[] playerColorsUnlocked;
    public bool[] playerSkinsUnlocked;
    public bool[] shieldColorsUnlocked;
    public bool[] shieldSkinsUnlocked;

    public int[] playerColorsWatchedAds;
    public int[] playerSkinsWatchedAds;
    public int[] shieldColorsWatchedAds;
    public int[] shieldSkinsWatchedAds;

    public int playerSkin = 0;
    public int playerColor = 0;
    public int shieldSkin = 0;
    public int shieldColor = 0;

    public PlayerData(){

        version = 1;

        totalScore = 0;

        bestScore = 0;

        playerColorsUnlocked = new bool[9];
        playerColorsUnlocked[0] = true;

        playerSkinsUnlocked = new bool[6];
        playerSkinsUnlocked[0] = true;

        shieldColorsUnlocked = new bool[9];
        shieldColorsUnlocked[0] = true;
        
        shieldSkinsUnlocked = new bool[3];
        shieldSkinsUnlocked[0] = true;

        playerColorsWatchedAds = new int[9];
        playerSkinsWatchedAds = new int[6];
        shieldColorsWatchedAds = new int[9];
        shieldSkinsWatchedAds = new int[3];

        playerSkin = 0;
        playerColor = 0;
        shieldSkin = 0;
        shieldColor = 0;

    }

}