using UnityEngine;
using System;
using GoogleMobileAds;
using GoogleMobileAds.Api;

public class AdMob : MonoBehaviour{

    public static AdMob instance;

    public string bottomBannerId = "ca-app-pub-8369690861079353/9109198592";
    public string deathInterstitialId = "ca-app-pub-8369690861079353/7796116927";
    public string rewardId = "ca-app-pub-8369690861079353/1135533322";
    public AdPosition bannerPosition;

    public BannerView bannerView;

    AdRequest adRequest;
    InterstitialAd interstitialAd;
    RewardedAd rewardedAd;

    public Action reward;

    void Start(){

        instance = this;

        MobileAds.Initialize((InitializationStatus status) => {

            if(status == null){

                return;

            }else{

                bannerView = new BannerView(bottomBannerId, AdSize.Banner, bannerPosition);
                bannerView.LoadAd(new AdRequest());
                LoadFullscreenAd();
                LoadRewardedAd();

                interstitialAd.OnAdFullScreenContentClosed += () => {
                    
                    LoadFullscreenAd();
                    Time.timeScale = 1f;
                    
                };

                interstitialAd.OnAdFullScreenContentFailed += (AdError e) => {

                    Debug.LogError("Interstitial failed: " + e);
                    Time.timeScale = 1f; // nigdy nie zostawiamy czasu w pause
                    LoadFullscreenAd();

                };

                rewardedAd.OnAdFullScreenContentClosed += () => {

                    LoadRewardedAd();
                    Shop.instance.LoadCustomsData();
                    Shop.instance.UpdateShopUI();
                    Time.timeScale = 1f;

                };

                rewardedAd.OnAdFullScreenContentFailed += (AdError e) => {

                    Debug.LogError("Rewarded failed: " + e);
                    Time.timeScale = 1f; // nigdy nie zostawiamy czasu w pause
                    LoadRewardedAd();

                };

                //rewardedAd.OnAdFullScreenContentClosed += () => {reward();};

            }

        });

    }

    public void LoadFullscreenAd(){

        adRequest = new AdRequest();
        InterstitialAd.Load(deathInterstitialId, adRequest, (InterstitialAd ad, LoadAdError error) => {

            if(error != null) return;

            interstitialAd = ad;

        });

    }

    public void ShowFullscreenAd(){

        Debug.Log("Showing fullscreen ad");

        if(interstitialAd != null && interstitialAd.CanShowAd()){

            interstitialAd.Show();
            Time.timeScale = 1f;

        }

    }

    public void LoadRewardedAd(){

        adRequest = new AdRequest();

        RewardedAd.Load(rewardId, adRequest, (RewardedAd ad, LoadAdError error) => {

            if(error != null) return;
            rewardedAd = ad;

        });

    }

    public void ShowRewardedAd(){

        Debug.Log("Showing rewarded ad");

        if(rewardedAd != null && rewardedAd.CanShowAd()){

            rewardedAd.Show((Reward r) => {

                reward?.Invoke();
                this.reward = null;
                LoadRewardedAd();
                Shop.instance.LoadCustomsData();
                Shop.instance.UpdateShopUI();

            });
            Time.timeScale = 1f;

        }else{

            Debug.Log("Cannot show rewarder ad");
            LoadRewardedAd();

        }

    }

}