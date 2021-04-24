using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour {


    public TMP_Text upgradeTitle;
    public TMP_Text cost;
    public Image spriteRenderer;

    public Upgrade linkedUpgrade;

    Button button;

    void Awake(){
        button = GetComponent<Button>();
    }

    public void Initiate(Upgrade upgrade){
        spriteRenderer.sprite = upgrade.sprite;
        upgradeTitle.text = upgrade.verboseName;
        cost.text = $"{upgrade.cost} $";
        linkedUpgrade = upgrade;
    }



    public void ButtonPressed(){
        GameLogic.instance.TryToBuyUpgrade(this);
    }

    public void UpgradeBought(){
        button.interactable = false;
    }
}
