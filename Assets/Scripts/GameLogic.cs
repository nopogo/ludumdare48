using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : Singleton<GameLogic>{
    
    public Rigidbody submarineRigidbody;
    public ParticleSystem underwaterParticles;

    

    public void StartDive(){
        Hook.instance.dormant = false;
        submarineRigidbody.isKinematic = false;
        underwaterParticles.Play();
    }


    public void GoUp(){
        underwaterParticles.Stop();
    }
}
