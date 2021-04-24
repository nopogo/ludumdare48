
using UnityEngine;


public class Singleton<Instance> : MonoBehaviour where Instance : Singleton<Instance> {
    public static Instance instance;

    public virtual void Awake() {
        if(!instance) {
            instance = this as Instance;
        }  else {
            Debug.LogError("WE ARE DUPLICATE DUMMY: "+ this);
            Destroy(gameObject);
        }
    }
}
