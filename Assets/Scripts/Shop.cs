using UnityEngine;
using TMPro;

public class Shop : MonoBehaviour{

    public static Shop instance;

    public SpriteRenderer player;
    public SpriteRenderer shield;

    public Customs customs;

    PlayerData playerData;

    public TMP_Text scoreText;

    public TMP_Text[] playerColorsText;
    public TMP_Text[] playerSkinsText;
    public TMP_Text[] shieldColorsText;
    public TMP_Text[] shieldSkinsText;

    void Awake(){

        if(instance != null && instance != this){

            Destroy(gameObject);

        }else{

            instance = this;
            DontDestroyOnLoad(gameObject);

        }

    }

    void Start(){

        LoadCustomsData();

    }

    public void UpdateShopUI(){

        scoreText.text = playerData.totalScore.ToString();

        UpdateTexts(playerColorsText, customs.playerColors, DataType.PLAYER_COLOR);
        UpdateTexts(playerSkinsText, customs.playerSkins, DataType.PLAYER_SKIN);
        UpdateTexts(shieldColorsText, customs.shieldColors, DataType.SHIELD_COLOR);
        UpdateTexts(shieldSkinsText, customs.shieldSkins, DataType.SHIELD_SKIN);

    }

    void UpdateTexts(TMP_Text[] texts, CustomData[] data, DataType dataType){

        int usedData = dataType == DataType.PLAYER_COLOR ? playerData.playerColor : dataType == DataType.PLAYER_SKIN ? playerData.playerSkin : dataType == DataType.SHIELD_COLOR ? playerData.shieldColor : playerData.shieldSkin;

        for(int i = 0; i < data.Length; i++){

            bool itemUnlocked = dataType == DataType.PLAYER_COLOR ? playerData.playerColorsUnlocked[i] : dataType == DataType.PLAYER_SKIN ? playerData.playerSkinsUnlocked[i] : dataType == DataType.SHIELD_COLOR ? playerData.shieldColorsUnlocked[i] : playerData.shieldSkinsUnlocked[i];

            if(i == usedData){

                texts[i].text = "Equipped";

            }else if(itemUnlocked){

                texts[i].text = "Owned";

            }else{

                switch(data[i].unlockType){

                    case UnlockType.SCORE:

                        texts[i].text = data[i].scorePrice.ToString();
                    
                        break;

                    case UnlockType.ADS:

                        int adsPrice = dataType == DataType.PLAYER_COLOR ? playerData.playerColorsWatchedAds[i] : dataType == DataType.PLAYER_SKIN ? playerData.playerSkinsWatchedAds[i] : dataType == DataType.SHIELD_COLOR ? playerData.shieldColorsWatchedAds[i] : playerData.shieldSkinsWatchedAds[i];

                        texts[i].text = "Ads: " + adsPrice + " / " + data[i].adsPrice.ToString();

                        break;

                }

            }

        }

    }

    //Ta sama funckja co wyzej jeszcze 3 razy dla skinow x 2 i koloru x 1

    public void LoadCustomsData(){

        playerData = PlayGamesManager.instance.playerData;

        player.sprite = customs.playerSkins[playerData.playerSkin].texture;
        player.color = customs.playerColors[playerData.playerColor].color;

        shield.sprite = customs.shieldSkins[playerData.shieldSkin].texture;
        shield.color = customs.shieldColors[playerData.shieldColor].color;

    }

    public void UsePlayerColor(int id){

        Debug.Log("Using player color with id: " + id.ToString());

        CustomColor data = customs.playerColors[id];
        if(playerData.playerColorsUnlocked[id]){

            Debug.Log("Color owned. Setting up...");

            player.color = data.color;
            playerData.playerColor = id;

        }else{

            Debug.Log("Color not owned...");

            switch(data.unlockType){

                case UnlockType.SCORE:

                    if(playerData.totalScore >= data.scorePrice){

                        playerData.playerColorsUnlocked[id] = true;
                        playerData.totalScore -= data.scorePrice;
                        player.color = data.color;
                        playerData.playerColor = id;

                    }
                    break;

                case UnlockType.ADS:

                    AdMob.instance.reward = () => {

                        playerData.playerColorsWatchedAds[id]++;
                        if(playerData.playerColorsWatchedAds[id] >= data.adsPrice){

                            playerData.playerColorsUnlocked[id] = true;

                        }
                        
                    };

                    AdMob.instance.ShowRewardedAd();

                    break; 

            }

        }

        SaveShopData();

    }

