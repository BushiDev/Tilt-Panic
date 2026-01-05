using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTilt : MonoBehaviour{

    public float speed = 20f;
    public float deadZone = 0.02f;
    Rigidbody2D rigidbody2d;

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

            GameManager.instance.GameOver();

        }

    }

}