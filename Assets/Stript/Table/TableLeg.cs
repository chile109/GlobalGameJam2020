using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public void toDestroy()
    {
        var rigidbody = gameObject.GetComponent<Rigidbody>();
        rigidbody.useGravity = true;
        rigidbody.AddForce(new Vector3(30f, 0, 0), ForceMode.Impulse);
        Destroy(gameObject, 0.15f);
    }
}
