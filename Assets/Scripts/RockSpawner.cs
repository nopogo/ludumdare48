using UnityEngine;

public class RockSpawner : MonoBehaviour {

    public Sprite startSprite;

    public Sprite[] rockVariationSprites;


    public GameObject rockFacePrefab;

    float nextDepthSpawn = 20f;
    bool firstSpawn = true;

    void Update(){
        if(GameLogic.instance.depth > nextDepthSpawn){
            SpawnRockFace();
            nextDepthSpawn += 20f;
        }
    }



    void SpawnRockFace(){
        // lastDepthSpawned = 
        SpriteRenderer tempSpriteRenderer = Instantiate(rockFacePrefab, transform).GetComponent<SpriteRenderer>();
        if(firstSpawn){
            tempSpriteRenderer.sprite = startSprite;
        }else{
            tempSpriteRenderer.sprite = rockVariationSprites[Random.Range(0, rockVariationSprites.Length)];
        }
        tempSpriteRenderer.transform.position = new Vector3(0f, nextDepthSpawn, 0f);
        firstSpawn = false;
        
    }
}
