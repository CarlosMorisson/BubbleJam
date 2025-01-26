using UnityEngine;

public class BubbleMovement : MonoBehaviour
{
    public static BubbleMovement Instance;
    private Transform bubblesParent;
    private GameObject[] bubbles; // Array de bolhas
    [Header("Position Parameters")] 
    [SerializeField] private Vector2 randomRangeX = new Vector2(-5f, 5f); // Intervalo aleatório no eixo X
    [SerializeField] private float yDistance = 2f; // Distância fixa no eixo Y entre as bolhas

    [Header("Bubble Parameters")]
    [SerializeField] [Range(0, 10)] private float baseSpeed = 5f; // Velocidade base
    [SerializeField] [Range(0, 50)] private float followRadius = 2f; // Raio de detecção de vizinhos
    [SerializeField] [Range(0, 10)] private float separationWeight = 2f; // Peso da separação
    [SerializeField] [Range(0, 10)] private float avoidanceRadius = 1f; // Peso da coesão
    [SerializeField] [Range(0, 10)] private float maxForce = 0.5f; // Força máxima
    [SerializeField] [Range(0, 5)] private float speedTolerance;

    private void Awake()
    {
        Instance = this;
    }
  
    public void NewBubble()
    {
        bubblesParent = GameObject.FindGameObjectWithTag("BubbleParent").transform;

        if (bubblesParent != null)
        {
            int bubbleCount = bubblesParent.childCount;
            bubbles = new GameObject[bubbleCount];

            for (int i = 0; i < bubbleCount; i++)
            {
                bubbles[i] = bubblesParent.GetChild(i).gameObject;
            }
         
        }
        else
        {
            Debug.LogError("Bubbles Parent não foi encontrado! Certifique-se de que ele está com a tag 'BubbleParent'.");
        }
    }
    private void HandleGameStateChange(GameController.GameState newState)
    {
        if (newState == GameController.GameState.Game)
        {
            
            NewBubble();
            PositionBubbles();
        }
        else 
        {
            ActiveAllBubbles();
            PositionBubbles();
            StopAllCoroutines();
            // Reseta o timer de spawn

        }
    }
    private void ActiveAllBubbles()
    {
        if (bubblesParent != null)
        {
            for (int i = 0; i < bubblesParent.childCount; i++)
            {
                bubbles[i].SetActive(true);
            }

        }
    }
    private void Start()
    {
        // Encontrar o pai das bolhas e inicializar o array
        GameController.OnGameStateChanged += HandleGameStateChange;
    }
    private void PositionBubbles()
    {
        for (int i = 0; i < bubbles.Length; i++)
        {
            if (!bubbles[i].activeSelf) continue; // Ignorar bolhas inativas

            Vector3 newPosition;

            // Próximas bolhas têm um valor aleatório no eixo X e uma distância fixa no eixo Y
            float randomX = Random.Range(randomRangeX.x, randomRangeX.y);
            float fixedY =  yDistance;
            newPosition = new Vector3(randomX, fixedY, 0f);

            // Aplicar a nova posição à bolha
            bubbles[i].transform.position = newPosition;
        }
    }
    private void Update()
    {
        // Obter a posição do mouse no mundo
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Garantir que as bolhas permaneçam no plano 2D
        if (GameController.GameState.Game != GameController.Instance.State)
            return;
        foreach (GameObject bubble in bubbles)
        {
            if (!bubble.activeSelf) continue;

            Vector3 followForce = FollowMouse(bubble, mousePosition);
            Vector3 separation = Separation(bubble);

            // Combinar forças
            Vector3 totalForce = followForce + (separation * separationWeight);

            // Limitar a força total
            totalForce = Vector3.ClampMagnitude(totalForce, maxForce);

            // Aplicar movimento
            bubble.transform.position += totalForce * baseSpeed * Time.deltaTime;
        }
    }

    private Vector3 FollowMouse(GameObject bubble, Vector3 mousePosition)
    {
        // Vetor apontando para o mouse
        Vector3 directionToMouse = mousePosition - bubble.transform.position;

        // Checar se a bolha está dentro do raio para seguir o mouse
        if (directionToMouse.magnitude < followRadius)
        {
            directionToMouse.Normalize();
            return directionToMouse;
        }

        return Vector3.zero;
    }

    private Vector3 Separation(GameObject bubble)
    {
        Vector3 steer = Vector3.zero;
        int count = 0;

        foreach (GameObject other in bubbles)
        {
            if (other == bubble || !other.activeSelf) continue;

            float distance = Vector3.Distance(bubble.transform.position, other.transform.position);
            if (distance < avoidanceRadius)
            {
                // Vetor apontando para longe do vizinho
                Vector3 diff = bubble.transform.position - other.transform.position;
                diff.Normalize();
                if(distance>0)
                    diff /= distance; // Ponderar pela distância
                steer += diff;
                count++;
            }
        }

        if (count > 0)
        {
            steer /= count;
        }

        return steer;
    }
}
