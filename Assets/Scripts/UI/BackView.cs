using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BackView : MonoBehaviour
{
    private float duration = 0.3f;

    private void OnEnable()
    {
        GameController.OnGameStateChanged += HideOrShow;
    }


    private void OnDisable()
    {
        GameController.OnGameStateChanged -= HideOrShow;
    }
    private void HideOrShow(GameController.GameState state)
    {
        RectTransform rect = transform as RectTransform;

        if (state == GameController.GameState.Game)
        {
            rect.DOAnchorPosY(0, duration).SetEase(Ease.InOutQuad);
        }
        else
            rect.DOAnchorPosY(Screen.height, duration).SetEase(Ease.InOutQuad);
    }
}
