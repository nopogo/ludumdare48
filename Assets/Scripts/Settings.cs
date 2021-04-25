
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

public static class Settings {
    public static FloatVariable oxygenPerSecond = new FloatVariable(1f);
    public static FloatVariable oxygen          = new FloatVariable(50f);
    public static FloatVariable subWeight       = new FloatVariable(1f);
    public static FloatVariable hookSpeed       = new FloatVariable(100f);
    public static FloatVariable hookSize        = new FloatVariable(1f);
    public static FloatVariable ropeLength      = new FloatVariable(3f);

    public static void Reset(){
        oxygenPerSecond.Reset();
        oxygen.Reset();
        subWeight.Reset();
        hookSpeed.Reset();
        hookSize.Reset();
        ropeLength.Reset();
    }
    
}
