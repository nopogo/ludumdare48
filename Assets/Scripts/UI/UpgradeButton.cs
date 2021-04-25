using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour {


    public TMP_Text upgradeTitle;
    public TMP_Text cost;
    public Image spriteRenderer;

    public Upgrade linkedUpgrade;

    Button button;

    int numberOfPurchases;

    void Awake(){
        button = GetComponent<Button>();
    }

    public void Initiate(Upgrade upgrade){
        spriteRenderer.sprite = upgrade.sprite;
        upgradeTitle.text = $"{upgrade.verboseName} +{Mathf.Abs(upgrade.valueChange)}";
        cost.text = $"{upgrade.cost} $";
        linkedUpgrade = upgrade;
    }



    public void ButtonPressed(){
        GameLogic.instance.TryToBuyUpgrade(this);
    }

    public void UpgradeBought(){
        numberOfPurchases +=1;
        if(numberOfPurchases >= linkedUpgrade.maxNumberPurchase){
            button.interactable = false;
        }
    }
}
