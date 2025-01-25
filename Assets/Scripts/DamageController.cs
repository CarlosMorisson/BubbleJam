using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    public static Action OnTakeDamage;

    public static void OnBubbleTakeDamage()
    {
        OnTakeDamage?.Invoke();
    }

}
