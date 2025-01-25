using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StoreView : MonoBehaviour
{
    public static StoreView Instance;
    [Header("Coins")]
    [SerializeField]
    private TextMeshProUGUI coinsText;

    private bool showDescription;

    void Start()
    {
        Instance = this;
    }
   
    public void CheckDescription(BuySkill buy)
    {
        showDescription = !showDescription;
        buy.DescriptionPanel.gameObject.SetActive(showDescription);
        //if(descriptionPanel)
        //Efeito DoTween
        //else
        //EfeitoDoTween
        buy.DescriptionPanel.GetComponentInChildren<TextMeshProUGUI>().text = "<b> " + buy.skillName + " : </b>" + buy.Descrition;
    }

    public void CheckItensThatCanBuy(BuySkill buy)
    {
        if (buy.purchaseTime == buy.maxPurchase)
        {
            buy.BuyButton.GetComponent<Button>().interactable = false;
            buy.BuyButton.GetComponentInChildren<TextMeshProUGUI>().text="Comprado";
        }
        else
        {
            buy.BuyButton.GetComponentInChildren<TextMeshProUGUI>().text = buy.skillPrice.ToString();
            //Colocar imagem representado vezes que comprou
        }
    }
    public void ActualizeValue(int skillValue)
    {
        coinsText.text = skillValue.ToString();
    }
}
