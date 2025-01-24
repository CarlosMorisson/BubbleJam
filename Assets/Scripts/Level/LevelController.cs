using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [Header("Obstacles Configuration")]
    [SerializeField]
    private List<GameObject> blockPrefabs = new List<GameObject>();

    [SerializeField]
    private GameObject specialBlockPrefab;

    [SerializeField]
    private int specialBlockSpawnInterval = 10;

    private Transform blockParent;

    [SerializeField]
    private Vector2 randomRangeX;

    [SerializeField]
    private float spawnY = 0f;

    [SerializeField]
    private float despawnY = -10f;

    [SerializeField]
    private int initialBlocks = 5;

    private int blocksSpawned = 0;
    private bool specialBlockSpawned = false;

    [Header("Acceleration")]
    [SerializeField]
    private float initialSpeed = 5f;

    [SerializeField]
    private float accelerationRate = 0.1f;

    [SerializeField]
    private float maxSpeed = 15f;

    private float currentSpeed;

    [Header("Timed Spawning")]
    [SerializeField]
    private float timeBetweenSpawns = 2f;

    private float timeSinceLastSpawn = 0f;

    private List<GameObject> activeBlocks = new List<GameObject>();

    private void OnEnable()
    {
        GameController.OnGameStateChanged += HandleGameStateChange;
    }

    private void OnDisable()
    {
        GameController.OnGameStateChanged -= HandleGameStateChange;
    }

    private void HandleGameStateChange(GameController.GameState newState)
    {
        if (newState == GameController.GameState.Game)
        {
            StartLevel();
        }
        else
        {
            StopAllCoroutines();
            foreach (var block in activeBlocks)
            {
                Destroy(block);
            }
            activeBlocks.Clear();
            specialBlockSpawned = false;
            blocksSpawned = 0;
            currentSpeed = initialSpeed;
            timeSinceLastSpawn = 0; // Reseta o timer de spawn
            Debug.Log("LevelController desativado.");
        }
    }

    private void StartLevel()
    {
        currentSpeed = initialSpeed;
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

        for (int i = 0; i < initialBlocks; i++)
        {
            SpawnBlock();
        }
        StartCoroutine(SpawnBlockCoroutine());
    }

    private IEnumerator SpawnBlockCoroutine()
    {
        while (true)
        {
            if (GameController.Instance.State == GameController.GameState.Game)
            {
                yield return new WaitForSeconds(timeBetweenSpawns);
                SpawnBlock();
            }
            else
            {
                yield break;
            }
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
                StoreController.Instance.VerifyToMonetize(BubbleController.Instance.GetActiveBubbleCount());
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

        if (blocksSpawned >= specialBlockSpawnInterval && !specialBlockSpawned)
        {
            newBlock = Instantiate(specialBlockPrefab, blockParent);
            specialBlockSpawned = true;
        }
        else
        {
            int randomIndex = Random.Range(0, blockPrefabs.Count);
            GameObject selectedPrefab = blockPrefabs[randomIndex];

            newBlock = Instantiate(selectedPrefab, blockParent);
            newBlock.transform.position = new Vector3(Random.Range(randomRangeX.x, randomRangeX.y), spawnY, 0);
            newBlock.GetComponent<ObstacleMovemment>().Speed = ObstacleAcceleration();
            blocksSpawned++;
        }
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