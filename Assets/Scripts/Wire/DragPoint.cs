using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Joint))]
public class DragPoint : MonoBehaviour
{
    private Vector3 m_screenPoint, m_offset;
    public Rigidbody Body
    {
        get; private set;
    }
    private Joint m_joint;

    private void Awake()
    {
        Body = GetComponent<Rigidbody>();
        m_joint = GetComponent<Joint>();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        m_offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_screenPoint.z));
    }

    // Update is called once per frame
    void Update()
    {
        var mouseScreenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_screenPoint.z);
        var mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        Body.MovePosition(mouseWorldPos + m_offset);
    }
}
