using UnityEngine;

public class PlayerTilt : MonoBehaviour{

    public float speed = 20f;
    Rigidbody2D rigidbody2d;

    void Start(){

        Input.gyro.enabled = true;

        rigidbody2d = GetComponent<Rigidbody2D>();

    }

    void Update(){

        if(ObstacleSpawner.instance.isPaused) return;

        Vector3 tilt = Input.acceleration;
        Vector3 move = new Vector3(tilt.x, 0f, 0f);
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