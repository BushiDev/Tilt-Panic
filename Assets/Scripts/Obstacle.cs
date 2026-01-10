using UnityEngine;

public class Obstacle : MonoBehaviour{

    public GameObject particlesPrefab;

    void OnDestroy(){

        GameObject go = Instantiate(particlesPrefab, transform.position, transform.rotation);
        var main = go.GetComponent<ParticleSystem>().main;
        main.startColor = GetComponent<SpriteRenderer>().color;
        Destroy(go, 2.5f);

    }

}