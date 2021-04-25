using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : Singleton<Hook> {

    public Camera Camera2D;
    Vector3 newHookPos;
    Vector3 mousePos;
    Rigidbody2D rigidbody2D;
    SpriteRenderer spriteRenderer;
    public LineRenderer ropeLineRenderer;
    public Transform ropeAttachment;

    Vector3 startPos;

    public bool dormant = true;

    float zOffset;
    // public float mouseForce = 1f;
    float rotSpeed = 50f;

    public override void Awake(){
        base.Awake();
        // mainCamera = Camera.main;
        zOffset = -Camera2D.transform.position.z;
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        ropeLineRenderer.enabled = false;
        startPos = transform.position;
    }

    void Start(){
        GameLogic.instance.diveStarted += OnStartedDiving;
        GameLogic.instance.diveEnded   += OnStoppedDiving;
    }

    void OnDisable(){
        GameLogic.instance.diveStarted -= OnStartedDiving;
        GameLogic.instance.diveEnded   -= OnStoppedDiving;
    }

    void OnStartedDiving(){
        spriteRenderer.enabled = true;
        ropeLineRenderer.enabled = true;
        dormant = false;
        transform.localScale = new Vector3(Settings.hookSize.currentAmount,Settings.hookSize.currentAmount, Settings.hookSize.currentAmount);
    }

    void OnStoppedDiving(){
        spriteRenderer.enabled = false;
        ropeLineRenderer.enabled = false;
        dormant = true;
        transform.position = startPos;
        newHookPos = ropeAttachment.position;
        rigidbody2D.velocity = Vector2.zero;
    }

    void Update(){
        if(dormant){
            return;
        }
    }

    Vector3 direction;

    public float correctionAmount = 0.01f;

    void FixedUpdate(){
        if(dormant){
            return;
        }

        mousePos = Input.mousePosition;
        mousePos.z = zOffset;
        newHookPos = Camera2D.ScreenToWorldPoint(mousePos);
        float currentDistance = Vector3.Distance(transform.position, ropeAttachment.position);
        if(currentDistance > Settings.ropeLength.currentAmount){
            Vector3 maxHookPos = transform.position + (Vector3.Normalize( ropeAttachment.position - transform.position)*correctionAmount);
            if(maxHookPos.y > ropeAttachment.position.y){
                maxHookPos.y = ropeAttachment.position.y;
            }
            rigidbody2D.MovePosition(maxHookPos);
            rigidbody2D.velocity = Vector2.zero;
        }else{
            direction = Vector3.Normalize(newHookPos - transform.position);
            rigidbody2D.AddForce(direction * Settings.hookSpeed.currentAmount * Time.fixedDeltaTime);
        }
    
        VisualLogic();
    }
    

    void VisualLogic(){
        float heading = Mathf.Atan2(-rigidbody2D.velocity.x, -rigidbody2D.velocity.y);
        transform.rotation *= Quaternion.Euler(0, 0, heading*Time.fixedDeltaTime * rotSpeed);
    }


    
}
