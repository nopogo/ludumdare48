using System;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : Singleton<GameLogic>{
    
    public Rigidbody submarineRigidbody;
    public ParticleSystem underwaterParticles;


    public List<Fish> caughtFish = new List<Fish>(); 

    public Fish[] fishes;
    public Upgrade[] upgrades;


    public Action diveStarted;
    public Action diveEnded;

    public float depth  = 0f;
    public float oxygen = 100f;    
    public float maxDepth = 10000f;
    public int money = 0;

    public float oxygenPerSecond = 1f; 

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
    }

    void Start(){
        fishes = Resources.LoadAll<Fish>("ScriptableObjects/Fish");
        upgrades = Resources.LoadAll<Upgrade>("ScriptableObjects/Upgrades");
        UILogic.instance.FillUpgrades();
        diveEnded += OnDiveStopped;
    }

    void OnDisable(){
        diveEnded -= OnDiveStopped;
    }

    void Update(){
        depth = -submarineRigidbody.transform.position.y;
        if (depth < 0f){
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
        oxygen -= oxygenPerSecond * Time.deltaTime;
        if(oxygen <= 0f){
            Die();

        }
    }

    void Die(){
        caughtFish = new List<Fish>();
        diveEnded?.Invoke();
    }


    public void TryToBuyUpgrade(UpgradeButton upgradeButton){
        if(upgradeButton.linkedUpgrade.cost < money){
            money -= upgradeButton.linkedUpgrade.cost;
            ApplyUpgrade(upgradeButton.linkedUpgrade);
            upgradeButton.UpgradeBought();
        }
    }


// TODO this is where we left off WIP
    void ApplyUpgrade(Upgrade upgrade){
        switch(upgrade.upgradeType){
            case UpgradeType.Weight:
                break;
            case UpgradeType.HookSize:
                break;
            case UpgradeType.OxygenTank:
                break;
            case UpgradeType.RopeLength:
                break;
        }
    }



    public void TriggerStartDive(){
        submarineRigidbody.isKinematic = false;        
        diveButtonPressed = true;        
    }

    void StartDive(){
        diveStarted?.Invoke();
        underwaterParticles.Play();
    }


    void OnDiveStopped(){
        didDiveStart = false;
        depth = 0f;
        oxygen = 100f;
        submarineRigidbody.isKinematic = true;
        submarineRigidbody.transform.position = subStartPos;
        underwaterParticles.Stop();
        CalculateProfit();
    }

    void CalculateProfit(){
        foreach(Fish fish in caughtFish){
            money += fish.value;
        }

        caughtFish = new List<Fish>();
        UILogic.instance.moneyText.text = $"{money} $"; 
    }

    void UpdateUI(){
        uiLogic.depthText.text = $"{Mathf.Round(depth)} m";
        uiLogic.oxygenText.text = $"{Mathf.Round(oxygen *10.0f)*0.1f} O2";
    }
}
