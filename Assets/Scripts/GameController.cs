using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public enum GameState
    {
        Store, 
        Game
    }
    public GameState State;

    void Start()
    {
        Instance = this;
        State = GameState.Store;
    }

    public void StartGame()
    {
        State = GameState.Game;
    }
}
