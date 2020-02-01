using System;
using UnityEngine;

public class SidePanelController : MonoBehaviour
{
    public Action ClickEvent;

    void OnMouseDown()
    {
        ClickEvent.Invoke();
    }
}