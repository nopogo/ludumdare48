using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWaveMovement : MonoBehaviour {


    float yBobStrength = .05f;
    float yBobSpeed = 3f;

    float xBobSpeed = 3.5f;
    float xBobStrength = 4f;

    float startY;

    void Awake(){
        startY = transform.position.y;
    }




    void Update () {
        Vector3 newPos = transform.position;
        Vector3 swayRotation = transform.rotation.eulerAngles;

        newPos.y = startY + Mathf.Sin(Time.time * yBobSpeed)*yBobStrength;
        swayRotation.z = Mathf.Sin(Time.time * xBobSpeed)*xBobStrength;
        transform.position = newPos;
        transform.rotation = Quaternion.Euler(swayRotation);
       
    }
}
