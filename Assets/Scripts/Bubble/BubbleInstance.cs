using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleInstance : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform bubblesSpace;
    public GameObject bubbles;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(bubbles,bubblesSpace);
            BubbleMovement.Instance.NewBubble();

        }
        
    }
}
