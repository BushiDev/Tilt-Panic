using UnityEngine;

public class Shield : MonoBehaviour{

    public static Shield instance;
    Collider2D myCollider;
    SpriteRenderer spriteRenderer;

    public AudioClip collected;
    public AudioClip destroyed;

    public AudioSource source;

    bool active;

    public bool isActive{

        get{return active;}
        set{
            
            active = value;
            spriteRenderer.enabled = active;
            myCollider.enabled = active;

            source.clip = active ? collected : destroyed;
            source.Play();

            if(Settings.instance.settingsData.vibrations) RDG.Vibration.Vibrate(100);

            
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