    public void UsePlayerSkin(int id){

        Debug.Log("Using player skin with id: " + id.ToString());

        CustomSkin data = customs.playerSkins[id];
        if(playerData.playerSkinsUnlocked[id]){

            Debug.Log("Color owned. Setting up...");

            player.sprite = data.texture;
            playerData.playerSkin = id;

        }else{

            Debug.Log("Color not owned...");

            switch(data.unlockType){

                case UnlockType.SCORE:

                    if(playerData.totalScore >= data.scorePrice){

                        playerData.playerSkinsUnlocked[id] = true;
                        playerData.totalScore -= data.scorePrice;
                        player.sprite = data.texture;
                        playerData.playerSkin = id;

                    }
                    break;

                case UnlockType.ADS:

                    AdMob.instance.reward = () => {

                        playerData.playerSkinsWatchedAds[id]++;
                        if(playerData.playerSkinsWatchedAds[id] >= data.adsPrice){

                            playerData.playerSkinsUnlocked[id] = true;

                        }
                        
                    };

                    AdMob.instance.ShowRewardedAd();

                    break; 

            }

        }

        SaveShopData();

    }

    public void UseShieldColor(int id){

        Debug.Log("Using shield color with id: " + id.ToString());

        CustomColor data = customs.shieldColors[id];
        if(playerData.shieldColorsUnlocked[id]){

            Debug.Log("Color owned. Setting up...");

            shield.color = data.color;
            playerData.shieldColor = id;

        }else{

            Debug.Log("Color not owned...");

            switch(data.unlockType){

                case UnlockType.SCORE:

                    if(playerData.totalScore >= data.scorePrice){

                        playerData.shieldColorsUnlocked[id] = true;
                        playerData.totalScore -= data.scorePrice;
                        shield.color = data.color;
                        playerData.shieldColor = id;

                    }
                    break;

                case UnlockType.ADS:

                    AdMob.instance.reward = () => {

                        playerData.shieldColorsWatchedAds[id]++;
                        if(playerData.shieldColorsWatchedAds[id] >= data.adsPrice){

                            playerData.shieldColorsUnlocked[id] = true;

                        }
                        
                    };

                    AdMob.instance.ShowRewardedAd();

                    break; 

            }

        }

        SaveShopData();

    }

    public void UseShieldSkin(int id){

        Debug.Log("Using shield skin with id: " + id.ToString());

        CustomSkin data = customs.shieldSkins[id];
        if(playerData.shieldSkinsUnlocked[id]){

            Debug.Log("Color owned. Setting up...");

            shield.sprite = data.texture;
            playerData.shieldSkin = id;

        }else{

            Debug.Log("Color not owned...");

            switch(data.unlockType){

                case UnlockType.SCORE:

                    if(playerData.totalScore >= data.scorePrice){

                        playerData.shieldSkinsUnlocked[id] = true;
                        playerData.totalScore -= data.scorePrice;
                        shield.sprite = data.texture;
                        playerData.shieldSkin = id;

                    }
                    break;

                case UnlockType.ADS:

                    AdMob.instance.reward = () => {

                        playerData.shieldSkinsWatchedAds[id]++;
                        if(playerData.shieldSkinsWatchedAds[id] >= data.adsPrice){

                            playerData.shieldSkinsUnlocked[id] = true;

                        }
                        
                    };

                    AdMob.instance.ShowRewardedAd();

                    break; 

            }

        }

        SaveShopData();

    }

    void SaveShopData(){

        PlayGamesManager.instance.playerData = playerData;

        if(PlayGamesManager.instance.playerSignedIn){

            PlayGamesManager.instance.SaveGameData();

        }
        PlayerPrefs.SetString("PlayerData", JsonUtility.ToJson(playerData));
        PlayerPrefs.Save();
        LoadCustomsData();
        UpdateShopUI();

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

    public UnlockType unlockType;
    public int scorePrice;
    public int adsPrice;
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

public enum DataType{PLAYER_COLOR, PLAYER_SKIN, SHIELD_COLOR, SHIELD_SKIN}