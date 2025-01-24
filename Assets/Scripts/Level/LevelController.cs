using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [Header("Obstacles Configuration")]
    [SerializeField] 
    private List<GameObject> blockPrefabs = new List<GameObject>();

    [SerializeField] [Tooltip("The Objective of game")] 
    private GameObject specialBlockPrefab; // Prefab do bloco especial

    [SerializeField] [Tooltip("Number of instances to go to the object")] 
    private int specialBlockSpawnInterval = 10; // A cada quantos blocos normais o bloco especial aparece

    private Transform blockParent;

    [SerializeField] [Tooltip("Randon x position of instances")]

    private Vector2 randomRangeX;

    [SerializeField] [Range(0, 30)] [Tooltip("Height in Y to spawn the blocks")]
    private float spawnY = 0f;

    [SerializeField] [Range(0, -30)] [Tooltip("Height in Y to destroy the blocks")]
    private float despawnY = -10f;

    [SerializeField] [Range(0, 30)] [Tooltip("Number of blocks")]
    private int initialBlocks = 5;

    private int blocksSpawned = 0;
    //
    [Header("Acceleration")]

    [SerializeField] [Tooltip("initial speed")]
    private float initialSpeed = 5f;

    [SerializeField] [Tooltip("how much can accelerate")]
    private float accelerationRate = 0.1f;

    [Tooltip("the max peed that the object can go")] [SerializeField] 
    private float maxSpeed = 15f;

    private float currentSpeed;
    //
    [Header("Timed Spawning")]
    [SerializeField] [Tooltip("Time between spawn")] [Range(1,15)]
    private float timeBetweenSpawns = 2f;

    private float timeSinceLastSpawn = 0f;
    //
    private List<GameObject> activeBlocks = new List<GameObject>();
    private bool specialBlockSpawned = false; // Flag para controlar se o bloco especial já foi instanciado

    void Start()
    {
        currentSpeed = initialSpeed;
        // Carrega os prefabs da pasta Resources
        blockParent = GameObject.FindGameObjectWithTag("ObstaclesParent").transform;
        Object[] loadedPrefabs = Resources.LoadAll("Blocos", typeof(GameObject));
        foreach (Object prefab in loadedPrefabs)
        {
            blockPrefabs.Add((GameObject)prefab);
        }
        if (specialBlockPrefab == null)
        {
            Debug.LogError("Prefab do bloco especial não atribuído!");
            return;
        }

        // Instancia os blocos iniciais
        for (int i = 0; i < initialBlocks; i++)
        {
            SpawnBlock();
        }
    }

    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn >= timeBetweenSpawns)
        {
            SpawnBlock();
            timeSinceLastSpawn = 0f;
        }

        // Verifica se os blocos saíram da tela e os destrói
        for (int i = activeBlocks.Count - 1; i >= 0; i--)
        {
            if (activeBlocks[i].transform.position.y < despawnY)
            {
                Destroy(activeBlocks[i]);
                activeBlocks.RemoveAt(i);
            }
        }
    }

    void SpawnBlock()
    {
        if (blockPrefabs.Count == 0 || specialBlockSpawned)
        {
            
            return;
        }
        
        GameObject newBlock;

        // Verifica se é hora de spawnar o bloco especial e se ele ainda não foi spawnado
        if (blocksSpawned >= specialBlockSpawnInterval && !specialBlockSpawned)
        {
            newBlock = Instantiate(specialBlockPrefab, blockParent);
            specialBlockSpawned = true; // Define a flag para true para impedir novas instâncias
        }
        else
        {
            int randomIndex = Random.Range(0, blockPrefabs.Count);
            GameObject selectedPrefab = blockPrefabs[randomIndex];

            newBlock = Instantiate(selectedPrefab, blockParent);
            newBlock.transform.position = new Vector3(Random.Range(randomRangeX.x, randomRangeX.y), spawnY, 0);
            newBlock.GetComponent<ObstacleMovemment>().Speed = ObstacleAcceleration();
            blocksSpawned++; // Incrementa o contador de blocos normais
        }

        newBlock.transform.position = new Vector3(0, spawnY, 0);

        activeBlocks.Add(newBlock);

        float randomX = Random.Range(randomRangeX.x, randomRangeX.y);
        Vector3 newPosition = newBlock.transform.position;
        newPosition.x = randomX;
        newBlock.transform.position = newPosition;
    }
    private float ObstacleAcceleration()
    {
        currentSpeed = Mathf.Min(currentSpeed + accelerationRate * Time.deltaTime, maxSpeed);
        return currentSpeed;
    }
}