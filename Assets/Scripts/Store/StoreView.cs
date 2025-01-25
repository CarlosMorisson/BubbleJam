using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using NEO.UiAnimations;

public class StoreView : MonoBehaviour
{
    public static StoreView Instance;
    [Header("Coins")]
    [SerializeField] private TextMeshProUGUI coinsText;

    private GameObject _descriptionPanel;
    private Button _buyButton;
    void Start()
    {
        PlayerPrefs.DeleteAll();
        Instance = this;
    }
   
    public void CheckDescription(BuySkill buy)
    {
        _descriptionPanel = buy.DescriptionPanel;

        RectTransform description = _descriptionPanel.transform as RectTransform;
        bool isOn = buy.GetComponent<Toggle>().isOn;

        if (isOn)
            description.NEOBounceIn(duration: 0.3f);
        else
            description.NEOBounceOut(duration: 0.3f);

        _descriptionPanel.GetComponentInChildren<TextMeshProUGUI>().text = "<b> " + buy.skillName + " : </b>" + buy.Descrition;
    }

    public void CheckItensThatCanBuy(BuySkill buy)
    {
        _buyButton = buy.BuyButton.GetComponent<Button>();
        if (buy.purchaseTime == buy.maxPurchase)
        {
            _buyButton.interactable = false;
            _buyButton.GetComponentInChildren<TextMeshProUGUI>().text="Comprado";
        }
        else
        {
            _buyButton.GetComponentInChildren<TextMeshProUGUI>().text = buy.skillPrice.ToString();
            //Colocar imagem representado vezes que comprou
        }
    }
    public void ActualizeValue(int skillValue)
    {
        coinsText.text = skillValue.ToString();
    }
}
