using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TimerDisplay : MonoBehaviour
{
    [SerializeField]
    private Slider timer;

    [SerializeField]
    private Image Progress;

    public void StartCountdown(float duration, Action timeOutCallback)
    {
        Sequence mySequence=DOTween.Sequence(); 
        mySequence.Append(timer.DOValue(0, 10f));
        mySequence.Insert( 2f, Progress.DOColor(Color.red, 3) );
        mySequence.AppendCallback(() =>
        {
            timeOutCallback.Invoke();
        });
    }
}
