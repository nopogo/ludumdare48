
using UnityEngine;
using TMPro;

public class UILogic : Singleton<UILogic> {

    public Transform surfaceCanvas;
    public Transform diveCanvas;

    public TMP_Text depthText;
    public TMP_Text oxygenText;
    public TMP_Text moneyText;


    public override void Awake(){
        base.Awake();
        surfaceCanvas.gameObject.SetActive(true);
        diveCanvas.gameObject.SetActive(false);
    }

    void Start (){
        GameLogic.instance.diveStarted += OnStartDive;
        GameLogic.instance.diveEnded   += OnEndDive;
    }

    public void OnDisable(){
        GameLogic.instance.diveStarted -= OnStartDive;
        GameLogic.instance.diveEnded   -= OnEndDive;
    }



    public void PressDive(){
        GameLogic.instance.TriggerStartDive();
        surfaceCanvas.gameObject.SetActive(false);
        
    }

    public void OnStartDive(){
        diveCanvas.gameObject.SetActive(true);
    }


    void OnEndDive(){
        surfaceCanvas.gameObject.SetActive(true);
        diveCanvas.gameObject.SetActive(false);
    }
}
