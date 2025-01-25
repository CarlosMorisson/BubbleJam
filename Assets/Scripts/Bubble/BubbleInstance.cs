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


        float newX = Random.Range(-1, 1);
        float newY = Random.Range(-1,1 );

        GameObject newBubble1 =Instantiate(bubbles, bubblesSpace);
        newBubble1.transform.position = position.position;
        newBubble1.transform.Translate(new Vector3(newX, newY, 0));
        newBubble1.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1))*6, ForceMode2D.Impulse);
        BubbleMovement.Instance.NewBubble();
         GameObject newBubble2 = Instantiate(bubbles, bubblesSpace);
        newBubble2.transform.position = position.position;
        newBubble2.transform.Translate(new Vector3(newX, newY, 0));
        BubbleMovement.Instance.NewBubble();
        newBubble1.gameObject.GetComponent<BubbleCollider>().ImunityBubble();
        newBubble2.gameObject.GetComponent<BubbleCollider>().ImunityBubble();
        newBubble1.GetComponent<SpriteRenderer>().color= Color.white;
        newBubble2.GetComponent<SpriteRenderer>().color = Color.white;
        newBubble2.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1))*6 ,ForceMode2D.Impulse);
        newBubble1.transform.localScale = new Vector3(0.26f,0.26f,0.26f);
        newBubble2.transform.localScale = new Vector3(0.26f, 0.26f, 0.26f);

        StartCoroutine(WaitTime(newBubble1,newBubble2));
    }

    IEnumerator WaitTime(GameObject obj1, GameObject obj2)
    {
        yield return new WaitForSeconds(2);
        obj1.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        obj2.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;



    }
}
