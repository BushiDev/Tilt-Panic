using UnityEngine;

public class Shop : MonoBehaviour{

    public static Shop instance;

    public SpriteRenderer player;
    public SpriteRenderer shield;

    public Customs customs;

    PlayerData playerData;

    void Awake(){

        instance = this;

    }

    public void LoadCustomsData(){

        playerData = PlayGamesManager.instance.playerData;

        player.sprite = customs.playerSkins[playerData.playerSkin].texture;
        player.color = customs.playerColors[playerData.playerColor].color;

        shield.sprite = customs.shieldSkins[playerData.shieldSkin].texture;
        shield.color = customs.shieldColors[playerData.shieldColor].color;

        for(int i = 0; i < customs.playerColors.Length; i++){

            customs.playerColors[i].isOwned = playerData.playerColorsUnlocked[i];
            customs.playerColors[i].watchedAds = playerData.playerColorsWatchedAds[i];

        }

        for(int i = 0; i < customs.playerSkins.Length; i++){

            customs.playerSkins[i].isOwned = playerData.playerSkinsUnlocked[i];
            customs.playerSkins[i].watchedAds = playerData.playerSkinsWatchedAds[i];

        }

        for(int i = 0; i < customs.shieldColors.Length; i++){

            customs.shieldColors[i].isOwned = playerData.shieldColorsUnlocked[i];
            customs.shieldColors[i].watchedAds = playerData.shieldColorsWatchedAds[i];

        }

        for(int i = 0; i < customs.shieldSkins.Length; i++){

            customs.shieldSkins[i].isOwned = playerData.shieldSkinsUnlocked[i];
            customs.shieldSkins[i].watchedAds = playerData.shieldSkinsWatchedAds[i];

        }


    }

    public void UsePlayerColor(int id){

        CustomColor data = customs.playerColors[id];
        if(data.isOwned){

            player.color = data.color;
            playerData.playerColor = id;

        }else{

            switch(data.unlockType){

                case UnlockType.SCORE:

                    if(playerData.totalScore >= data.scorePrice){

                        data.isOwned = true;
                        playerData.totalScore -= data.scorePrice;
                        player.color = data.color;
                        playerData.playerColor = id;

                    }
                    break;

                case UnlockType.ADS:

                    AdMob.instance.rewardedAd.OnAdFullScreenContentClosed += () => {

                        data.watchedAds++;
                        playerData.playerColorsWatchedAds[id] = data.watchedAds;
                        if(data.watchedAds == data.adsPrice){

                            data.isOwned = true;
                            playerData.playerColorsUnlocked[id] = true;

                        }

                        //AdMob.instance.rewardedAd.OnAdFullScreenContentClosed = null;
                        
                    };

                    break; 

                case UnlockType.MONEY:

                    //Kod do IAP, narazie nie mam nawet pluginu iap. nie wiem czy dodam opcje iap czy zostawie poprzednie i chuj

                    break;

            }

        }

        if(PlayGamesManager.instance.playerSignedIn){

            PlayGamesManager.instance.playerData = playerData;

            PlayGamesManager.instance.SaveGameData();

        }
        PlayerPrefs.SetString("PlayerData", JsonUtility.ToJson(playerData));
        PlayerPrefs.Save();
        LoadCustomsData();

    }
}

[System.Serializable]
public class Customs{

    public CustomColor[] playerColors;
    public CustomSkin[] playerSkins;
    public CustomColor[] shieldColors;
    public CustomSkin[] shieldSkins;

}

[System.Serializable]
public class CustomData{

    public bool isOwned;
    public UnlockType unlockType;
    public int scorePrice;
    public int adsPrice;
    public int watchedAds;
    public float moneyPrice;

}

public enum UnlockType{SCORE, ADS, ACHIEVEMENT, MONEY}

[System.Serializable]
public class CustomColor : CustomData{

    public Color color;
    

}

[System.Serializable]
public class CustomSkin : CustomData{

    public Sprite texture;
    

}