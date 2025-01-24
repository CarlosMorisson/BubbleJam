using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StoreController : MonoBehaviour
{
    public int playerCoins; // Valor atual do jogador
    public BuySkill[] itemsForSale; // Array de itens � venda

    private void Start()
    {
        // Carregar o valor do jogador do PlayerPrefs (se necess�rio)
        playerCoins = PlayerPrefs.GetInt("PlayerCoins", 0);
    }

    public void BuyItem(int itemIndex)
    {
        // Verificar se o �ndice do item � v�lido
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
                // Salvar a informa��o da compra no PlayerPrefs (opcional)
                // Voc� pode usar um formato JSON para salvar um array de booleanos
                // indicando quais itens foram comprados
            }
            else
            {
                Debug.Log("Voc� n�o tem moedas suficientes!");
            }
        }
        else
        {
            Debug.LogError("�ndice de item inv�lido!");
        }
    }
}
