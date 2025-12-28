using UnityEngine;

public class UIController : MonoBehaviour{

    public GameObject[] menus;

    public static UIController instance;

    void Awake(){

        if(instance != null && instance != this){

            Destroy(gameObject);

        }else{

            instance = this;

        }

    }

    public void HideAll(){

        foreach(GameObject g in menus){

            g.SetActive(false);

        }

    }

    public void HideSelected(int index){

        menus[index].SetActive(false);

    }

    public void ShowSelected(int index){

        menus[index].SetActive(true);

    }

}