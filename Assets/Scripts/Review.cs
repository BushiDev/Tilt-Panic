using UnityEngine;
using Google.Play.Review;
using System.Collections;

public class Review : MonoBehaviour{

    ReviewManager reviewManager;
    PlayReviewInfo reviewInfo;

    void Start(){

        reviewManager = new ReviewManager();

        if(!PlayerPrefs.HasKey("Review")){

            PlayerPrefs.SetInt("Review", 0);
            PlayerPrefs.Save();

        }

    }

    public void ShowReview() => StartCoroutine(RequestReview());

    IEnumerator RequestReview(){

        var request = reviewManager.RequestReviewFlow();

        yield return request;

        if(request.Error != ReviewErrorCode.NoError){

            Debug.Log("Review error: " + request.Error);

            Application.OpenURL("https://play.google.com/store/apss/details?id=com.BubbleGamesStudio.TiltPanic");

            PlayerPrefs.SetInt("Review", 1);
            PlayerPrefs.Save();

            yield break;

        }

        reviewInfo = request.GetResult();

        var launch = reviewManager.LaunchReviewFlow(reviewInfo);
        yield return launch;

        PlayerPrefs.SetInt("Review", 1);
        PlayerPrefs.Save();

    }

    public void NoReview(){

        PlayerPrefs.SetInt("Review", 1);
        PlayerPrefs.Save();

    }

}