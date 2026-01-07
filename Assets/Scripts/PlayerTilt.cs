using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerTilt : MonoBehaviour{

    public float speed = 20f;
    public float deadZone = 0.02f;
    Rigidbody2D rigidbody2d;
    public SpriteRenderer spriteRenderer;
    public GameObject deathParticles;

    public float smooth = 8f;

    float currentTilt;

    void Start(){

#if UNITY_ANDROID
        InputSystem.EnableDevice(UnityEngine.InputSystem.Accelerometer.current);
#endif
        rigidbody2d = GetComponent<Rigidbody2D>();


    }

    void Update(){

        if(ObstacleSpawner.instance.isPaused) return;
#if UNITY_EDITOR
        float rawTilt = Random.Range(-1f, 1f);
#else
        float rawTilt = Accelerometer.current.acceleration.ReadValue().x;
#endif
        if(Mathf.Abs(rawTilt) < deadZone) rawTilt = 0f;
        currentTilt = Mathf.Lerp(currentTilt, rawTilt, Time.deltaTime * smooth);
        transform.position += Vector3.right * currentTilt * speed * Time.deltaTime;
        ClampToScreen();

    }

    void ClampToScreen(){

        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = Camera.main.ViewportToWorldPoint(pos);

    }

    void OnTriggerEnter2D(Collider2D collider2D){

        if(collider2D.tag.Equals("Obstacle")){

            GetComponent<AudioSource>().Play();

            if(Settings.instance.settingsData.vibrations) RDG.Vibration.Vibrate(250);

            StartCoroutine(Die());

        }

    }

    IEnumerator Die(){

        CameraHitZoom.instance.Hit(transform.position);
        SurvivalTimer.instance.isAlive = false;
        ObstacleSpawner.instance.isPaused = true;

        yield return new WaitForSeconds(0.2f);

        Time.timeScale = 0.6f;

        UIController.instance.HideAll();
        GameManager.instance.RemoveAllObstacles();
        spriteRenderer.enabled = false;
        GameObject go = Instantiate(deathParticles, transform.position, Quaternion.Euler(90f, 0f, 0f));
        Destroy(go, 2.5f);

        yield return new WaitForSeconds(0.5f);

        Time.timeScale = 1f;

        yield return new WaitForSeconds(1f);

        GameManager.instance.GameOver();
        spriteRenderer.enabled = true;

    }

}