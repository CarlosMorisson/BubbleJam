using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoterControler : MonoBehaviour
{
    [SerializeField]
    private Transform space;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private float frequency;


    [SerializeField]
    private bool shakeMoviment;

    public float minAngle = -45f; // �ngulo m�nimo de rota��o
    public float maxAngle = 45f;  // �ngulo m�ximo de rota��o
    public float frequencySheke = 1f;  // Frequ�ncia de rota��o


    [SerializeField]
    private bool radar;
    public float detectionRadius = 5f;
    public LayerMask detectionLayer;
    public float rotationSpeed = 100f;



    private bool isActive;

    private void Start()
    {
        if (radar)
        {
            StartCoroutine(Radarcomportament());
        }
        else if (shakeMoviment)
        {
            StartCoroutine(ShekeComportament());
        }

    }

    void OnBecameVisible()
    {
        Debug.Log("O objeto est� vis�vel!");
        isActive = true;


        StartCoroutine(ShotBullets());
        if (radar)
        {
            StartCoroutine(Radarcomportament());
        }
        else if (shakeMoviment)
        {
            StartCoroutine(ShekeComportament());
        }
    }

    void OnBecameInvisible()
    {
      //  Debug.Log("O objeto est� invis�vel!");

        isActive = false;
        StopCoroutine(ShotBullets());
    }


    IEnumerator ShotBullets()
    {
        while (isActive)
        {

            Instantiate(bullet, space);

            yield return new WaitForSeconds(frequency);


        }

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

                // Rotar o objeto em dire��o � dire��o m�dia
                float angle = Mathf.Atan2(averageDirection.y, averageDirection.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
            }

            yield return new WaitForSeconds(frequency);


        }

    }

    IEnumerator ShekeComportament()
    {
        while (isActive)
        {
            float randomAngle = Random.Range(minAngle, maxAngle);

            // Rotaciona o GameObject
            transform.Rotate(Vector3.forward, randomAngle);
            yield return new WaitForSeconds(frequency);


        }

    }

}