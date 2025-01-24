using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuySkill : MonoBehaviour
{
    public string skillName;
    public int skillPrice;
    public bool isPurchased;
    public int purchaseTime;
    public int maxPurchase;
}
