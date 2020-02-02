using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class DragPoint : MonoBehaviour
{
    private bool m_manual = false;
    private Vector3 m_dest;
    public Action DropCallback;
    private Vector3 m_screenPoint, m_offset, m_liftOffset;
    private Vector3 m_prevPoint;
    private bool m_holding = true;
    public Rigidbody Body
    {
        get; private set;
    }
    public float LiftOffsetValue;
    [SerializeField]
    private float m_dragForce;

    private void Awake()
    {
        Body = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        m_offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_screenPoint.z));
        m_offset += Camera.main.transform.forward * -LiftOffsetValue;
        m_prevPoint = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (m_manual)
        {
            Body.MovePosition(m_dest);
            return;
        }
        if (!m_holding)
            return;
        var mouseScreenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_screenPoint.z);
        var mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        var pos = mouseWorldPos + m_offset;
        //Debug.Log(((pos - m_prevPoint) / Time.fixedDeltaTime).magnitude);
        Body.velocity = Vector3.ClampMagnitude((pos - m_prevPoint) * m_dragForce / Time.fixedDeltaTime, 100f);
        m_prevPoint = pos;
    }

    public void Release()
    {
        m_holding = false;
        Body.mass = 10f;
        Body.useGravity = true;
    }

    public void ManualPullTo(Vector3 pos)
    {
        m_manual = true;
        m_dest = pos;
    }
}
