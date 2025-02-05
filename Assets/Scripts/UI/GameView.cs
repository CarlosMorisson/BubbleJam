using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using NEO.UiAnimations;

public class GameView : MonoBehaviour
{
    public static GameView Instance;
    [SerializeField]
    private GameObject finalGamePanel;

    [SerializeField]
    private GameObject victoryGamePanel;

    [SerializeField] private RectTransform _bubbleCount;
    
    [SerializeField]
    private Image healthBarImage;
    void Start()
    {
        Instance = this;
    }
    private void OnEnable()
    {
        GameController.OnGameStateChanged += LoadFinalGame;
    }
    private void OnDisable()
    {
        GameController.OnGameStateChanged -= LoadFinalGame;
    }
    public void LoadFinalGame(GameController.GameState newState)
    {
        switch (newState)
        {
            case GameController.GameState.Store:
                finalGamePanel.SetActive(false);
                victoryGamePanel.SetActive(false);
                _bubbleCount.NEOFadeOut(duration: 0.1f);
                break;

            case GameController.GameState.Game:
                finalGamePanel.SetActive(false);
                victoryGamePanel.SetActive(false);
                _bubbleCount.NEOFadeIn(duration: 0.1f);
                break;

            case GameController.GameState.End:
                finalGamePanel.SetActive(true);
                victoryGamePanel.SetActive(false);
                _bubbleCount.NEOFadeOut(duration: 0.1f);
                break;

            case GameController.GameState.Victory:
                victoryGamePanel.SetActive(true);
                finalGamePanel.SetActive(false);
                _bubbleCount.NEOFadeOut(duration: 0.1f);
                break;
            default:
                break;
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
