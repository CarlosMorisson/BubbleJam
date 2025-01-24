using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleController : MonoBehaviour
{
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

}
