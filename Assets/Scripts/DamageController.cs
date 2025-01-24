using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    public static Action<int> OnTakeDamage;

    void OnEnable()
    {
        OnTakeDamage += SpaceTakeDamage;

    }
    private void OnDisable()
    {

    }

    void SpaceTakeDamage(int num)
    {
        DamageView.Instance.UpdateText();

       
    }

}
