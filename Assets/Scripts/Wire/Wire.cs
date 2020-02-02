using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class Wire : MonoBehaviour
{
    private static Action m_callback;
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
    [SerializeField]
    private float m_maxAngle;
    private bool m_isFreeFall = false;

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
        //m_joint.axis = Camera.main.transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseDown()
    {
        m_callback?.Invoke();
        m_puller = Instantiate(m_pullerPrefab, transform.position, Quaternion.identity);
        //m_puller.DropCallback += StartRecursiveInverseBreak;
        StartRecursiveInverseConnect(m_puller.Body);
        WireRigidbody.useGravity = false;
        m_callback = StartRecursiveInverseBreak;
    }

    public void OnMouseUp()
    {
        m_isFreeFall = true;
        m_puller.Release();
        //StartRecursiveInverseBreak();
        WireRigidbody.useGravity = true;
    }

    public void OnMouseDrag()
    {
        //m_joint.targetPosition = m_puller.transform.position;
    }

    public void StartRecursiveInverseConnect(Rigidbody rb)
    {
        if (m_joint.connectedBody != null)
        {
            m_parentWire.RecursiveInverseConnect(this.WireRigidbody, JointRotation());
        }
        ResetJoint(Vector3.zero, rb, null); //add anywhere
    }

    public void StartRecursiveInverseBreak()
    {
        m_isFreeFall = false;
        if (m_puller != null) Destroy(m_puller.gameObject);
        var angles = m_parentWire?.RecursiveInverseBreak();
        ResetJoint(m_anchorValue, m_parentWire?.WireRigidbody, angles);
    }

    public void RecursiveInverseConnect(Rigidbody rb, Vector3 curAngles)
    {
        if (m_joint.connectedBody != null)
        {
            m_parentWire.RecursiveInverseConnect(this.WireRigidbody, JointRotation());
        }
        ResetJoint(-m_anchorValue, rb, curAngles);
    }

    public Vector3? RecursiveInverseBreak()
    {
        var angles = m_parentWire?.RecursiveInverseBreak();
        Vector3? rot = null;
        if (m_joint.connectedBody != null)
        {
            rot = JointRotation();
        }
        ResetJoint(m_anchorValue, m_parentWire?.WireRigidbody, angles);
        return rot;
    }

    public void ResetJoint(Vector3 anchor, Rigidbody connectedBody, Vector3? curAngles)
    {
        m_joint.anchor = anchor;
        m_joint.connectedBody = connectedBody;
        if (m_joint.connectedBody != null && curAngles.HasValue)
        {
            m_joint.angularXMotion = ConfigurableJointMotion.Limited;

            //Debug.LogFormat("gameObject {0}: curAngles is {1}", gameObject.name, curAngles.ToString());
            var reverseAngle = curAngles.Value;
            m_joint.lowAngularXLimit = new SoftJointLimit()
            {
                limit = reverseAngle.x - m_maxAngle,
                bounciness = 0,
                contactDistance = 0
            };
            m_joint.highAngularXLimit = new SoftJointLimit()
            {
                limit = reverseAngle.x + m_maxAngle,
                bounciness = 0,
                contactDistance = 0
            };
            /*
            m_joint.angularYLimit = new SoftJointLimit()
            {
                limit = reverseAngle.x + m_maxAngle,
                bounciness = 0,
                contactDistance = 0
            };
            m_joint.angularZLimit = new SoftJointLimit()
            {
                limit = reverseAngle.y + m_maxAngle,
                bounciness = 0,
                contactDistance = 0
            };*/
        }
        else
        {
            m_joint.angularXMotion = ConfigurableJointMotion.Free;
        }
    }

    public Quaternion GetJointRotation()
    {
        return (Quaternion.FromToRotation(m_joint.axis, m_joint.connectedBody.transform.rotation.eulerAngles));
    }

    public float To180(float v)
    {
        if (v > 180)
        {
            v = v - 360;
        }
        return v;
    }
    Vector3 JointRotation()
    {
        Quaternion jointBasis = Quaternion.LookRotation(m_joint.secondaryAxis, Vector3.Cross(m_joint.axis, m_joint.secondaryAxis));
        Quaternion jointBasisInverse = Quaternion.Inverse(jointBasis);
        var rotation = (jointBasisInverse * Quaternion.Inverse(m_joint.connectedBody.rotation) * WireRigidbody.transform.rotation * jointBasis).eulerAngles;
        return new Vector3(To180(rotation.x), To180(rotation.z), To180(rotation.y));
    }
}
