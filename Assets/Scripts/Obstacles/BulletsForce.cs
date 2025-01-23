using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsForce : MonoBehaviour
{
    [SerializeField]
    private float  forceScale;
   
    private Transform origin;
    [SerializeField]
    private float lifeTime;
    private Vector2 direction;

    private void OnEnable()
    {
        origin=this.gameObject.transform.parent.parent.GetChild(1);
        Debug.Log(origin.name);
        direction = origin.right;//this.gameObject.transform.parent.eulerAngles;
        this.gameObject.GetComponent<Rigidbody2D>().AddForce(direction*forceScale,ForceMode2D.Impulse);
        StartCoroutine(BulletDestroy());

    }
    IEnumerator BulletDestroy()
    {
        yield return new WaitForSeconds(lifeTime);

        Destroy(this.gameObject);
    }
}
