using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTilt : MonoBehaviour{

    public float speed = 20f;
    Rigidbody2D rigidbody2d;

    InputAction tiltAction;

    void Start(){

        InputSystem.EnableDevice(UnityEngine.InputSystem.Gyroscope.current);

        Input.gyro.enabled = true;

        rigidbody2d = GetComponent<Rigidbody2D>();

        tiltAction = InputSystem.actions.FindAction("Tilt/Tilt");

    }

    void Update(){

        if(ObstacleSpawner.instance.isPaused) return;

        float tilt = UnityEngine.InputSystem.Gyroscope.current.angularVelocity.ReadValue().x;
        Vector3 move = new Vector3(tilt, 0f, 0f);
        transform.position += move * speed * Time.deltaTime;

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

            GameManager.instance.GameOver();

        }

    }

}