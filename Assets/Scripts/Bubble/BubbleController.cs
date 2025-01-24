using System.Collections;
using System.Collections.Generic;
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

    void Start()
    {
        Instance = this;
        bubblesParent = GameObject.FindGameObjectWithTag("BubbleParent").transform;
        if (bubblesParent != null)
        {
            int bubbleCount = bubblesParent.childCount;
            bubbles = new GameObject[bubbleCount];

            for (int i = 0; i < bubbleCount; i++)
            {
                bubbles[i] = bubblesParent.GetChild(i).gameObject;
                bubbles[i].GetComponent<BubbleCollider>().bubbleDamage = BubbleDamage;
            }
        }
    }
    public void CheckUpgrades(string skillName, bool isPurchased)
    {
        //Sim � porquice mas � s� pra Jam, se quiser expandir tem q tirar essa caganeira
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