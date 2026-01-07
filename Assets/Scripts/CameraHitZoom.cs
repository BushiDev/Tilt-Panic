using UnityEngine;
using System.Collections;

public class CameraHitZoom : MonoBehaviour{

    public static CameraHitZoom instance;

    Camera camera;
    float defaultSize;
    public float zoom = 0.85f;
    Vector3 defaultPosition;
    public float zoomInTime = 0.1f;
    public float zoomOutTime = 0.05f;

    void Awake(){

        instance = this;

    }

    void Start(){

        camera = GetComponent<Camera>();
        defaultSize = camera.orthographicSize;
        defaultPosition = transform.position;

    }

    public void Hit(Vector3 position){

        StopAllCoroutines();
        StartCoroutine(Zoom(position));

    }

    IEnumerator Zoom(Vector3 position){

        Vector3 targetPosition = new Vector3(position.x, position.y, defaultPosition.z);
        float targetSize = defaultSize * zoom;

        float t = 0f;

        while(t < zoomInTime){

            t += Time.deltaTime;
            float lerp = t / zoomInTime;
            camera.orthographicSize = Mathf.Lerp(defaultSize, targetSize, lerp);
            transform.position = Vector3.Lerp(defaultPosition, targetPosition, lerp);
            yield return null;

        }

        yield return new WaitForSeconds(zoomInTime * 2f);

        t = 0f;

        while(t < zoomOutTime){

            t += Time.deltaTime;
            float lerp = t / zoomOutTime;
            camera.orthographicSize = Mathf.Lerp(targetSize, defaultSize, lerp);
            transform.position = Vector3.Lerp(targetPosition, defaultPosition, lerp);
            yield return null;

        }

        camera.orthographicSize = defaultSize;
        transform.position = defaultPosition;

    }

}