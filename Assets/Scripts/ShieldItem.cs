using UnityEngine;

public class ShieldItem : Obstacle{

    void OnTriggerEnter2D(Collider2D collider){

        if(collider.gameObject.tag.Equals("Player")){

            if(PlayGamesManager.instance.playerSignedIn) PlayGamesManager.instance.CollectAchievement(GPGSIds.achievement_shield_off_i_on);

            Shield.instance.isActive = true;
            Destroy(gameObject);

        }

    }

}