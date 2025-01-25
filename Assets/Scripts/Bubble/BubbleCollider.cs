//using System;
using System.Collections;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class BubbleCollider : MonoBehaviour
{
    [Header("Fan Influence")]
    [SerializeField] [Range(1, 20)] [Tooltip("How much the bubble can knockback")]
    private float knockbackForce = 10f;

    [SerializeField] [Range(1, 20)] [Tooltip("The max distance to apply the knockback")]
    public float maxPushDistance = 5f;

    [Header("Bubble Fight")]
    [SerializeField] [Tooltip("Time To enable bubble collision")]
    private float collisionTimer = 2;
    public float bubbleDamage { private get; set; }
    [Header("Bubble improvement")]
    public bool Acid, Double, Troy, Bounce;

    public bool imunity;
    private Vector3 direction;   

   

    private Rigidbody2D rb;
    private void OnEnable()
    {

        ChangeCollor(GameController.GameState.Game);

        GameController.OnGameStateChanged += ChangeCollor;



    }
    public void ChangeCollor(GameController.GameState state)
    {
        Debug.Log("change cololor");
        if(state!=GameController.GameState.Game)
        {
            return;
        }

        Debug.Log("change cololor passou");
        if (Troy)
        {
            Debug.Log("change cololor Troy");
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.4f, 0.93f, 1, 1);
        }
        else if (Acid)
        {
            Debug.Log("change cololor Acoid");
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.49f, 0.85f, 0.49f, 1);
        }
        else if (Bounce)
        {
            Debug.Log("change cololor kik");
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.49f, 0.49f, 1, 1);

        }
        else if (Double)
        {
            Debug.Log("change cololor doubole");
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0.85f, 0.45f, 1);

        }
        else
        {
            Debug.Log("change cololor sem cor ");
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    private void Start()
    {
       
        rb = GetComponent<Rigidbody2D>();
        //Fazer com que novas bolhas nao sejam imediatamente destruidas quando ocorrer colisao
        StartCoroutine(EnableCollision());
      
    }
    private IEnumerator EnableCollision()
    {
        rb.isKinematic = true;
        yield return new WaitForSeconds(collisionTimer);
        rb.isKinematic = false;
    }
    #region Colliders
    private void OnTriggerEnter2D(Collider2D collision)
    {
     
        // Verifica se o objeto que entrou no trigger tem a tag "Fan"
        if (collision.CompareTag("Fan"))
        {
            // Calcula a direção *contrária* ao centro do collider do "Fan"
            Vector2 knockbackDirection = transform.position - collision.transform.position;
            knockbackDirection.Normalize();

            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }else if (collision.CompareTag("Teleport"))
        {
            collision.GetComponentInParent<WholeEffect>().TeleportPlayer(this.gameObject);
        }
        else if (collision.CompareTag("Damage"))
        {
            if(Troy)
            {
                Troy = false;
                ImunityBubble();

                ChangeCollor(GameController.GameState.Game);
                StartCoroutine(TroyImpruvment());
            
            }
            else if(Acid)
            {
                Acid = false;
                ImunityBubble();
                ChangeCollor(GameController.GameState.Game);
                this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }
            else if(Bounce)
            {
                Bounce=false;
                ChangeCollor(GameController.GameState.Game);
                ImunityBubble();
                direction= this.transform.position - collision.transform.position;
                rb.AddForce(direction*8, ForceMode2D.Impulse);
                StartCoroutine(BaouceFluid());
                this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;


            }
            else if(Double)
            {
                Double=false;
                DoubleBooble();
                ChangeCollor(GameController.GameState.Game);

            }
            else
            {
                ChangeCollor(GameController.GameState.Game);
                TakeDamage();
            }
           
        }
        else if (collision.CompareTag("Enemy"))
        {
            ApplyDamage();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Fan") || collision.CompareTag("Teleport") || collision.CompareTag("Attraction"))
        {
            rb.velocity = Vector2.zero;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Attraction"))
        {
            collision.GetComponentInParent<WholeEffect>().ApplyAttraction(rb);
        }
    }
    #endregion
    void ApplyDamage()
    {
        BossModel.OnTakeDamage.Invoke((int)bubbleDamage);
        TakeDamage();
    }
    void  TakeDamage()
    {
        if(imunity)
        {
            DamageController.OnTakeDamage.Invoke();
            this.gameObject.SetActive(false);

        }
     

      
      
    }
    IEnumerator TroyImpruvment()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0);
        yield return new WaitForSeconds(1);
        this.gameObject.GetComponent<SpriteRenderer>().color =Color.white;
        this.gameObject.transform.localScale = new Vector3(0.26f, 0.26f, 0.26f);
     




    }

    public void ImunityBubble()
    {
        StartCoroutine(Imunity());
    }
    IEnumerator Imunity()
    {
        imunity = false;
       
        yield return new WaitForSeconds(2);
        imunity = true;

    }

    void DoubleBooble()
    {
       
        BubbleInstance.Instance.BubbleInstace(this.gameObject.transform);
       
        this.gameObject.SetActive(false);



    }

    IEnumerator BaouceFluid()
    {
      
            yield return new WaitForSeconds(2);
            rb.velocity = Vector2.zero;





    }
}
