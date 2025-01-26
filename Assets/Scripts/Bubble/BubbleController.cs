using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class BubbleController : MonoBehaviour
{
    public static BubbleController Instance;
    private Transform bubblesParent;
    private GameObject[] bubbles;


    [Header("Damage")]
    [SerializeField] [Tooltip("How much Damage a bubble give")]
    private float BubbleDamage;

    [Header("Bubble Skills")]
    [SerializeField]
    private bool doubleBubble, acidBubble, troiaBubble, bounceBubble;

    private int activeBubbleCount;

    private void OnEnable()
    {
        GameController.OnGameStateChanged += BubblesProps;
    }
    private void OnDisable()
    {
        GameController.OnGameStateChanged -= BubblesProps;
    }
    public void CheckBubbleCount()
    {
        int bubbleCount = bubblesParent.childCount;
        bubbles = new GameObject[bubbleCount];

        for (int i = 0; i < bubbleCount; i++)
        {
            activeBubbleCount = bubblesParent.childCount;
        }
    }
    private int CountActiveBubbles()
    {
        int activeBubbleCount = -1;
        foreach (Transform child in bubblesParent)
        {
            
            if (child.gameObject.activeSelf)
            {
                activeBubbleCount++;
            }
        }
        return activeBubbleCount;
    }

    public int GetActiveBubbleCount()
    {
        if (bubblesParent == null)
        {
            Debug.LogError("Bubbles Parent não foi encontrado!");
            return 0; // Retorna 0 em caso de erro
        }
        if (CountActiveBubbles() <= 0)
        {
            if (BossModel.Instance == null || BossModel.Instance.bossLife > 0)
            {
                GameController.Instance.GetGameState(GameController.GameState.End);
            }
            else if (BossModel.Instance.bossLife <= 0)
            {
                GameController.Instance.GetGameState(GameController.GameState.Victory);
            }
        }
            
        return CountActiveBubbles();
    }
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        BubblesProps(GameController.GameState.Game);
    }
    public void BubblesProps(GameController.GameState state)
    {
        
        if (state== GameController.GameState.Game)
        {
            bubblesParent = GameObject.FindGameObjectWithTag("BubbleParent").transform;
            if (bubblesParent != null)
            {
                int bubbleCount = bubblesParent.childCount;
                bubbles = new GameObject[bubbleCount];
                activeBubbleCount = CountActiveBubbles();
                for (int i = 0; i < bubbleCount; i++)
                {
                    //if (bubbles[i].name.Contains("Clone"))
                    //{
                    //    Destroy(bubbles[i]);    
                    //}
                        bubbles[i] = bubblesParent.GetChild(i).gameObject;
                    bubbles[i].GetComponent<BubbleCollider>().bubbleDamage = BubbleDamage;

                    bubbles[i].GetComponent<BubbleCollider>().Acid = acidBubble;
                    bubbles[i].GetComponent<BubbleCollider>().Double = doubleBubble;
                    bubbles[i].GetComponent<BubbleCollider>().Troy = troiaBubble;
                    bubbles[i].GetComponent<BubbleCollider>().Bounce = bounceBubble;

                }
            }

        }
       
    }
    public void CheckUpgrades(string skillName, bool isPurchased)
    {
        //Sim é porquice mas é só pra Jam, se quiser expandir tem q tirar essa caganeira
        if (skillName == "Bolha Dupla")
            doubleBubble = isPurchased;
        if (skillName == "Bolha de Troia")
            troiaBubble = isPurchased;
        if (skillName == "Bolha Acida")
            acidBubble = isPurchased;
        if (skillName == " Bolha Ricochete")
            bounceBubble = isPurchased;
    }
}
