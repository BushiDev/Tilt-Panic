using UnityEngine;
using GoogleMobileAds.Api;

public class AdMob : MonoBehaviour{

    public static AdMob instance;

    public string bottomBannerId = "ca-app-pub-8369690861079353/9109198592";
    public string deathInterstitialId = "ca-app-pub-8369690861079353/7796116927";
    public string rewardId = "ca-app-pub-8369690861079353/1135533322";

    public AdPosition bannerPosition;

    BannerView bannerView;

    AdRequest adRequest;
    InterstitialAd interstitialAd;
    RewardedAd rewardedAd;

    public System.Action reward;

    void Awake(){

        if(instance != null && instance != this){

            Destroy(gameObject);

        }else{

            instance = this;
            DontDestroyOnLoad(gameObject);

        }

    }

    void Start(){

        MobileAds.Initialize((InitializationStatus status) => {

            if(status == null){

                return;

            }else{

                bannerView = new BannerView(bottomBannerId, AdSize.Banner, bannerPosition);
                bannerView.LoadAd(new AdRequest());

                LoadFullscreenAd();
                LoadRewardedAd();

            }

        });

    }

    public void LoadFullscreenAd(){

        adRequest = new AdRequest();
        InterstitialAd.Load(deathInterstitialId, adRequest, (InterstitialAd ad, LoadAdError error) => {

            if(error != null) return;

            if(interstitialAd != null){

                interstitialAd.OnAdFullScreenContentClosed -= LoadFullscreenAd;
                interstitialAd.OnAdFullScreenContentFailed -= ContentFailed;
                interstitialAd.OnAdFullScreenContentClosed -= ResumeGame;
                interstitialAd.OnAdFullScreenContentFailed -= ContentFailed;

            }

            interstitialAd = ad;

            if(interstitialAd != null){

                interstitialAd.OnAdFullScreenContentClosed += LoadFullscreenAd;
                interstitialAd.OnAdFullScreenContentFailed += ContentFailed;
                interstitialAd.OnAdFullScreenContentClosed += ResumeGame;
                interstitialAd.OnAdFullScreenContentFailed += ContentFailed;

            }

        });

    }

    void ContentFailed(AdError e){ResumeGame();}

    public void ShowFullscreenAd(){

        if(interstitialAd != null && interstitialAd.CanShowAd()){

            Time.timeScale = 0f;
            
            interstitialAd.Show();
            
        }

    }

    public void LoadRewardedAd(){

        adRequest = new AdRequest();

        RewardedAd.Load(rewardId, adRequest, (RewardedAd ad, LoadAdError error) => {

            if(error != null) return;

            if(rewardedAd != null){

                rewardedAd.OnAdFullScreenContentFailed -= RewardFailed;
                rewardedAd.OnAdFullScreenContentClosed -= ResumeGame;
                rewardedAd.OnAdFullScreenContentClosed -= RewardClosed;

            }
            rewardedAd = ad;

            if(rewardedAd != null){

                rewardedAd.OnAdFullScreenContentFailed += RewardFailed;
                rewardedAd.OnAdFullScreenContentClosed += ResumeGame;
                rewardedAd.OnAdFullScreenContentClosed += RewardClosed;

            }

        });

    }

    void RewardClosed(){

        LoadRewardedAd();
        Shop.instance.LoadCustomsData();
        Shop.instance.UpdateShopUI();

    }

    void RewardFailed(AdError error){

        LoadRewardedAd();
        ResumeGame();

    }

    public void ShowRewardedAd(){

        if(rewardedAd != null && rewardedAd.CanShowAd()){

            Time.timeScale = 0f;

            rewardedAd.Show((Reward r) => {

                if(reward == null) return;

                reward?.Invoke();
                this.reward = null;

            });

        }else{

            UIController.instance.ShowSelected(7);
            LoadRewardedAd();

        }

    }

    void ResumeGame(){

        Time.timeScale = 1f;

    }

}