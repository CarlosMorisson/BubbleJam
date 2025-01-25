using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public enum GameState
    {
        Store, 
        Game,
        End,
        Victory
    }
    public GameState State;

    public static Action<GameState> OnGameStateChanged;

    void Awake()
    {
        Instance = this;
        State = GameState.Store;
    }
    public void StartGame()
    {
        GetGameState(GameState.Game);
    }
    public void StoreGame()
    {
        GetGameState(GameState.Store);
    }

    public void GetGameState(GameState state)
    {
        State = state;
        OnGameStateChanged?.Invoke(State);
    }

}
