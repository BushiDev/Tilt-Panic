using UnityEngine;

public class ShieldItem : MonoBehaviour{

    void OnTriggerEnter2D(Collider2D collider){

        if(collider.gameObject.tag.Equals("Player")){

            Shield.instance.isActive = true;
            Destroy(gameObject);

        }

    }

}