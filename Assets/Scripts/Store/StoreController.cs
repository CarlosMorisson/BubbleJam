using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StoreController : MonoBehaviour
{
    public int playerCoins; // Valor atual do jogador
    public BuySkill[] itemsForSale; // Array de itens à venda

    private void Start()
    {
        // Carregar o valor do jogador do PlayerPrefs (se necessário)
        playerCoins = PlayerPrefs.GetInt("PlayerCoins", 0);
    }

    public void BuyItem(int itemIndex)
    {
        // Verificar se o índice do item é válido
        if (itemIndex >= 0 && itemIndex < itemsForSale.Length)
        {
            BuySkill item = itemsForSale[itemIndex];

            // Verificar se o jogador tem dinheiro suficiente
            if (playerCoins >= item.skillPrice && item.purchaseTime<item.maxPurchase)
            {
                // Subtrair o valor do jogador
                playerCoins -= item.skillPrice;

                // Salvar o novo valor do jogador no PlayerPrefs
                PlayerPrefs.SetInt("PlayerCoins", playerCoins);

                // Marcar o item como comprado
                item.isPurchased = true;
                item.purchaseTime++;
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
