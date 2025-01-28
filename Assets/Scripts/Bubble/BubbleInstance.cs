using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleInstance : MonoBehaviour
{
    private const float SIZE_MINIBUBBLE = 0.26f;
    private const int BUBLES_TO_INSTANTIATE = 2;

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
        for (int i = 0; i < BUBLES_TO_INSTANTIATE; i++)
        {
            float newX = Random.Range(-1, 1);
            float newY = Random.Range(-1, 1);

            GameObject newBubble = Instantiate(bubbles, bubblesSpace);
            BubbleMovement.Instance.NewBubble();

            newBubble.GetComponent<SpriteRenderer>().color = Color.white;
            newBubble.GetComponent<BubbleCollider>().imunity = true;
            newBubble.GetComponent<BubbleCollider>().ImunityBubble();
            bubblesList.Add(newBubble);
            newBubble.transform.position = position.position;
            newBubble.transform.Translate(new Vector3(newX, newY, 0));

            newBubble.GetComponent<Rigidbody2D>()
                    .AddForce(new Vector2(Random.Range(-1, 1),
                    Random.Range(-1, 1)) * 6,
                    ForceMode2D.Impulse);

            newBubble.transform.localScale = new Vector2(SIZE_MINIBUBBLE, SIZE_MINIBUBBLE);
            StartCoroutine(WaitForStopBubble(newBubble));
        }
        /*GameObject newBubble1 =Instantiate(bubbles, bubblesSpace);
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

        StartCoroutine(WaitTime(newBubble1,newBubble2));*/
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

    IEnumerator WaitForStopBubble(GameObject obj)
    {
        yield return new WaitForSeconds(2);
        if(obj != null)
        {
            obj.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

        }
    }
}
