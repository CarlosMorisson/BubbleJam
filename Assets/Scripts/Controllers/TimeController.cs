using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public static Action OnKickTimeStop;
    private bool isCoroutineRunning = false;

    [SerializeField]
    [Range(0, 1)]
    private float timeToWait;

    private void OnEnable()
    {
        OnKickTimeStop += KickStopTime;
    }

    private void OnDisable()
    {
        OnKickTimeStop -= KickStopTime;
    }
    void KickStopTime()
    {
        if (isCoroutineRunning)
            StartCoroutine(KickTimeStop());
    }


    IEnumerator KickTimeStop()
    {
        isCoroutineRunning=true;    
        Time.timeScale = 0;
        yield return new WaitForSeconds(timeToWait);
        Time.timeScale = 1;
        isCoroutineRunning = true;

    }

}
