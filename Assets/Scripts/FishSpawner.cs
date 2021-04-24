using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour {

    

    public GameObject fishPrefab;

    public BoxCollider2D spawnPlane;


    float waitTillNextFish = 1f;

    void Start() {
        GameLogic.instance.diveStarted += OnDiveStarted;
        GameLogic.instance.diveEnded += OnDiveEnded;
    }

    void OnDissable() {
        GameLogic.instance.diveStarted -= OnDiveStarted;
        GameLogic.instance.diveEnded -= OnDiveEnded;
    }



    void OnDiveStarted(){
        StartCoroutine(SpawnFish());
    }

    void OnDiveEnded(){
        StopCoroutine(SpawnFish());
    }

    Vector3 GetSpawnPosition(){
        return new Vector3(
            Random.Range(spawnPlane.bounds.min.x, spawnPlane.bounds.max.x),
            Random.Range(spawnPlane.bounds.min.y, spawnPlane.bounds.max.y),
            Random.Range(spawnPlane.bounds.min.z, spawnPlane.bounds.max.z)
        );
    }

    Fish GetFish(){
        List<Fish> tempFish = new List<Fish>();
        foreach(Fish fish in GameLogic.instance.fishes){
            if(GameLogic.instance.depth > fish.minDepth){
                int totalChance = Mathf.FloorToInt(10f * fish.depthChance.Evaluate(GameLogic.instance.percentageDown));
                for (int i = 0; i < totalChance; i++){
                    tempFish.Add(fish);
                }
            }
        }
        if(tempFish.Count == 0){
            return null;
        }
        return tempFish[Random.Range(0, tempFish.Count)];
    }

    IEnumerator SpawnFish(){
        yield return new WaitForSeconds(waitTillNextFish);
        Fish fishToSpawn = GetFish();
        if(fishToSpawn != null){
            GameObject spawnedFish = Instantiate(fishPrefab, GetSpawnPosition(), Quaternion.identity);
            spawnedFish.GetComponent<SpriteRenderer>().sprite = fishToSpawn.sprite;
            BoxCollider2D boxColl = spawnedFish.AddComponent<BoxCollider2D>();
            boxColl.size = new Vector2(1,1);
            boxColl.isTrigger = true;
            spawnedFish.GetComponent<FishObject>().Initiate(fishToSpawn);
        }
        
        StartCoroutine(SpawnFish());
    }

    
}
