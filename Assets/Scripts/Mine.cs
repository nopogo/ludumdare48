using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour {


    void OnTriggerEnter2D(Collider2D other){

        Hook hook = other.transform.GetComponent<Hook>();
        if(hook != null){
            GameLogic.instance.Bomb();
        }
    }
}
