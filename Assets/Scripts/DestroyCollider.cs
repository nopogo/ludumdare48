using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCollider : MonoBehaviour {
    
    void OnTriggerEnter2D(Collider2D other){
        if(other.transform.GetComponent<FishObject>() != null){
            Destroy(other.gameObject);
        }
    }
}
