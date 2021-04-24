using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum UpgradeType {Weight, OxygenTank, HookSize, RopeLength};

[CreateAssetMenu]
public class Upgrade : ScriptableObject {
    public string verboseName;
    public Sprite sprite;
    public int cost;
    public float valueChange;
    public UpgradeType upgradeType;
}
