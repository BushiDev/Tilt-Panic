using UnityEngine;

public class Shield : MonoBehaviour{

    public static Shield instance;
    Collider2D myCollider;
    SpriteRenderer spriteRenderer;

    bool active;

    public bool isActive{

        get{return active;}
        set{
            
            active = value;
            spriteRenderer.enabled = active;
            myCollider.enabled = active;

            
        }

    }

    void Awake(){

        instance = this;

    }

    void Start(){

        myCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.enabled = false;
        myCollider.enabled = false;

    }

    void OnTriggerEnter2D(Collider2D collider){

        if(collider.gameObject.tag.Equals("Obstacle")){

            Destroy(collider.gameObject);
            isActive = false;

        }

    }

}