using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Joint))]
public class DragPoint : MonoBehaviour
{
    private Vector3 m_screenPoint, m_offset; 
    private Vector3 m_prevPoint;
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
        m_prevPoint = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var mouseScreenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_screenPoint.z);
        var mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        var pos = mouseWorldPos + m_offset;
        Body.velocity = Vector3.ClampMagnitude((pos - m_prevPoint) / Time.fixedDeltaTime, 100f);
        m_prevPoint = pos;
    }
}
