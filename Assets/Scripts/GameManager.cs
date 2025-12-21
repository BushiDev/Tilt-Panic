using UnityEngine;

public class GameManager : MonoBehaviour{

    public static GameManager instance;

    void Awake(){

        if(instance != null && instance != this){

            Destroy(gameObject);

        }else{

            instance = this;

        }

    }

    public void ShowGameOver(int score){

        if(score > best){

            best = score;

        }



    }

}