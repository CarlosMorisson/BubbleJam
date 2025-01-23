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
    private float frenquancy;

    private bool isActive;


    void OnBecameVisible()
    {
        Debug.Log("O objeto est� vis�vel!");
        isActive = true;

        StartCoroutine(ShotBullets());
    }

    void OnBecameInvisible()
    {
        Debug.Log("O objeto est� invis�vel!");

        isActive = false;
        StopCoroutine(ShotBullets());
    }
   

    IEnumerator ShotBullets()
    {
        while (isActive) 
        {
                   
            Instantiate(bullet,space);
         
            yield return new WaitForSeconds(frenquancy);

           
        }

    }
      
}
