using UnityEngine;

public class AchievementsCollector : MonoBehaviour{

    public static AchievementsCollector instance;

    float timer;

    public bool useTimer;

    void Awake(){

        instance = this;

    }

    void Update(){

        if(!useTimer) return;

        timer += Time.deltaTime;

        if(timer >= 30f && timer < 33f && PlayGamesManager.instance.playerSignedIn) PlayGamesManager.instance.CollectAchievement(GPGSIds.achievement_still_alive);
        
        if(timer >= 120f && timer < 123f && PlayGamesManager.instance.playerSignedIn) PlayGamesManager.instance.CollectAchievement(GPGSIds.achievement_real_survivor);

    }

    public void OnDeath(){

        if(!PlayGamesManager.instance.playerSignedIn) return;

        PlayGamesManager.instance.CollectAchievement(GPGSIds.achievement_first_blood);

        if(timer < 5f){

            PlayGamesManager.instance.CollectAchievement(GPGSIds.achievement_too_fast);

        }

    }

}