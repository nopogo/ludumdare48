using UnityEngine;

public class CameraLogic : MonoBehaviour {

    Quaternion startQuaterion;
    Vector3 startPosition;

    void Start() {
        startQuaterion = transform.rotation;
        startPosition = transform.position;
        GameLogic.instance.diveStarted += OnDiveStarted;
        GameLogic.instance.diveEnded += OnDiveEnded;
    }

    void OnDissable() {
        GameLogic.instance.diveStarted -= OnDiveStarted;
        GameLogic.instance.diveEnded -= OnDiveEnded;
    }

    void OnDiveStarted(){
        transform.localPosition = new Vector3(0f, -2f, -8f);
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    void OnDiveEnded(){
        transform.position = startPosition;
        transform.rotation = startQuaterion;

    }


}
