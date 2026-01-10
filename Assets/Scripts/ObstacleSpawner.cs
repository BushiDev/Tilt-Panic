using UnityEngine;
using System.Collections;

public class ObstacleSpawner : MonoBehaviour{

    public static ObstacleSpawner instance;

    public GameObject prefab;
    public GameObject shield;
    public float spawnDelay = 1.5f;
    public float fallSpeed = 3f;

    public bool isPaused = true;

    Camera mainCamera;

    void Awake(){

        if(instance != null && instance != this){

            Destroy(gameObject);

        }else{

            instance = this;
            DontDestroyOnLoad(gameObject);

        }

    }

    void Start(){

        mainCamera = Camera.main;

        StartCoroutine(SpawnLoop());

    }

    IEnumerator SpawnLoop(){

        while(true){

            if(!isPaused){

                SpawnObstacle();

                spawnDelay = Mathf.Max(0.3f, spawnDelay - 0.015f);
                fallSpeed += 0.075f;

                yield return new WaitForSeconds(spawnDelay);

            }else{

                yield return new WaitForSeconds(0.1f);

            }


        }

    }

    void SpawnObstacle(){

        float x = Random.Range(0, 1f);
        Vector3 pos = mainCamera.ViewportToWorldPoint(new Vector3(x, 1.1f, 0f));
        pos.z = 0f;

        if(Random.Range(0, 100) > 7){

            GameObject o = Instantiate(prefab, pos, Quaternion.identity);
            o.GetComponent<Rigidbody2D>().linearVelocity = Vector2.down * fallSpeed;
            Destroy(o, 10f);

        }else{

            GameObject o = Instantiate(shield, pos, Quaternion.identity);
            o.GetComponent<Rigidbody2D>().linearVelocity = Vector2.down * fallSpeed / 2f;
            Destroy(o, 15f);
        }

    }

    public void Reset(){

        spawnDelay = 1.5f;
        fallSpeed = 3f;

    }

}