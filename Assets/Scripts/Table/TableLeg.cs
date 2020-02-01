using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 桌腳的每一個節
 * */
public class TableLeg : MonoBehaviour
{
    /*private void Update()
    {
        var previousObject = table;
        for(var i = 0; i < gameObject.transform.childCount; i++)
        {
            var obj = gameObject.transform.GetChild(i).gameObject;
            if(
                obj.transform.transform.position.y + (obj.GetComponent<Renderer>().bounds.size.y / 2)
                !=
                previousObject.transform.transform.position.y - (previousObject.GetComponent<Renderer>().bounds.size.y / 2)
            )
            {
                obj.transform.transform.position += new Vector3(0, +1 * Time.deltaTime, 0);
            }
            previousObject = obj;
        }
    }*/
    public void toDestroy(Quaternion hammerRotation)
    {
        float angle = hammerRotation.eulerAngles.y - 90;
        if (angle < 0)
            angle += 360f;
        var _in1or3quadrant = in1or3quadrant(angle);
        angle *= Mathf.Deg2Rad;
        var rigidbody = gameObject.GetComponent<Rigidbody>();
        rigidbody.useGravity = true;
        var z = Mathf.Sin(angle) * 10f * (_in1or3quadrant ? -1 : 1);
        var x = Mathf.Cos(angle) * 10f * (_in1or3quadrant ? -1 : 1);
        rigidbody.AddForce(new Vector3(x, Random.Range(-30, 30), z), ForceMode.Impulse);
        Destroy(gameObject, 0.15f);
    }

    //判斷角度是否在第一或第四象限
    bool in1or3quadrant(float angle) => (90 <= angle && angle < 180 || 270 < angle && angle <= 360);
}
