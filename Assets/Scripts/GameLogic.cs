using System;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class GameLogic : Singleton<GameLogic>{
    
    public Rigidbody submarineRigidbody;
    public ParticleSystem underwaterParticles;
    public ParticleSystem underwaterUnlitParticles;

    public StudioEventEmitter splashEmitter;

    public GameObject ambiencePlayerBelowSurface;
    public GameObject ambiencePlayerAboveSurface;

    public GameObject[] surfaceOnlyObjects;


    public List<Fish> caughtFish = new List<Fish>(); 

    public Fish[] fishes;
    public Upgrade[] upgrades;


    public Action diveStarted;
    public Action diveEnded;

    public float depth  = 0f;
    
    public float maxDepth = 10000f;
    public int money = 0;
    float yLevelUnderwaterStart = 1.2f;
        

    public float percentageDown{
        get{
            return depth / maxDepth;
        }
    }

    Vector3 subStartPos;
    UILogic uiLogic;

    bool diveButtonPressed = false;
    bool didDiveStart = false;


    public override void Awake(){
        base.Awake();
        uiLogic = UILogic.instance;
        subStartPos = Sub.instance.transform.position;
        ambiencePlayerAboveSurface.SetActive(true);
    }

    void Start(){
        fishes = Resources.LoadAll<Fish>("ScriptableObjects/Fish");
        upgrades = Resources.LoadAll<Upgrade>("ScriptableObjects/Upgrades");
        UILogic.instance.FillUpgrades();
        diveEnded += OnDiveStopped;
        diveStarted += OnDiveStarted;
    }

    void OnDisable(){
        diveEnded -= OnDiveStopped;
        diveStarted -= OnDiveStarted;
    }

    void Update(){
        depth = -submarineRigidbody.transform.position.y;
        
        if (depth < yLevelUnderwaterStart){
            return;
        }
        if(diveButtonPressed){
            if(didDiveStart == false){
                didDiveStart = true;
                diveButtonPressed = false;
                diveStarted?.Invoke();
            }
        }
        if(Input.GetKeyDown(KeyCode.Space) && didDiveStart){
            diveEnded?.Invoke();
        }
        UseOxygen();
        UpdateUI();
    }

    void UseOxygen(){
        Settings.oxygen.currentAmount -= Settings.oxygenPerSecond.currentAmount * Time.deltaTime;
        if(Settings.oxygen.currentAmount <= 0f){
            Die();

        }
    }

    void Die(){
        caughtFish = new List<Fish>();
        FmodAudioTriggerManager.instance.PlayNegativeSound();
        diveEnded?.Invoke();
    }


    public void TryToBuyUpgrade(UpgradeButton upgradeButton){
        if(upgradeButton.linkedUpgrade.cost <= money){
            money -= upgradeButton.linkedUpgrade.cost;
            FmodAudioTriggerManager.instance.PlayCashSound();
            UpdateMoneyUI();
            ApplyUpgrade(upgradeButton.linkedUpgrade);
            upgradeButton.UpgradeBought();
        }
    }




    void ApplyUpgrade(Upgrade upgrade){
        switch(upgrade.upgradeType){
            case UpgradeType.Weight:
                Settings.subDrag.currentUpgradeAmount += upgrade.valueChange;
                break;
            case UpgradeType.HookSize:
                Settings.hookSize.currentUpgradeAmount += upgrade.valueChange;
                break;
            case UpgradeType.OxygenTank:
                Settings.oxygen.currentUpgradeAmount += upgrade.valueChange;
                break;
            case UpgradeType.RopeLength:
                Settings.ropeLength.currentUpgradeAmount += upgrade.valueChange;
                break;
            case UpgradeType.HookSpeed:
                Settings.hookSpeed.currentUpgradeAmount += upgrade.valueChange;
                break;
        }
    }



    public void TriggerStartDive(){
        submarineRigidbody.isKinematic = false;        
        diveButtonPressed = true;        
    }

    void OnDiveStarted(){
        Settings.Reset();
        underwaterParticles.Play();
        underwaterUnlitParticles.Play();
        ambiencePlayerBelowSurface.SetActive(true);
        ambiencePlayerAboveSurface.SetActive(false);
        foreach(GameObject go in surfaceOnlyObjects){
            go.SetActive(false);
        }
    }


    void OnDiveStopped(){
        didDiveStart = false;
        depth = 0f;
        submarineRigidbody.isKinematic = true;
        submarineRigidbody.transform.position = subStartPos;
        underwaterParticles.Stop();
        underwaterUnlitParticles.Stop();
        underwaterUnlitParticles.Clear();
        ambiencePlayerBelowSurface.SetActive(false);
        ambiencePlayerAboveSurface.SetActive(true);
        foreach(GameObject go in surfaceOnlyObjects){
            go.SetActive(true);
        }
        CalculateProfit();
    }

    void CalculateProfit(){
        foreach(Fish fish in caughtFish){
            money += fish.value;
        }

        caughtFish = new List<Fish>();
        UpdateMoneyUI();
    }

    void UpdateMoneyUI(){
        UILogic.instance.moneyText.text = $"{money} $"; 
    }

    void UpdateUI(){
        uiLogic.depthText.text = $"{Mathf.Round(depth)} m";
        uiLogic.oxygenText.text = $"{Mathf.Round(Settings.oxygen.currentAmount * 10.0f)*0.1f} O2";
    }
}
