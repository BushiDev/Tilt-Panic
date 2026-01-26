using UnityEngine;
using UnityEngine.UI;

public class DevModeManager : MonoBehaviour{

    public static DevModeManager instance{get; private set;}

    public CanvasGroup canvasGroup;
    public GameObject shieldPrefab;
    public Transform player;
    SpriteRenderer spriteRenderer;
    public SpriteRenderer shieldRenderer;

    public Sprite[] skins;
    int currentSkin;
    int currentPlayerColor;
    int currentShieldColor;

    public bool devModeEnabled{get; private set;}
    public bool godMode{get; private set;}
    public bool infiniteShield{
        
        get{

            return shield;

        }

        private set{

            shield = value;
            if(shield) Shield.instance.isActive = true;

        }
        
    }
    public bool showAds{get; private set;}
    public bool showUI{
        
        get{

            return ui;

        }
        
        private set{

            ui = value;
            canvasGroup.alpha = ui ? 1f : 0f;

        }
        
    }

    bool shield;
    bool ui = true;

    void Awake(){

        if(instance != null && instance != this){

            Destroy(gameObject);

        }else{

            instance = this;

        }

    }

    void Start(){
        
        spriteRenderer = player.GetComponent<SpriteRenderer>();
        
    }

    void Update(){

        if(Input.GetKeyDown(KeyCode.M)){

            if(!devModeEnabled) GameManager.instance.devModeWasEnabled = true;
            
            devModeEnabled = !devModeEnabled;

        }
        
        if(Input.GetKeyDown(KeyCode.P) && devModeEnabled) godMode = !godMode;
        if(Input.GetKeyDown(KeyCode.I) && devModeEnabled) infiniteShield = !infiniteShield;
        if(Input.GetKeyDown(KeyCode.L) && devModeEnabled) showAds = !showAds;
        if(Input.GetKeyDown(KeyCode.U) && devModeEnabled) showUI = !showUI;
        if(Input.GetKeyDown(KeyCode.O) && devModeEnabled) SpawnShield();
        if(Input.GetKeyDown(KeyCode.K) && devModeEnabled) SkinChange();
        if(Input.GetKeyDown(KeyCode.J) && devModeEnabled) PlayerColorChange();
        if(Input.GetKeyDown(KeyCode.N) && devModeEnabled) ShieldColorChange();
        if(Input.GetKeyDown(KeyCode.Y) && devModeEnabled) Time.timeScale = 0.5f;
        if(Input.GetKeyDown(KeyCode.T) && devModeEnabled) Time.timeScale = 1.5f;
        if(Input.GetKeyDown(KeyCode.R) && devModeEnabled) Time.timeScale = 1f;


    }

    void SpawnShield(){

        GameObject o = Instantiate(shieldPrefab, player.position + Vector3.up * 10f, Quaternion.identity);
        o.GetComponent<Rigidbody2D>().linearVelocity = Vector2.down * ObstacleSpawner.instance.fallSpeed / 2f;
        Destroy(o, 15f);

    }

    void SkinChange(){

        currentSkin++;
        if(currentSkin == skins.Length) currentSkin = 0;

        spriteRenderer.sprite = skins[currentSkin];

    }

    void PlayerColorChange(){

        currentPlayerColor++;
        if(currentPlayerColor == Shop.instance.customs.playerColors.Length) currentPlayerColor = 0;

        spriteRenderer.color = Shop.instance.customs.playerColors[currentPlayerColor].color;

    }

    void ShieldColorChange(){

        currentShieldColor++;
        if(currentShieldColor == Shop.instance.customs.shieldColors.Length) currentShieldColor = 0;

        PlayGamesManager.instance.playerData.shieldColor = currentShieldColor;

        shieldRenderer.color = Shop.instance.customs.shieldColors[currentShieldColor].color;

    }

}