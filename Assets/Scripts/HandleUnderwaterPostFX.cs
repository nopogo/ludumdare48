using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HandleUnderwaterPostFX : MonoBehaviour {


    Vector3 startPos;

    Transform mainCameraTransform;

    Transform originalParent;

    Volume volume;

    void Awake(){
        startPos = transform.position;
        mainCameraTransform = Camera.main.transform;
        originalParent = transform.parent;
        volume = GetComponent<Volume>();
    }

    void Update(){
        if(mainCameraTransform.position.y < 0){
            transform.parent = mainCameraTransform;
        }else{
            transform.parent = originalParent;
        }
    }
}
