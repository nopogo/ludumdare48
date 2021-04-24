using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : Singleton<Hook> {

    Camera mainCamera;
    Vector3 newHookPos;
    Vector3 mousePos;
    Rigidbody2D rigidbody2D;

    public bool dormant = true;


    float zOffset;
    public float mouseForce = 1f;

    public override void Awake(){
        base.Awake();
        mainCamera = Camera.main;
        zOffset = -mainCamera.transform.position.z;
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update(){
        if(dormant){
            return;
        }
        mousePos = Input.mousePosition;
        mousePos.z = zOffset;
        newHookPos = mainCamera.ScreenToWorldPoint(mousePos);
    }

    Vector3 direction;

    void FixedUpdate(){
        if(dormant){
            return;
        }
        direction = newHookPos - transform.position;
        rigidbody2D.AddForce(direction * mouseForce);
    }
}
