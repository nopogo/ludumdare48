using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour {

    Camera mainCamera;
    Vector3 newHookPos;
    Vector3 mousePos;
    Rigidbody2D rigidbody2D;

    float zOffset;
    public float mouseForce = 1f;

    void Awake(){
        mainCamera = Camera.main;
        zOffset = -mainCamera.transform.position.z;
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update(){
        mousePos = Input.mousePosition;
        mousePos.z = zOffset;
        newHookPos = mainCamera.ScreenToWorldPoint(mousePos);
    }

    Vector3 direction;

    void FixedUpdate(){
        direction = newHookPos - transform.position;
        rigidbody2D.AddForce(direction * mouseForce);
    }
}
