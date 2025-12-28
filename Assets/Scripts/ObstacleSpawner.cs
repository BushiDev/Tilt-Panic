using UnityEngine;
using System.Collections;

public class ObstacleSpawner : MonoBehaviour{

    public static ObstacleSpawner instance;

    public GameObject prefab;
    public float spawnDelay = 1.5f;
    public float fallSpeed = 3f;

    public bool isPaused = true;

    void Awake(){

        if(instance != null && instance != this){

            Destroy(gameObject);

        }else{

            instance = this;

        }

    }

    void Start(){

        StartCoroutine(SpawnLoop());

    }

    IEnumerator SpawnLoop(){

        while(true){

            if(!isPaused){

                SpawnObstacle();

                spawnDelay = Mathf.Max(0.2f, spawnDelay - 0.01f);
                fallSpeed += 0.03f;

                yield return new WaitForSeconds(spawnDelay);

            }else{

                yield return new WaitForSeconds(spawnDelay);

            }


        }

    }

    void SpawnObstacle(){

        float x = Random.Range(0, 1f);
        Vector3 pos = Camera.main.ViewportToWorldPoint(new Vector3(x, 1.5f, 0f));
        pos.z = 0f;

        GameObject o = Instantiate(prefab, pos, Quaternion.Euler(0f, 0f, 180f));
        o.GetComponent<Rigidbody2D>().linearVelocity = Vector2.down * fallSpeed;
        Destroy(o, 5f);

    }

}