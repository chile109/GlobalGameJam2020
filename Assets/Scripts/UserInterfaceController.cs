using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UserInterfaceController : MonoBehaviour
{
    public Image CrossEfect;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CrossScene();
        }
    }

    public void CrossScene()
    {
        var fadeOut = CrossEfect.DOFade(1, 1f);
        var fadeIn = CrossEfect.DOFade(0, 1f);
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(fadeOut);
        mySequence.Append(fadeIn);
        
    }
}
