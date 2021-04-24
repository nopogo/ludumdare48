using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILogic : MonoBehaviour {

    public Transform surfaceCanvas;
    public Transform diveCanvas;


    void Awake(){
        surfaceCanvas.gameObject.SetActive(true);
        diveCanvas.gameObject.SetActive(false);
    }



    public void PressDive(){
        GameLogic.instance.StartDive();
        surfaceCanvas.gameObject.SetActive(false);
        diveCanvas.gameObject.SetActive(true);
    }


    public void GoUp(){
        GameLogic.instance.GoUp();
        surfaceCanvas.gameObject.SetActive(true);
        diveCanvas.gameObject.SetActive(false);
    }
}
