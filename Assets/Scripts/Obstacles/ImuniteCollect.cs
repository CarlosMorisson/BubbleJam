using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImuniteCollect : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            AllImunity();


        this.gameObject.SetActive(false);
    }
    void AllImunity()
    {
       GameObject allbubbles = GameObject.FindGameObjectWithTag("BubbleParent");
        foreach (Transform t in allbubbles.transform)
        {
            Debug.Log(t.name);
            if(t.gameObject.activeSelf)
                t.gameObject.GetComponent<BubbleCollider>().ImunityBubble();
        }

    }
}
