
using UnityEngine;
using TMPro;

public class UILogic : Singleton<UILogic> {

    public Transform surfaceCanvas;
    public Transform diveCanvas;
    public Transform upgradeGridParent;
    public GameObject upgradeUIPrefab;

    public GameObject upgradePanel;

    public TMP_Text depthText;
    public TMP_Text oxygenText;
    public TMP_Text moneyText;

    public Canvas scoreCanvas;

    public TMP_Text depthValue;
    public TMP_Text fishValue;
    public TMP_Text moneyValue;
    public TMP_Text upgradeValue;
    public TMP_Text diveValue;
    public TMP_Text failedValue;



    public override void Awake(){
        base.Awake();
        surfaceCanvas.gameObject.SetActive(true);
        diveCanvas.gameObject.SetActive(false);
        scoreCanvas.gameObject.SetActive(false);
    }

    void Start (){
        GameLogic.instance.diveStarted += OnStartDive;
        GameLogic.instance.diveEnded   += OnEndDive;
    }

    public void OnDisable(){
        GameLogic.instance.diveStarted -= OnStartDive;
        GameLogic.instance.diveEnded   -= OnEndDive;
    }

    public void FillUpgrades(){
        foreach(Upgrade upgrade in GameLogic.instance.upgrades){
            UpgradeButton upgradeButton = Instantiate(upgradeUIPrefab, upgradeGridParent).GetComponent<UpgradeButton>();
            upgradeButton.Initiate(upgrade);
        }
    }


    public void PressQuit(){
        depthValue.text   = Settings.depthValue.ToString();
        fishValue.text    = Settings.fishValue.ToString();
        moneyValue.text   = Settings.moneyValue.ToString();
        upgradeValue.text = Settings.upgradeValue.ToString();
        diveValue.text    = Settings.diveValue.ToString();
        failedValue.text  = Settings.failedValue.ToString();

        scoreCanvas.gameObject.SetActive(true);
        surfaceCanvas.gameObject.SetActive(false);
        upgradePanel.SetActive(false);
    }

    public void CloseGame(){
        Application.Quit();
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
        upgradePanel.SetActive(true);
    }
}
