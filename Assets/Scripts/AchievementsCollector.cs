using UnityEngine;

public class AchievementsCollector : MonoBehaviour{

    public static AchievementsCollector instance;

    public AchievementsCollected AC;

    float timer;

    public bool useTimer;

    public int shieldsCollected;

    const int VERSION = 2;

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

        if(GameManager.instance.devModeWasEnabled) return;

        if(!useTimer || !PlayGamesManager.instance.playerSignedIn || AC.allTimeCollected) return;

        timer += Time.deltaTime;

        CheckTimeAchievements();
        CheckShieldAchievements();

    }

    void CheckTimeAchievements(){

        if(GameManager.instance.devModeWasEnabled) return;

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

    void CheckShieldAchievements(){

        if(GameManager.instance.devModeWasEnabled) return;

        if(!AC.collected[(int)AchievementID.MultiShield]){

            if(Shield.instance.isActive && shieldsCollected >= 5){

                AC.collected[(int)AchievementID.MultiShield] = true;
                PlayGamesManager.instance.CollectAchievement(GPGSIds.achievement_multi_shield);

            }

        }

    }

    public void OnDeath(){

        if(GameManager.instance.devModeWasEnabled) return;

        shieldsCollected = 0;

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

        if(GameManager.instance.devModeWasEnabled) return;

        if(!pause) return;

        PlayerPrefs.SetString("AchievementsCollected", JsonUtility.ToJson(AC));
        PlayerPrefs.Save();

    }

}

[System.Serializable]
public class AchievementsCollected{

    public int version = 3;

    public bool[] collected;

    public bool allTimeCollected;

    public AchievementsCollected(){

        collected = new bool[5];
        allTimeCollected = false;

    }

}

public enum AchievementID{
    StillAlive = 0,
    RealSurvivor = 1,
    FirstBlood = 2,
    TooFast = 3,
    MultiShield = 4
}