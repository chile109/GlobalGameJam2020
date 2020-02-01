using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 管理桌子的腳
 * */
public class TableLegs : MonoBehaviour
{
    public Dictionary<int, List<GameObject>> legs = new Dictionary<int, List<GameObject>>();

    int[] _namberConversion = {3, 2, 1, 0};//因為動畫中將桌腳的順序做反了，所以這是用來轉換桌腳編號用的
    void Start()
    {
        for(var i = 0;i < 4; i++)//桌腳編號
        {
            legs[i] = new List<GameObject>();
            for (var j = 1;j <= 3; j++)//每一角有三節
            {
                var leg = transform.Find(string.Format("Leg_0{0}.00{1}", j, _namberConversion[i]));
                if (leg != null)
                    legs[i].Add(leg.gameObject);
            }
        }
        var r = Random.Range(0, 4);//遊戲開始時隨意清除一角
        Destroy(legs[r][0]);
        legs[r].RemoveAt(0);
    }
    
    public bool isWin()
    {
        var count = legs[0].Count;
        for (var i = 1; i < 4; i++)
        {
            if (count != legs[i].Count)
                return false;
            count = legs[i].Count;
        }
        return true;
    }
}
