using UnityEngine;
using System.Collections;

public class ObstacleSpawner : MonoBehaviour{

    public GameObject prefab;
    public float spawnDelay = 1.2f;
    public float fallSpeed = 3f;

    void Start(){

        StartCoroutine(SpawnLoop());

    }

    IEnumerator SpawnLoop(){

        while(true){

            SpawnObstacle();

            spawnDelay = Mathf.Max(0.3f, spawnDelay - 0.01f);
            fallSpeed += 0.02f;

            yield return new WaitForSeconds(spawnDelay);

        }

    }

    void SpawnObstacle(){

        float x = Random.Range(0, 1f);
        Vector3 pos = Camera.main.ViewportToWorldPoint(new Vector3(x, 1.1f, 0f));
        pos.z = 0f;

        GameObject o = Instantiate(prefab, pos, Quaternion.Euler(0f, 0f, 180f));
        o.GetComponent<Rigidbody2D>().linearVelocity = Vector2.down * fallSpeed;
        Destroy(o, 5f);

    }

}