using System;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.Experimental.Rendering.Universal;

public class GameLogic : Singleton<GameLogic>{

    const string ExposureVar = "_Exposure";
    
    public Rigidbody submarineRigidbody;
    public ParticleSystem underwaterParticles;
    public ParticleSystem underwaterUnlitParticles;

    public Light directionalLight;
    public Light2D directionalLight2D;
    public Material skyboxMaterial;

    public Light[] spotLights;
    public Light2D[] spotLights2D;

    public StudioEventEmitter splashEmitter;

    public GameObject ambiencePlayerBelowSurface;
    public GameObject ambiencePlayerAboveSurface;

    public Transform surfaceOnlyObjectParent;
    public Transform belowOnlyObjectParent;

    public GameObject topRope;


    public List<Fish> caughtFish = new List<Fish>(); 

    public Fish[] fishes;
    public Upgrade[] upgrades;


    public Action diveStarted;
    public Action diveEnded;

    public float depth  = 0f;
    
    

    public int money = 0;
    
        
    [SerializeField]
    public float percentageDown{
        get{
            return depth / Settings.maxDepth;
        }
    }

    Vector3 subStartPos;
    UILogic uiLogic;

    bool diveButtonPressed = false;
    bool didDiveStart = false;




    public override void Awake(){
        base.Awake();
        Settings.directionalLight2DStartingIntensity = directionalLight2D.intensity;
        Settings.directionalLightStartingIntensity = directionalLight.intensity;
        skyboxMaterial.SetFloat(ExposureVar, Settings.startSkyboxMaterialExposure);
        uiLogic = UILogic.instance;
        subStartPos = Sub.instance.transform.position;
        ambiencePlayerAboveSurface.SetActive(true);
        foreach(Light light in spotLights){
            light.enabled=false;
        }
        foreach(Light2D light in spotLights2D){
            light.enabled=false;
        }
    }

    void Start(){
        fishes = Resources.LoadAll<Fish>("ScriptableObjects/Fish");
        upgrades = Resources.LoadAll<Upgrade>("ScriptableObjects/Upgrades");
        UILogic.instance.FillUpgrades();
        diveEnded += OnDiveStopped;
        diveStarted += OnDiveStarted;

        foreach(Transform t in belowOnlyObjectParent.GetComponentsInChildren<Transform>(true)){
            t.gameObject.SetActive(false);
        }
    }

    void OnDisable(){
        diveEnded -= OnDiveStopped;
        diveStarted -= OnDiveStarted;
    }

    void Update(){
        depth = -submarineRigidbody.transform.position.y;
        
        if (depth < Settings.yLevelUnderwaterStart){
            return;
        }

        //Underwater logic

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
        LightingEffects();
        UpdateUI();
    }

    void UseOxygen(){
        Settings.oxygen.currentAmount -= Settings.oxygenPerSecond.currentAmount * Time.deltaTime;
        if(Settings.oxygen.currentAmount <= 0f){
            Die();

        }
    }

    void LightingEffects(){
        directionalLight.intensity = Mathf.Lerp(Settings.directionalLightStartingIntensity, 0f, depth/Settings.noLightDepth);
        directionalLight2D.intensity = Mathf.Lerp(Settings.directionalLight2DStartingIntensity, 0f, depth/Settings.noLightDepth);
        skyboxMaterial.SetFloat(ExposureVar, Mathf.Lerp(Settings.startSkyboxMaterialExposure, 0f, depth/Settings.noLightDepth));
    }

    void ResetLightingEffects(){
        directionalLight.intensity = Settings.directionalLightStartingIntensity;
        directionalLight2D.intensity = Settings.directionalLight2DStartingIntensity;
        skyboxMaterial.SetFloat(ExposureVar, Settings.startSkyboxMaterialExposure);
    }

    public void Bomb(){
        Debug.Log("boom");
        FmodAudioTriggerManager.instance.PlayBombSound();
        // yield return new WaitForSeconds(FmodAudioTriggerManager.instance.bombSoundInstance.GEtLe)
        Die();
    }

    void Die(){
        Settings.failedValue +=1;
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
        }else{
            FmodAudioTriggerManager.instance.PlayNegativeSound();
        }
    }




    void ApplyUpgrade(Upgrade upgrade){
        Settings.upgradeValue +=1;
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
        topRope.SetActive(false);
    }

    void OnDiveStarted(){
        Settings.diveValue +=1;
        Settings.Reset();
        underwaterParticles.Play();
        underwaterUnlitParticles.Play();
        ambiencePlayerBelowSurface.SetActive(true);
        ambiencePlayerAboveSurface.SetActive(false);
        foreach(Transform t in surfaceOnlyObjectParent.GetComponentsInChildren<Transform>(true)){
            t.gameObject.SetActive(false);
        }
        foreach(Transform t in belowOnlyObjectParent.GetComponentsInChildren<Transform>(true)){
            t.gameObject.SetActive(true);
        }
        foreach(Light light in spotLights){
            light.enabled=true;
        }
        foreach(Light2D light in spotLights2D){
            light.enabled=true;
        }
    }


    void OnDiveStopped(){
        ResetLightingEffects();
        didDiveStart = false;
        Settings.depthValue += Mathf.FloorToInt(depth);
        depth = 0f;
        submarineRigidbody.isKinematic = true;
        submarineRigidbody.transform.position = subStartPos;
        underwaterParticles.Stop();
        underwaterUnlitParticles.Stop();
        underwaterUnlitParticles.Clear();
        ambiencePlayerBelowSurface.SetActive(false);
        ambiencePlayerAboveSurface.SetActive(true);
        foreach(Transform t in surfaceOnlyObjectParent.GetComponentsInChildren<Transform>(true)){
            t.gameObject.SetActive(true);
        }
        foreach(Transform t in belowOnlyObjectParent.GetComponentsInChildren<Transform>(true)){
            t.gameObject.SetActive(false);
        }
        foreach(Light light in spotLights){
            light.enabled=false;
        }
        foreach(Light2D light in spotLights2D){
            light.enabled=false;
        }
        topRope.SetActive(true);
        CalculateProfit();
    }

    void CalculateProfit(){
        foreach(Fish fish in caughtFish){
            Settings.fishValue  += 1;
            Settings.moneyValue += fish.value;
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
