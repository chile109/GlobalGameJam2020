using UnityEngine;
using System.Collections;

public class WireSceneController : MonoBehaviour
{
    public static WireSceneController I;
    public bool hasWon = false;

    private void Start()
    {
        I = this;
    }
}
