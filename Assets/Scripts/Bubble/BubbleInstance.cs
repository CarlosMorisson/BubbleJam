using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleInstance : MonoBehaviour
{
    // Start is called before the first frame update
    public static BubbleInstance Instance;
    public Transform bubblesSpace;
    public GameObject bubbles;
    private void Awake()
    {
        Instance = this;
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(bubbles, bubblesSpace);
            BubbleMovement.Instance.NewBubble();

        }
        
    }

    public void BubbleInstace(Transform position)
    {


        //  //float newX = Random.Range(1, 2);
        //  //float newY = Random.Range(1, 2);

        // /* GameObject newBubble1 =*/ Instantiate(bubbles, bubblesSpace);
        //  //newBubble1.transform.position = position.position;
        //  //newBubble1.transform.Translate(new Vector3(newX, newY, 0));
        //  BubbleMovement.Instance.NewBubble();
        ///*  GameObject newBubble2 = */Instantiate(bubbles, bubblesSpace);
        //  //newBubble2.transform.position = position.position;
        //  //newBubble2.transform.Translate(new Vector3(newX, newY, 0));
        //  BubbleMovement.Instance.NewBubble();

        StartCoroutine(WaitTime());
    }

    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(Random.Range(1, 4));
        Instantiate(bubbles, bubblesSpace);
        BubbleMovement.Instance.NewBubble();
        yield return new WaitForSeconds(Random.Range(1, 4));
        Instantiate(bubbles, bubblesSpace);

        BubbleMovement.Instance.NewBubble();

    }
}
