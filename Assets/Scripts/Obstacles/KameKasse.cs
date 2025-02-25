using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KameKasse : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            Alawakbar();


        this.gameObject.SetActive(false);
    }
    void Alawakbar()
    {
        GameObject allbubbles = GameObject.FindGameObjectWithTag("BubbleParent");
        foreach (Transform t in allbubbles.transform)
        {
            Debug.Log(t.name);
            if (t.gameObject.activeSelf)
                t.gameObject.GetComponent<BubbleCollider>().Alawakbar=true;

            return;
        }

    }
}
