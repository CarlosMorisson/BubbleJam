using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[System.Serializable]
public class Theme
{
    public string themeName;
    public string normalBlocksFolder;
    public string challengeBlocksFolder;
}
public class LevelController : MonoBehaviour
{
    [Header("Themes Configuration")]
    [SerializeField]
    private List<Theme> themes = new List<Theme>();

    [SerializeField]
    private int currentThemeIndex = 0;

    [Header("Obstacles Configuration")]
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
    private int initialBlocks = 20;

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

    [Header("Post Process Control")]
    [SerializeField]
    public PostProcessVolume volume;

    [SerializeField]
    public float maxIntensity = 0.8f;

    [SerializeField]
    public float increaseSpeed = 0.1f;

    [SerializeField]
    private Vignette vignette;

    private float timeSinceLastSpawn = 0f;

    private List<GameObject> activeBlocks = new List<GameObject>();
    private List<GameObject> normalBlocksPool = new List<GameObject>();
    private List<GameObject> challengeBlocksPool = new List<GameObject>();
    private List<GameObject> usedNormalBlocks = new List<GameObject>();
    private List<GameObject> usedChallengeBlocks = new List<GameObject>();

    [SerializeField]
    [Range(0,18)]
    private int difficultyCounter = 0;

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
            if (volume != null)
            {
                volume.profile.TryGetSettings(out vignette);
            }
        }
        else
        {
            StopAllCoroutines();
            foreach (var block in activeBlocks)
            {
                Destroy(block);
            }
            if (volume != null)
            {
                volume.profile.TryGetSettings(out vignette);
                vignette.intensity.value = 0;
            }
            activeBlocks.Clear();
            specialBlockSpawned = false;
            blocksSpawned = 0;
            currentSpeed = initialSpeed;
            timeSinceLastSpawn = 0;
            Debug.Log("LevelController desativado.");
        }
    }

    private void StartLevel()
    {
        currentSpeed = initialSpeed;
        blockParent = GameObject.FindGameObjectWithTag("ObstaclesParent").transform;
        LoadBlocksForCurrentTheme();

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

    private void LoadBlocksForCurrentTheme()
    {
        Theme currentTheme = themes[currentThemeIndex];
        normalBlocksPool.Clear();
        challengeBlocksPool.Clear();
        usedNormalBlocks.Clear();
        usedChallengeBlocks.Clear();

        string normalFolderPath = $"{currentTheme.themeName}/{currentTheme.normalBlocksFolder}";
        string challengeFolderPath = $"{currentTheme.themeName}/{currentTheme.challengeBlocksFolder}";

        Object[] loadedNormalPrefabs = Resources.LoadAll(normalFolderPath, typeof(GameObject));
        Debug.Log(currentTheme.normalBlocksFolder);
        foreach (Object prefab in loadedNormalPrefabs)
        {
            normalBlocksPool.Add((GameObject)prefab);
        }

        Object[] loadedChallengePrefabs = Resources.LoadAll(challengeFolderPath, typeof(GameObject));
        foreach (Object prefab in loadedChallengePrefabs)
        {
            challengeBlocksPool.Add((GameObject)prefab);
        }
    }

    private IEnumerator SpawnBlockCoroutine()
    {
        while (true)
        {
            if (vignette != null && vignette.intensity.value < 0.5f)
            {
                vignette.intensity.value += increaseSpeed * blocksSpawned;
                if (vignette.intensity.value > 0.5f)
                    vignette.intensity.value = 0.5f;
            }
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
        if (activeBlocks.Capacity > 0)
        {
            for (int i = activeBlocks.Count - 1; i >= 0; i--)
            {
                if (activeBlocks[i].transform.position.y < despawnY || specialBlockSpawned)
                {
                    Destroy(activeBlocks[i]);
                    activeBlocks.RemoveAt(i);
                    StoreController.Instance.VerifyToMonetize(BubbleController.Instance.GetActiveBubbleCount(false));
                }
            }
        }
    }

    void SpawnBlock()
    {
        GameObject newBlock;

        if (blocksSpawned >= specialBlockSpawnInterval && !specialBlockSpawned)
        {
            specialBlockPrefab.SetActive(true);
            specialBlockSpawned = true;
        }
        else
        {
            if (difficultyCounter > 0 && challengeBlocksPool.Count > 0)
            {
                newBlock = GetChallengeBlock();
            }
            else
            {
                newBlock = GetNormalBlock();
            }

            newBlock.transform.position = new Vector3(0, spawnY, 0);
            newBlock.GetComponent<ObstacleMovemment>().Speed = ObstacleAcceleration();
            blocksSpawned++;
            activeBlocks.Add(newBlock);
        }
    }

    private GameObject GetNormalBlock()
    {
        if (normalBlocksPool.Count == 0)
        {
            normalBlocksPool.AddRange(usedNormalBlocks);
            usedNormalBlocks.Clear();
        }

        int randomIndex = Random.Range(0, normalBlocksPool.Count);
        GameObject selectedBlock = normalBlocksPool[randomIndex];
        normalBlocksPool.RemoveAt(randomIndex);
        usedNormalBlocks.Add(selectedBlock);

        return Instantiate(selectedBlock, blockParent);
    }

    private GameObject GetChallengeBlock()
    {
        if (challengeBlocksPool.Count == 0)
        {
            challengeBlocksPool.AddRange(usedChallengeBlocks);
            usedChallengeBlocks.Clear();
        }

        int randomIndex = Random.Range(0, challengeBlocksPool.Count);
        GameObject selectedBlock = challengeBlocksPool[randomIndex];
        challengeBlocksPool.RemoveAt(randomIndex);
        usedChallengeBlocks.Add(selectedBlock);

        return Instantiate(selectedBlock, blockParent);
    }

    private float ObstacleAcceleration()
    {
        currentSpeed = Mathf.Min(currentSpeed + accelerationRate * Time.deltaTime, maxSpeed);
        return currentSpeed;
    }

    public void IncreaseDifficulty()
    {
        difficultyCounter++;
    }

    public void SetTheme(int themeIndex)
    {
        if (themeIndex >= 0 && themeIndex < themes.Count)
        {
            currentThemeIndex = themeIndex;
            LoadBlocksForCurrentTheme();
        }
        else
        {
            Debug.LogError("Índice de tema inválido!");
        }
    }
}