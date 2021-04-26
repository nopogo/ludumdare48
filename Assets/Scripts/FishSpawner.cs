using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour {

    

    public GameObject fishPrefab;
    public GameObject bomb;
    public BoxCollider2D spawnPlane;
    List<GameObject> spawnedFishList = new List<GameObject>();
    List<GameObject> spawnedBombs = new List<GameObject>();
    float startWaitUntillNextFish;

    float timeLastBombSpawned;


    void Start() {
        timeLastBombSpawned = Time.time;
        GameLogic.instance.diveStarted += OnDiveStarted;
        GameLogic.instance.diveEnded += OnDiveEnded;
    }

    void OnDissable() {
        StopCoroutine(SpawnFish());
        GameLogic.instance.diveStarted -= OnDiveStarted;
        GameLogic.instance.diveEnded -= OnDiveEnded;
    }



    void OnDiveStarted(){
        StartCoroutine(SpawnFish());
    }

    void OnDiveEnded(){
        StopCoroutine(SpawnFish());
        DeleteOldFish();
    }


    void DeleteOldFish(){
        for (int i = spawnedFishList.Count - 1; i >= 0 ; i--){
            GameObject temp = spawnedFishList[i];
            spawnedFishList.RemoveAt(i);
            Destroy(temp);
        }
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
            if(GameLogic.instance.depth > fish.minDepth && (fish.maxDepth == 0 || GameLogic.instance.depth < fish.maxDepth)){
                for (int i = 0; i < fish.spawnWeight; i++){
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
        SpawnDepthDelay depthDelay = Settings.GetSpawnDepthDelay(GameLogic.instance.depth);
        startWaitUntillNextFish = Random.Range(depthDelay.waitInSecondsMin, depthDelay.waitInSecondsMax);

        yield return new WaitForSeconds(startWaitUntillNextFish);
        Fish fishToSpawn = GetFish();
        if(Settings.bombDelay < Time.time - timeLastBombSpawned){
           spawnedBombs.Add(Instantiate(bomb, GetSpawnPosition(), Quaternion.Euler(new Vector3(-90f, 0f, 0f))));
           timeLastBombSpawned = Time.time;
        }else{
            if(fishToSpawn != null){
                GameObject spawnedFish = Instantiate(fishPrefab, GetSpawnPosition(), Quaternion.identity);
                spawnedFishList.Add(spawnedFish);
                spawnedFish.GetComponent<SpriteRenderer>().sprite = fishToSpawn.sprite;
                BoxCollider2D boxColl = spawnedFish.AddComponent<BoxCollider2D>();
                boxColl.size = new Vector2(1,1);
                boxColl.isTrigger = true;
                spawnedFish.GetComponent<FishObject>().Initiate(fishToSpawn);
            }
        }
        
        if(GameLogic.instance.depth > 0f){
            StartCoroutine(SpawnFish());
        }
    }

    
}
