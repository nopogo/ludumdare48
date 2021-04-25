using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sub : Singleton<Sub>{
    Rigidbody rigidbody;

	public override void Awake() {
		base.Awake();
		rigidbody = GetComponent<Rigidbody>();
	}



	void Start(){
		GameLogic.instance.diveStarted += OnDiveStarted;
	}

	void OnDisable(){
		GameLogic.instance.diveStarted -= OnDiveStarted;
	}



	void OnDiveStarted(){
		rigidbody.drag = Settings.subDrag.currentAmount;
	}
}
