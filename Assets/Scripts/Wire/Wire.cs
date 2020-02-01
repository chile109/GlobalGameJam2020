using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class Wire : MonoBehaviour
{
    private Vector3 m_screenPoint, m_offset;
    private Collider m_collider;
    public Rigidbody WireRigidbody
    {
        get; private set;
    }
    private ConfigurableJoint m_joint;
    [SerializeField]
    private Wire m_parentWire;
    private Rigidbody m_jointConnected;
    [SerializeField]
    private DragPoint m_pullerPrefab;
    private DragPoint m_puller;
    [SerializeField]
    private Vector3 m_anchorValue;

    private void Awake()
    {
        m_collider = GetComponent<Collider>();
        WireRigidbody = GetComponent<Rigidbody>();
        m_joint = GetComponent<ConfigurableJoint>();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_joint.connectedBody = m_parentWire?.WireRigidbody;
        m_screenPoint = Camera.main.WorldToScreenPoint(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseDown()
    {
        m_puller = Instantiate(m_pullerPrefab, transform.position, Quaternion.identity);
        StartRecursiveInverseConnect(m_puller.Body);
    }

    public void OnMouseUp()
    {
        Destroy(m_puller.gameObject);
        EndRecursiveInverseConnect();
    }

    public void OnMouseDrag()
    {
        //m_joint.targetPosition = m_puller.transform.position;
    }

    public void StartRecursiveInverseConnect(Rigidbody rb)
    {
        if (m_joint.connectedBody != null)
        {
            m_parentWire.RecursiveInverseConnect(this.WireRigidbody);
        }
        m_joint.anchor = Vector3.zero;
        m_joint.connectedBody = rb;
    }

    public void EndRecursiveInverseConnect()
    {
        m_joint.anchor = m_anchorValue;
        m_joint.connectedBody = m_parentWire?.WireRigidbody;
        m_parentWire?.EndRecursiveInverseConnect();
    }

    public void RecursiveInverseConnect(Rigidbody rb)
    {
        if (m_joint.connectedBody != null)
        {
            m_parentWire.RecursiveInverseConnect(this.WireRigidbody);
        }
        m_joint.anchor = -m_anchorValue;
        m_joint.connectedBody = rb;
    }
}
