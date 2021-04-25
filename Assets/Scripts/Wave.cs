using UnityEngine;

public class Wave : MonoBehaviour {

    float minSpeed = 2f;
    float maxSpeed = 5f;

    float speed = 0f;
    float bobStrength = .2f;
    float bobSpeed = 3f;

    float maxLifeTime = 60f;

    float timePassed;


    void Awake(){
        speed = Random.Range(minSpeed, maxSpeed);
    }




    void Update () {
        Vector3 newPos = transform.position + transform.parent.transform.forward * speed * Time.deltaTime;

        newPos.y = Mathf.Sin(Time.time * bobSpeed)*bobStrength;
        transform.position = newPos ;

        timePassed += Time.deltaTime;
        if(timePassed > maxLifeTime){
            Destroy(gameObject);
        }
    }
    
}
