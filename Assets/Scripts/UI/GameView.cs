using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameView : MonoBehaviour
{
    public static GameView Instance;
    [SerializeField]
    private GameObject finalGamePanel;
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
            //MostrarVitoria
        }
    }

}
