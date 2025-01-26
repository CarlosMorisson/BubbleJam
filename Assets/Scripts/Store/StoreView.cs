using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using NEO.UiAnimations;
using DG.Tweening;
public class StoreView : MonoBehaviour
{
    public static StoreView Instance;
    [Header("Coins")]

    [SerializeField]
    private TextMeshProUGUI coinsText;
    [SerializeField] public GameObject StoreCanva;
    [SerializeField] private RectTransform _storeItensContainer, _descriptionsContainer, _bequer, _startButton,_table;

    private Vector3 _storeItensInitialPosition, _descriptionsInitialPosition, _bequerInitialPosition, _startInitialPosition,_tableInitialPos;
    void Start()
    {
        
        Instance = this;

        _storeItensInitialPosition = _storeItensContainer.anchoredPosition;
        _descriptionsInitialPosition = _descriptionsContainer.anchoredPosition;
        _bequerInitialPosition = _bequer.anchoredPosition;
        _startInitialPosition = _startButton.anchoredPosition;
        _tableInitialPos = _table.anchoredPosition;

    }
   
    public void CheckDescription(BuySkill buy)
    {
        GameObject _descriptionPanel = buy.DescriptionPanel;

        RectTransform description = _descriptionPanel.transform as RectTransform;
        bool isOn = buy.GetComponent<Toggle>().isOn;

        if (isOn)
        {
            _descriptionPanel.SetActive(true);
            description.NEOBounceIn(duration: 0.3f);
        }
        else
        {
            _descriptionPanel.SetActive(false);
            description.NEOBounceOut(duration: 0.3f);
        }

        _descriptionPanel.GetComponentInChildren<TextMeshProUGUI>().text = "<b> " + buy.skillName + " : </b>" + buy.Descrition;
    }

    public void CheckItensThatCanBuy(BuySkill buy)
    {
        Button _buyButton = buy.BuyButton.GetComponent<Button>();
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

    public void StartGameButton()
    {
        float duration = 1f; // Duração de cada animação
        float delayBetween = 0.2f; // Atraso entre os elementos

        Sequence sequence = DOTween.Sequence();

        // Animação de descida
        sequence.Insert(delayBetween, _storeItensContainer.DOAnchorPosY(-Screen.height, duration).SetEase(Ease.InOutQuad));
        sequence.Insert(delayBetween, _descriptionsContainer.DOAnchorPosY(-Screen.height, duration).SetEase(Ease.InOutQuad));
        sequence.Insert(delayBetween, _bequer.DOShakeAnchorPos(delayBetween * 4, strength: new Vector2(10f, 0), vibrato: 20, randomness: 90));

        sequence.InsertCallback(delayBetween * 2.5f, () =>
        { 
            _bequer.GetComponent<ParticleSystem>().Play();
            AudioController.Instance.PlayAudio("SFX", "BolhasSubindo");
        });
        sequence.InsertCallback(delayBetween * 3, () => GameController.Instance.StartGame());
        sequence.Insert(delayBetween * 3, _bequer.DOAnchorPosY(-Screen.height, duration).SetEase(Ease.InOutQuad));
        sequence.Insert(delayBetween * 4, _table.DOAnchorPosY(-Screen.height, duration).SetEase(Ease.InOutQuad));
        sequence.InsertCallback(0, () => _startButton.GetComponent<ParticleSystem>().Play());
        sequence.InsertCallback(0.01f, () => _startButton.anchoredPosition = new Vector2(0, -Screen.height))
            .OnStart(() =>
            {
                AudioController.Instance.PlayAudio("SFX", "Swip");
            })
            .OnComplete(() => _startButton.GetComponent<ParticleSystem>().Stop());
        sequence.Insert(delayBetween, Camera.main.DOShakeRotation(delayBetween * 3, strength: new Vector2(1f, 1f), vibrato: 20, randomness: 90))
            .OnStart(() => AudioController.Instance.PlayAudio("SFX","Tremendo"));

    }

    public void BackStoreButtons()
    {
        StoreCanva.SetActive(true);
        float duration = 0.25f; // Duração de cada animação
        float delayBetween = 0.1f; // Atraso entre os elementos

        Sequence sequence = DOTween.Sequence();
        // Animação de volta às posições iniciais
        sequence.Insert(duration + delayBetween, _storeItensContainer.DOAnchorPosY(_storeItensInitialPosition.y, duration).SetEase(Ease.InOutQuad));
        sequence.Insert(duration + delayBetween, _descriptionsContainer.DOAnchorPosY(_descriptionsInitialPosition.y, duration).SetEase(Ease.InOutQuad));
        sequence.Insert(duration + delayBetween * 2, _bequer.DOAnchorPosY(_bequerInitialPosition.y, duration).SetEase(Ease.InOutQuad));
        sequence.Insert(duration + delayBetween * 2, _table.DOAnchorPosY(_tableInitialPos.y, duration).SetEase(Ease.InOutQuad));
        sequence.Insert(duration, _startButton.DOAnchorPosY(_startInitialPosition.y, duration).SetEase(Ease.InOutQuad));
    }
}
