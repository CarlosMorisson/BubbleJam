using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NEO.UiAnimations;
using DG.Tweening;
public class LogoView : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RectTransform logo = transform.GetChild(0).transform as RectTransform;
        CanvasGroup cg = GetComponent<CanvasGroup>();
        cg.alpha = 1;

        logo.NEOBigSlideDownIn(duration: 1f);
        cg.DOFade(endValue: 0, duration: 2f).SetEase(Ease.InExpo);
    }
}
