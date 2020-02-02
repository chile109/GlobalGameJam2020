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
        show(Quaternion.Euler(0, 0, 0));
    }
    public static void show(Quaternion quaternion)
    {
        var o = Instantiate(winParticle);
        o.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(1920 / 2, 0, 8f));
        var s = o.GetComponent<ParticleSystem>().shape;
        s.rotation = quaternion.eulerAngles;
        Destroy(o, 1.5f);
    }
}
