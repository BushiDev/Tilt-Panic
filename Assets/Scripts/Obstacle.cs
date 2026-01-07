using UnityEngine;

public class Obstacle : MonoBehaviour{

    public GameObject particlesPrefab;

    void OnDestroy(){

        GameObject go = Instantiate(particlesPrefab, transform.position, transform.rotation);
        Destroy(go, 2.5f);

    }

}