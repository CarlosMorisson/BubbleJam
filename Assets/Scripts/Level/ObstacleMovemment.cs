using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMovemment : MonoBehaviour
{
    public float Speed { private get; set; }
    void Update()
    {
        transform.Translate(Vector3.down * Speed * Time.deltaTime);
    }
}
