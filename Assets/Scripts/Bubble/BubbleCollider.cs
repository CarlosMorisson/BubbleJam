using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleCollider : MonoBehaviour
{
    [Header("Fan Influence")]
    [SerializeField] [Range(1, 20)] [Tooltip("How much the bubble can knockback")]
    private float knockbackForce = 10f;

    [SerializeField] [Range(1, 20)] [Tooltip("The max distance to apply the knockback")]
    public float maxPushDistance = 5f;

    private Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
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
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Fan") || collision.CompareTag("Teleport"))
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
}
