using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableLegs : MonoBehaviour
{
    public Dictionary<int, List<GameObject>> legs = new Dictionary<int, List<GameObject>>();

    int[] _namberConversion = {3, 2, 1, 0};
    void Start()
    {
        for(var i = 0;i < 4; i++)
        {
            legs[i] = new List<GameObject>();
            for (var j = 1;j <= 3; j++)
            {
                var leg = transform.Find(string.Format("Leg_0{0}.00{1}", j, _namberConversion[i]));
                if (leg != null)
                    legs[i].Add(leg.gameObject);
            }
        }
    }
    
    public bool isWin()
    {
        var count = 0;
        for (var i = 0; i < 4; i++)
        {
            count += legs[i].Count;
        }
        return count % 4 == 0;
    }
}
