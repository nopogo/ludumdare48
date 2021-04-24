using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishObject : MonoBehaviour {

    public Fish fishType;

    float xDirection;

    SpriteRenderer spriteRenderer;

    bool fishFinishedInitiation = false;

    public void Initiate(Fish fishTypeVar){
        fishType = fishTypeVar;
        spriteRenderer = GetComponent<SpriteRenderer>();
        xDirection = Random.Range(-1f*fishType.maxSpeed, 1f*fishType.maxSpeed);
        fishFinishedInitiation = true;
    }

    void Update(){
        if(fishFinishedInitiation == false){
            return;
        }
        CheckEdgeScreen();
        Movement();
        CheckFlip();
    }

    float timeLastFlip = 0f;
    float minDelay = 5f;

    void CheckEdgeScreen(){
        if(timeLastFlip == 0f || timeLastFlip + minDelay < Time.time ){
            if(transform.position.x > 8f || transform.position.x < -8f){
                xDirection *= -1;
                timeLastFlip = Time.time;
            }
        }
    }

    Vector3 movementVector;
    void Movement(){
        movementVector = transform.position;
        movementVector.x += xDirection * Time.deltaTime;
        transform.position = movementVector;
    }

    void CheckFlip(){
        spriteRenderer.flipX  = xDirection > 0;
    }



    void OnTriggerEnter2D(Collider2D other){

        Hook hook = other.transform.GetComponent<Hook>();
        if(hook != null){
            GameLogic.instance.caughtFish.Add(fishType);
            Destroy(gameObject);
        }
    }
}
