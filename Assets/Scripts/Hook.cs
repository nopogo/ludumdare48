using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : Singleton<Hook> {

    Camera mainCamera;
    Vector3 newHookPos;
    Vector3 mousePos;
    Rigidbody2D rigidbody2D;
    SpriteRenderer spriteRenderer;
    public LineRenderer ropeLineRenderer;

    Vector3 startPos;

    public bool dormant = true;

    float zOffset;
    public float mouseForce = 1f;

    public override void Awake(){
        base.Awake();
        mainCamera = Camera.main;
        zOffset = -mainCamera.transform.position.z;
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        ropeLineRenderer.enabled = false;
        startPos = transform.position;
    }

    void Start(){
        GameLogic.instance.diveStarted += OnStartedDiving;
        GameLogic.instance.diveEnded += OnStoppedDiving;
    }

    void OnDisable(){
        GameLogic.instance.diveStarted -= OnStartedDiving;
        GameLogic.instance.diveEnded -= OnStoppedDiving;
    }

    void OnStartedDiving(){
        spriteRenderer.enabled = true;
        ropeLineRenderer.enabled = true;
        dormant = false;
    }

    void OnStoppedDiving(){
        spriteRenderer.enabled = false;
        ropeLineRenderer.enabled = false;
        dormant = true;
        transform.position = startPos;
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
