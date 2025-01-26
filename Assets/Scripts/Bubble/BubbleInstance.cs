using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleInstance : MonoBehaviour
{
    // Start is called before the first frame update
    public static BubbleInstance Instance;
    public Transform bubblesSpace;
    public GameObject bubbles;
    public List<GameObject> bubblesList;
    private void Awake()
    {
        Instance = this;
    }
    // Update is called once per frame
    private void OnEnable()
    {
        GameController.OnGameStateChanged += DeleteClones;
    }
    private void OnDisable()
    {
        GameController.OnGameStateChanged -= DeleteClones;
    }


    public void BubbleInstace(Transform position)
    {


        float newX = Random.Range(-1, 1);
        float newY = Random.Range(-1,1 );

        GameObject newBubble1 =Instantiate(bubbles, bubblesSpace);
        newBubble1.GetComponent<SpriteRenderer>().color = Color.white;
        newBubble1.gameObject.GetComponent<BubbleCollider>().imunity = true ;
       
        newBubble1.gameObject.GetComponent<BubbleCollider>().ImunityBubble();

        bubblesList.Add(newBubble1);
    
        newBubble1.transform.position = position.position;
        newBubble1.transform.Translate(new Vector3(newX, newY, 0));
        newBubble1.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1))*6, ForceMode2D.Impulse);
      


         GameObject newBubble2 = Instantiate(bubbles, bubblesSpace);
        newBubble2.GetComponent<SpriteRenderer>().color = Color.white;
        newBubble2.gameObject.GetComponent<BubbleCollider>().imunity=true;
     
        newBubble2.gameObject.GetComponent<BubbleCollider>().ImunityBubble();
      
        newBubble2.transform.position = position.position;
        newBubble2.transform.Translate(new Vector3(newX, newY, 0));
        BubbleMovement.Instance.NewBubble();
        bubblesList.Add(newBubble2);

    




        newBubble2.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1))*6 ,ForceMode2D.Impulse);
        newBubble1.transform.localScale = new Vector3(0.26f,0.26f,0.26f);
        newBubble2.transform.localScale = new Vector3(0.26f, 0.26f, 0.26f);

        StartCoroutine(WaitTime(newBubble1,newBubble2));
    }

    void DeleteClones(GameController.GameState state)
    {
        if (state == GameController.GameState.Store|| state == GameController.GameState.End|| state == GameController.GameState.Victory)
        {
            foreach (GameObject bubble in bubblesList)
            {
                Destroy(bubble);
            }
            bubblesList.Clear();
        }
        //BubbleMovement.Instance.NewBubble();
        //BubbleMovement.Instance.PositionBubbles();

    }

    IEnumerator WaitTime(GameObject obj1, GameObject obj2)
    {
        yield return new WaitForSeconds(2);
        if(obj1!=null)
        {
            obj1.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

        }

        if (obj2!=null)
        {
            obj2.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }

     



    }
}
