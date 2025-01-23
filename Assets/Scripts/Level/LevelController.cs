using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [Header("Obstacles Configuration")]
    [SerializeField]
    private List<GameObject> blockPrefabs = new List<GameObject>();
    [SerializeField]
    [Tooltip("The Objective of game")]
    private GameObject specialBlockPrefab; // Prefab do bloco especial
    [SerializeField]
    [Tooltip("Number of instances to go to the object")]
    private int specialBlockSpawnInterval = 10; // A cada quantos blocos normais o bloco especial aparece
    private Transform blockParent;
    [SerializeField]
    [Tooltip("Randon x position of instances")]
    private Vector2 randomRangeX;
    [SerializeField]
    [Tooltip("Height of blocks padronized")]
    private float blockHeight = 1f;
    [SerializeField]
    [Range(0, 30)]
    [Tooltip("Height in Y to spawn the blocks")]
    private float spawnY = 0f;
    [SerializeField]
    [Range(0, -30)]
    [Tooltip("Height in Y to destroy the blocks")]
    private float despawnY = -10f;
    [SerializeField]
    [Range(0, 30)]
    [Tooltip("Number of blocks")]
    private int initialBlocks = 5;
    [SerializeField]
    [Range(0, 30)]
    [Tooltip("Number of instances to finish the game")]
    private int blocksSpawned = 0; // Contador de blocos normais instanciados

    private List<GameObject> activeBlocks = new List<GameObject>();
    private bool specialBlockSpawned = false; // Flag para controlar se o bloco especial já foi instanciado

    void Start()
    {
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
        // Verifica se os blocos saíram da tela e os destrói
        for (int i = activeBlocks.Count - 1; i >= 0; i--)
        {
            if (activeBlocks[i].transform.position.y < despawnY)
            {
                Destroy(activeBlocks[i]);
                activeBlocks.RemoveAt(i);
                // Spawna outro quando destruido
                SpawnBlock();
            }
        }
    }

    void SpawnBlock()
    {
        if (blockPrefabs.Count == 0)
        {
            Debug.LogError("Nenhum prefab de bloco encontrado na pasta Resources/Blocos!");
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

            blocksSpawned++; // Incrementa o contador de blocos normais
        }

        newBlock.transform.position = new Vector3(0, spawnY, 0);
        spawnY += blockHeight;

        activeBlocks.Add(newBlock);

        float randomX = Random.Range(randomRangeX.x, randomRangeX.y);
        Vector3 newPosition = newBlock.transform.position;
        newPosition.x = randomX;
        newBlock.transform.position = newPosition;
    }
}