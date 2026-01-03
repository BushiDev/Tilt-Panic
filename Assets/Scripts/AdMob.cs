using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;

public class AdMob : MonoBehaviour{

    public static AdMob instance;

    public string bottomBannerId = "ca-app-pub-8369690861079353/9109198592";
    public string deathInterstitialId = "ca-app-pub-8369690861079353/7796116927";
    public AdPosition bannerPosition;

    public BannerView bannerView;

    AdRequest adRequest;
    InterstitialAd interstitialAd;

    void Start(){

        instance = this;

        MobileAds.Initialize((InitializationStatus status) => {

            if(status == null){

                return;

            }else{

                bannerView = new BannerView(bottomBannerId, AdSize.Banner, bannerPosition);
                bannerView.LoadAd(new AdRequest());
                LoadFullscreenAd();

                interstitialAd.OnAdFullScreenContentClosed += () => {
                    
                    LoadFullscreenAd();
                    Time.timeScale = 1f;
                    
                };

                interstitialAd.OnAdFullScreenContentFailed += (AdError e) => {

                    Debug.LogError("Interstitial failed: " + e);
                    Time.timeScale = 1f; // nigdy nie zostawiamy czasu w pause
                    LoadFullscreenAd();

                };

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

}