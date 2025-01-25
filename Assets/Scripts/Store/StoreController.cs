using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StoreController : MonoBehaviour
{
    public static StoreController Instance;
    public int playerCoins; // Valor atual do jogador
    public BuySkill[] itemsForSale; // Array de itens à venda

    private void Start()
    {
        Instance = this;
        GameController.OnGameStateChanged += HandleGameStateChange;
        // Carregar o valor do jogador do PlayerPrefs (se necessário)
        playerCoins = PlayerPrefs.GetInt("PlayerCoins", 0);

        StoreView.Instance.ActualizeValue(playerCoins);
        //Verifica quais itens foram comprados para atualizar a UI
        foreach(BuySkill buy in itemsForSale)
        {
            buy.purchaseTime = PlayerPrefs.GetInt(buy.name + "purchaseTime");
            if (buy.purchaseTime > 0)
                buy.isPurchased = true;
            StoreView.Instance.CheckItensThatCanBuy(buy);
            
            BubbleController.Instance.CheckUpgrades(buy.skillName, buy.isPurchased);
        }
    }

    private void HandleGameStateChange(GameController.GameState newState)
    {
        
        if(GameController.GameState.Store == newState)
        {
            StoreView.Instance.StoreCanva.SetActive(true);
            Debug.Log("debuguei");
        }
        PlayerPrefs.SetInt("PlayerCoins", playerCoins);
    }
    public void VerifyToMonetize(int bubbleCount)
    {
        playerCoins += bubbleCount / 2;
    }
    public void BuyItem(int itemIndex)
    {
        // Verificar se o índice do item é válido
        if (itemIndex >= 0 && itemIndex < itemsForSale.Length)
        {
            BuySkill item = itemsForSale[itemIndex];

            // Verificar se o jogador tem dinheiro suficiente
            if (playerCoins >= item.skillPrice && item.purchaseTime < item.maxPurchase)
            {
                // Subtrair o valor do jogador
                playerCoins -= item.skillPrice;

                // Salvar o novo valor do jogador no PlayerPrefs
                PlayerPrefs.SetInt("PlayerCoins", playerCoins);
               
                // Marcar o item como comprado
                item.isPurchased = true;
                item.purchaseTime++;
                PlayerPrefs.SetInt(item.name + "purchaseTime", item.purchaseTime);

                StoreView.Instance.ActualizeValue(playerCoins);
                StoreView.Instance.CheckItensThatCanBuy(item);

                BubbleController.Instance.CheckUpgrades(item.skillName, item.isPurchased);
                // Salvar a informação da compra no PlayerPrefs (opcional)
                // Você pode usar um formato JSON para salvar um array de booleanos
                // indicando quais itens foram comprados
            }
            else
            {
                Debug.Log("Você não tem moedas suficientes!");
            }
        }
        else
        {
            Debug.LogError("Índice de item inválido!");
        }
    }
}
