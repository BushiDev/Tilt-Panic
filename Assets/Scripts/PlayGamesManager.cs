using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class PlayeGamesManager : MonoBehaviour{

    public static PlayeGamesManager instance;

    public bool playerSignedIn;

    void Start(){

        if(instance != null && instance != this){

            Destroy(gameObject);

        }else{

            instance = this;

        }

       // PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
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

            }else{

                playerSignedIn = false;
                UIController.instance.HideAll();
                UIController.instance.ShowSelected(3);

            }

        });

    }

    public void LeaderboardUpdate(string id, int value){

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

}