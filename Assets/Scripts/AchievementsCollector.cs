using UnityEngine;

public class AchievementsCollector : MonoBehaviour{

    public static AchievementsCollector instance;

    public AchievementsCollected AC;

    float timer;

    public bool useTimer;

    const int VERSION = 1;

    void Awake(){

        if(instance != null && instance != this){

            Destroy(gameObject);

        }else{

            instance = this;

        }

    }

    void Start(){

        if(!PlayerPrefs.HasKey("AchievementsCollected")){

            AC = new AchievementsCollected();
            PlayerPrefs.SetString("AchievementsCollected", JsonUtility.ToJson(AC));
            PlayerPrefs.Save();

        }else{

            AC = JsonUtility.FromJson<AchievementsCollected>(PlayerPrefs.GetString("AchievementsCollected"));

        }

        if(AC.version != VERSION){

            bool[] arr = new bool[(new AchievementsCollected()).collected.Length];

            for(int i = 0; i < AC.collected.Length; i++){

                arr[i] = AC.collected[i];

            }

            AC.collected = arr;
            AC.version = VERSION;

        }

    }

    void Update(){

        if(!useTimer || !PlayGamesManager.instance.playerSignedIn || AC.allTimeCollected) return;

        timer += Time.deltaTime;

        CheckTimeAchievements();

    }

    void CheckTimeAchievements(){

        if(!AC.allTimeCollected) AC.allTimeCollected = (AC.collected[(int)AchievementID.StillAlive] && AC.collected[(int)AchievementID.RealSurvivor]);

        if(timer >= 30f && !AC.collected[(int)AchievementID.StillAlive]){

            PlayGamesManager.instance.CollectAchievement(GPGSIds.achievement_still_alive);

            AC.collected[(int)AchievementID.StillAlive] = true;
        
        }

        if(timer >= 120f && !AC.collected[(int)AchievementID.RealSurvivor]){

            PlayGamesManager.instance.CollectAchievement(GPGSIds.achievement_real_survivor);

            AC.collected[(int)AchievementID.RealSurvivor] = true;

        }

    }

    public void OnDeath(){

        if(!PlayGamesManager.instance.playerSignedIn) return;

        if(!AC.collected[(int)AchievementID.FirstBlood]){
            
            PlayGamesManager.instance.CollectAchievement(GPGSIds.achievement_first_blood);

            AC.collected[(int)AchievementID.FirstBlood] = true;
            
        }

        if(timer < 5f && !AC.collected[(int)AchievementID.TooFast]){

            PlayGamesManager.instance.CollectAchievement(GPGSIds.achievement_too_fast);

            AC.collected[(int)AchievementID.TooFast] = true;
        
        }

        timer = 0f;

    }

    void OnApplicationPause(bool pause){

        if(!pause) return;

        PlayerPrefs.SetString("AchievementsCollected", JsonUtility.ToJson(AC));
        PlayerPrefs.Save();

    }

}

[System.Serializable]
public class AchievementsCollected{

    public int version = 1;

    public bool[] collected;

    public bool allTimeCollected;

    public AchievementsCollected(){

        collected = new bool[4];
        allTimeCollected = false;

    }

}

public enum AchievementID{
    StillAlive = 0,
    RealSurvivor = 1,
    FirstBlood = 2,
    TooFast = 3
}