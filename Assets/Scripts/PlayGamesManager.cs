using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class PlayeGamesManager : MonoBehaviour{

    public static PlayeGamesManager instance;

    void Awake(){

        if(instance != null && instance != this){

            Destroy(gameObject);

        }else{

            instance = this;

        }

        //Config

    }

}