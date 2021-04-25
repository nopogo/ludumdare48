using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
public class WaveSpawner : MonoBehaviour {

    public Sprite[] waveSprites;
    public GameObject wavePrefab;

    BoxCollider boundingCollider;

    float minTimeBetweenWave = 1f;
    float maxTimeBetweenWave = 5f;

    public bool flipTheX = false;

    void Awake(){
        boundingCollider = GetComponent<BoxCollider>();
        StartCoroutine(SpawnWave());
    }


    Vector3 GetSpawnPosition(){
        return new Vector3(
            Random.Range(boundingCollider.bounds.min.x, boundingCollider.bounds.max.x),
            0f,
            Random.Range(boundingCollider.bounds.min.z, boundingCollider.bounds.max.z)
        );
    }

    Sprite GetSprite(){
        return waveSprites[Random.Range(0, waveSprites.Length)];
    }

    IEnumerator SpawnWave(){
        yield return new WaitForSeconds(Random.Range(minTimeBetweenWave, maxTimeBetweenWave));
        GameObject waveObject = Instantiate(wavePrefab, GetSpawnPosition(), Quaternion.identity, transform);
        SpriteRenderer spriteRenderer = waveObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = GetSprite();
        spriteRenderer.flipX = flipTheX;

        StartCoroutine(SpawnWave());
    }
}
