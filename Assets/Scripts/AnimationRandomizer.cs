using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationRandomizer : MonoBehaviour {


    Transform[] animationGameObjects;


    void Awake(){
        animationGameObjects = transform.GetComponentsInChildren<Transform>(true);
        StartCoroutine(StartAnimationObject());
    }



    IEnumerator StartAnimationObject(){
        foreach(Transform tempTransform in animationGameObjects){
            tempTransform.gameObject.SetActive(true);
            yield return new WaitForSeconds(Random.Range(0f, 2f));
        }
    }
}
