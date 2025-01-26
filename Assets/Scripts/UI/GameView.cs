using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class GameView : MonoBehaviour
{
    public static GameView Instance;
    [SerializeField]
    private GameObject finalGamePanel;

    [SerializeField]
    private GameObject victoryGamePanel;

    [SerializeField]
    private Image healthBarImage;
    void Start()
    {
        Instance = this;
        GameController.OnGameStateChanged += LoadFinalGame;
    }
    void Update()
    {
        
    }
    public void LoadFinalGame(GameController.GameState newState)
    {
        if (GameController.GameState.End == newState)
        {
            finalGamePanel.SetActive(true);
            Debug.Log("Final Mostrado");
        }
        else if(GameController.GameState.Victory == newState)
        {
            victoryGamePanel.SetActive(true);
        }
    }
    public void SetHealth(float health, float maxHealth)
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        float targetFillAmount = health / maxHealth;

        // Animar o fillAmount usando DOTween
        healthBarImage.DOFillAmount(targetFillAmount, 0.5f).SetEase(Ease.InOutQuad);
    }
}
