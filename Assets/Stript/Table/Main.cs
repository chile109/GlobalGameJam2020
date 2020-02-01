using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public GameObject tableLeg;
    void Start()
    {
        Physics.gravity = new Vector3(Physics.gravity.x, -Physics.gravity.y, Physics.gravity.z);
        for(var i = 0;i < tableLeg.transform.childCount; i++)
        {
            tableLeg.transform.GetChild(i).GetComponent<Rigidbody>().useGravity = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
