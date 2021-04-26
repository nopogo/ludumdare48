
using System.Collections.Generic;

public class FloatVariable {
    public float startingAmount;
    public float currentAmount;
    public float currentUpgradeAmount;

    public FloatVariable(float init){
        startingAmount       = init;
        currentAmount        = init;
        currentUpgradeAmount = init;
    }

    public void Upgrade(float amount){
        currentUpgradeAmount += amount;
    }

    public void Reset(){
        currentAmount = currentUpgradeAmount;
    }
}

public class SpawnDepthDelay{
    public float minDepth;
    public float waitInSecondsMin;
    public float waitInSecondsMax;

    public SpawnDepthDelay(float depthValue, float waitValueMin, float waitValueMax){
        minDepth = depthValue;
        waitInSecondsMin = waitValueMin;
        waitInSecondsMax = waitValueMax;
    }
}

public static class Settings {
    public static FloatVariable oxygenPerSecond = new FloatVariable(1f);
    public static FloatVariable oxygen          = new FloatVariable(50f);
    public static FloatVariable hookSpeed       = new FloatVariable(100f);
    public static FloatVariable hookSize        = new FloatVariable(1f);
    public static FloatVariable ropeLength      = new FloatVariable(3f);
    public static FloatVariable subDrag         = new FloatVariable(10f);


    public static float startSkyboxMaterialExposure = 0.5f;
    public static float directionalLightStartingIntensity;
    public static float directionalLight2DStartingIntensity;

    public static float maxDepth = 500f;
    public static float noLightDepth = 100f;
    public static float yLevelUnderwaterStart = 1.2f;


    public static List<SpawnDepthDelay> spawnDepthDelayLookupList = new List<SpawnDepthDelay>(){
        new SpawnDepthDelay(20f, 4f, 8f),
        new SpawnDepthDelay(50f, 3f, 5f),
        new SpawnDepthDelay(80f, 1f, 3f),
        new SpawnDepthDelay(100f, .5f, 3f),
        new SpawnDepthDelay(200f, .2f, 2f),
        new SpawnDepthDelay(300f, .2f, 1f),
    };

    public static SpawnDepthDelay GetSpawnDepthDelay(float currentDepth){
        foreach(SpawnDepthDelay spawnDepthDelay in spawnDepthDelayLookupList){
            if(currentDepth < spawnDepthDelay.minDepth){
                return spawnDepthDelay;
            }
        }
        return spawnDepthDelayLookupList[0];
    }

    public static void Reset(){
        oxygenPerSecond.Reset();
        oxygen.Reset();
        hookSpeed.Reset();
        hookSize.Reset();
        ropeLength.Reset();
        subDrag.Reset();
    }
    
}
