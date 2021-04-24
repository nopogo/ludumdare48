using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Fish : ScriptableObject {
    public Sprite sprite;
    public float minDepth;
    public AnimationCurve depthChance = new AnimationCurve();
    public bool isNegative = false;
    public int value = 10;
    public float maxSpeed = 5f;
}
