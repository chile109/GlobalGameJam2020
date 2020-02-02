using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinEffect : MonoBehaviour
{
    private static GameObject winParticle;
    static WinEffect()
    {
        winParticle = Resources.Load<GameObject>("WinEffect");
        Debug.Log(winParticle);
    }

    public static void show()
    {
        var o = Instantiate(winParticle);
        o.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(1920 / 2, 0, 8f));
        Destroy(o, 1.5f);
    }
}
