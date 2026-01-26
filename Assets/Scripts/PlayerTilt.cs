using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerTilt : MonoBehaviour{

    public static PlayerTilt instance;

    public float speed = 20f;
    public float deadZone = 0.02f;
    public SpriteRenderer spriteRenderer;
    public GameObject deathParticles;

    public float smooth = 8f;

    float currentTilt;

    public AudioSource audioSource;
    public Camera mainCamera;

    public bool saveByInvert;

    float rawTilt;

    void Awake(){

        if(instance != null && instance != this){

            Destroy(gameObject);

        }else{

            instance = this;
            DontDestroyOnLoad(gameObject);

        }

    }

    void Start(){

#if UNITY_ANDROID
        InputSystem.EnableDevice(UnityEngine.InputSystem.Accelerometer.current);
#endif

    }

    void Update(){

        if(ObstacleSpawner.instance.isPaused) return;
        if(DevModeManager.instance.devModeEnabled) rawTilt = Keyboard.current.aKey.isPressed ? -1f : Keyboard.current.dKey.isPressed ? 1f : 0f;
        else rawTilt = Accelerometer.current.acceleration.ReadValue().x;

        if(Mathf.Abs(rawTilt) < deadZone) rawTilt = 0f;
        currentTilt = Mathf.Lerp(currentTilt, rawTilt, Time.deltaTime * smooth);
        transform.position += Vector3.right * currentTilt * speed * Time.deltaTime * Settings.instance.settingsData.sensitivity * Time.timeScale;
        ClampToScreen();

    }

    void ClampToScreen(){

        Vector3 pos = mainCamera.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = mainCamera.ViewportToWorldPoint(pos);

    }

    void OnTriggerEnter2D(Collider2D collider2D){

        if(DevModeManager.instance.godMode) return;

        if(collider2D.tag.Equals("Obstacle") && !saveByInvert){

            audioSource.Play();

            if(Settings.instance.settingsData.vibrations) RDG.Vibration.Vibrate(250);

            StartCoroutine(Die());

        }

    }

    IEnumerator Die(){

        CameraHitZoom.instance.Hit(transform.position);
        SurvivalTimer.instance.isAlive = false;
        ObstacleSpawner.instance.isPaused = true;

        yield return new WaitForSeconds(0.2f);

        Time.timeScale = 0.75f;

        UIController.instance.HideAll();
        GameManager.instance.RemoveAllObstacles();
        spriteRenderer.enabled = false;
        GameObject go = Instantiate(deathParticles, transform.position, Quaternion.identity);
        var main = go.GetComponent<ParticleSystem>().main;
        main.startColor = spriteRenderer.color;
        Destroy(go, 2.5f);
        //jest jak chce, wiem ze timescale wplywa. zostaje
        yield return new WaitForSeconds(0.5f);

        Time.timeScale = 1f;

        yield return new WaitForSeconds(1f);

        spriteRenderer.enabled = true;
        GameManager.instance.GameOver();

    }

}