using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneControler : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isActive;
  
    public float detectionRadius = 50f;
    public LayerMask detectionLayer;

    public float velocit;
    

    void OnBecameVisible()
    {
       
        isActive = true;


       
            StartCoroutine(Radarcomportament());
    
    }

    void OnBecameInvisible()
    {
      

        isActive = false;
        StopCoroutine(Radarcomportament());
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se o objeto que entrou no trigger tem a tag "Fan"
        if (collision.CompareTag("Player"))
        {
            this.gameObject.GetComponent<CircleCollider2D>().radius = 1;

            this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;

            StartCoroutine(WaitFrame());           
        }


    }
    IEnumerator WaitFrame()
    {
        yield return new WaitForSeconds(0.5f);
        this.gameObject.SetActive(false);
    }
        IEnumerator Radarcomportament()
    {
        while (isActive)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, detectionLayer);

            if (colliders.Length > 0)
            {
                Vector3 averageDirection = Vector2.zero;
                foreach (Collider2D collider in colliders)
                {
                    averageDirection += (collider.transform.position - transform.position).normalized;
                }
                averageDirection /= colliders.Length;
                this.gameObject.GetComponent<Rigidbody2D>().AddForce(averageDirection *velocit,ForceMode2D.Impulse) ;
               
            }

            yield return new WaitForSeconds(1);
            this.gameObject.GetComponent<Rigidbody2D>().velocity=Vector2.zero;


        }

    }
}
