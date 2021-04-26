using UnityEngine;

public class RockSpawner : MonoBehaviour {

    public Sprite startSprite;
    public Sprite startTopSprite;
    public Sprite startBottomSprite;

    public Sprite[] rockVariationSprites;
    public Sprite[] top;
    public Sprite[] bottom;


    public GameObject rockFacePrefab;



    float nextDepthSpawn = 30f;
    bool firstSpawn = true;

    float rockSpawnDistance = 14f;
    float rockOutofScreenDistance = 30f;
 
    void Update(){
        if(GameLogic.instance.depth + rockOutofScreenDistance > nextDepthSpawn){
            SpawnRockFace();
            nextDepthSpawn += rockSpawnDistance;
        }
    }



    void SpawnRockFace(){

        // lastDepthSpawned = 
        SpriteRenderer[] spriteRendererArray = Instantiate(rockFacePrefab, transform).GetComponentsInChildren<SpriteRenderer>();
        if(firstSpawn){
            
            spriteRendererArray[0].sprite = startSprite;
            spriteRendererArray[1].sprite = startTopSprite;
            spriteRendererArray[2].sprite = startBottomSprite;
        }else{
            int index = Random.Range(0, rockVariationSprites.Length);
            spriteRendererArray[0].sprite = rockVariationSprites[index];
            spriteRendererArray[1].sprite = top[index];
            spriteRendererArray[2].sprite = bottom[index];
        }
        spriteRendererArray[0].transform.position = new Vector3(0f, -nextDepthSpawn, 0f);
        firstSpawn = false;
        
    }
}
