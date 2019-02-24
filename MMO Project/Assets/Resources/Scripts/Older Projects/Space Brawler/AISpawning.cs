using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawning : MonoBehaviour {

    [SerializeField]
    private float SpawnRadiusMin = 1050;
    [SerializeField]
    private float SpawnRadiusMax = 1200;

    [SerializeField]
    private int MinSpawns = 20;
    [SerializeField]
    private int MaxSpawns = 30;
    private int SpawnCount;
    [SerializeField]
    private float NeutralToAgessiveSpawnnRate = 0.8f; // Rate of neutral to agressive Ai spawn rate

    [SerializeField]
    private GameObject NeutralAiPrefab;
    [SerializeField]
    private GameObject AgressiveAiPrefab;

    [SerializeField]
    private Transform SpawnTransform;
    [SerializeField]
    private Transform parentTransform;

    [SerializeField]
    private SphereCollider RadiusTrigger;
    [SerializeField]
    private SphereCollider SpawnCheckTrigger;

    // Use this for initialization
    void Start()
    {
        RadiusTrigger.isTrigger = true;
        RadiusTrigger.radius = SpawnRadiusMax;
        SpawnCheckTrigger.isTrigger = true;
        SpawnCheckTrigger.radius = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (SpawnCount < MinSpawns)
        {
            MoveSpawnTransform();
            SpawnRandom();
        }
    }

    private void MoveSpawnTransform()
    {
        float randRadius = Random.Range(SpawnRadiusMin, SpawnRadiusMax);
        float randX = randRadius * Mathf.Cos(Random.Range(0f, 2 * Mathf.PI));
        float randY = randRadius * Mathf.Cos(Random.Range(0f, 2 * Mathf.PI));
        float randZ = randRadius * Mathf.Cos(Random.Range(0f, 2 * Mathf.PI));

        SpawnTransform.localPosition = new Vector3(randX, randY, randZ);
    }

    //private void CheckLocation(float x, float y, float z, float RADIUS)
    //{
    //    SpawnCheckTrigger.center = new Vector3(x, y, z);
    //    SpawnCheckTrigger.radius = RADIUS;
    //}

    private void SpawnRandom()
    {
        float randChance = Random.Range(0f, 1f);
        if (randChance <= NeutralToAgessiveSpawnnRate)
        {
            GameObject temp = Instantiate(NeutralAiPrefab, SpawnTransform);
            temp.transform.parent = parentTransform;
        }
        else
        {
            GameObject temp = Instantiate(AgressiveAiPrefab, SpawnTransform);
            temp.transform.parent = parentTransform;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "NeutralAI" || other.tag == "AgressiveAi")
        {
            SpawnCount++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "NeutralAI" || other.tag == "AgressiveAi")
        {
            SpawnCount--;
        }
    }
}
