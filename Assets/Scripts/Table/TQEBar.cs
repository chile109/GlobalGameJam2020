using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 畫面下方的TQE條
 * */
public class TQEBar : MonoBehaviour
{
    public AnimationCurve moveTrackX;//游標的X軸浮動座標函數
    public AnimationCurve moveTrackY;//游標的Y軸浮動座標函數
    public AnimationCurve moveTrackZ;//游標的Z軸浮動座標函數

    public GameObject badBar;//壞區域條
    public GameObject goodBar;//好區域條
    public GameObject cursor;//游標
    public bool isPause;

    protected float totleTimeX = 0;//游標的X軸浮動座標函數總時間
    protected float totleTimeY = 0;//游標的Y軸浮動座標函數總時間
    protected float totleTimeZ = 0;//游標的Z軸浮動座標函數總時間
    protected Vector3 cursorOriginalPosition;//游標原始座標
    protected float runtime;
    void Start()
    {
        totleTimeX = moveTrackX[moveTrackX.length - 1].time;
        totleTimeY = moveTrackY[moveTrackY.length - 1].time;
        totleTimeZ = moveTrackZ[moveTrackZ.length - 1].time;
        cursorOriginalPosition = transform.localPosition;
        newRange();
    }

    void Update()
    {
        if (!isPause)
        {
            runtime += Time.deltaTime;
            cursor.transform.localPosition = new Vector3(moveTrackX.Evaluate(runtime % totleTimeX) + cursorOriginalPosition.x, moveTrackY.Evaluate(runtime % totleTimeY) + cursorOriginalPosition.y, moveTrackZ.Evaluate(runtime % totleTimeZ) + cursorOriginalPosition.z);
        }
    }

    /**
     * 取得當下結果
     * */
    public bool getResult()
    {
        if (
                goodBar.transform.localPosition.x - goodBar.GetComponent<RectTransform>().rect.width / 2 <= cursor.transform.localPosition.x
                &&
                cursor.transform.localPosition.x <= goodBar.transform.localPosition.x + goodBar.GetComponent<RectTransform>().rect.width / 2
        )
        {
            newRange();
            return true;
        }
        else
            return false;
    }


    /**
     * 更新好區域的位置
     * */
    void newRange()
    {
        var goodBarWidth = badBar.GetComponent<RectTransform>().rect.width / Random.Range(5f, 10f);
        goodBar.transform.localPosition = new Vector2(Random.Range(0, badBar.GetComponent<RectTransform>().rect.width / 2 - goodBarWidth / 2) * (Random.Range(0, 2) == 0 ? 1 : -1), goodBar.transform.localPosition.y);
        var rect = goodBar.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(goodBarWidth, rect.sizeDelta.y);
    }
}
