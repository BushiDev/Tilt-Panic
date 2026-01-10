using UnityEngine;

public class ShieldItem : Obstacle{

    SpriteRenderer spriteRenderer;

    void Start(){

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Shop.instance != null ? Shop.instance.customs.shieldColors[PlayGamesManager.instance.playerData.shieldColor].color : new Color(1f, 1f, 1f, 0.2f);

    }

    void OnTriggerEnter2D(Collider2D collider){

        if(collider.gameObject.tag.Equals("Player")){

            if(PlayGamesManager.instance != null && PlayGamesManager.instance.playerSignedIn) PlayGamesManager.instance.CollectAchievement(GPGSIds.achievement_shield_off_i_on);

            if(Shield.instance != null) Shield.instance.isActive = true;
            Destroy(gameObject);

        }

    }

}