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
    [SerializeField]
    private float m_maxAngle;

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
        ResetJoint(Vector3.zero, rb);
    }

    public void EndRecursiveInverseConnect()
    {
        ResetJoint(m_anchorValue, m_parentWire?.WireRigidbody);
        m_parentWire?.EndRecursiveInverseConnect();
    }

    public void RecursiveInverseConnect(Rigidbody rb)
    {
        if (m_joint.connectedBody != null)
        {
            m_parentWire.RecursiveInverseConnect(this.WireRigidbody);
        }
        ResetJoint(-m_anchorValue, rb);
    }

    public void ResetJoint(Vector3 anchor, Rigidbody connectedBody)
    {
        if (m_joint.connectedBody != null)
        {
            Debug.Log("jointRotation: "+GetJointRotation());
            var eulerAngless = GetJointRotation().eulerAngles;
            Debug.Log("eulerAngless: " + eulerAngless);
            var eulerAngles = JointRotation(m_joint);
            Debug.Log("eulerAngles: "+eulerAngles);
            /*
            m_joint.lowAngularXLimit = new SoftJointLimit()
            {
                limit = eulerAngles.z - m_maxAngle,
                bounciness = 0,
                contactDistance = 0
            };
            m_joint.highAngularXLimit = new SoftJointLimit()
            {
                limit = eulerAngles.z + m_maxAngle,
                bounciness = 0,
                contactDistance = 0
            };
            m_joint.angularYLimit = new SoftJointLimit()
            {
                limit = eulerAngles.x + m_maxAngle,
                bounciness = 0,
                contactDistance = 0
            };
            m_joint.angularZLimit = new SoftJointLimit()
            {
                limit = eulerAngles.y + m_maxAngle,
                bounciness = 0,
                contactDistance = 0
            };*/
            Debug.Log(eulerAngles.z);
            Debug.Log(m_joint.angularZLimit);
        }
        m_joint.anchor = anchor;
        m_joint.connectedBody = connectedBody;
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
    Vector3 JointRotation(ConfigurableJoint joint)
    {
        Quaternion jointBasis = Quaternion.LookRotation(joint.secondaryAxis, Vector3.Cross(joint.axis, joint.secondaryAxis));
        Quaternion jointBasisInverse = Quaternion.Inverse(jointBasis);
        var rotation = (jointBasisInverse * Quaternion.Inverse(joint.connectedBody.rotation) * joint.GetComponent<Rigidbody>().transform.rotation * jointBasis).eulerAngles;
        return new Vector3(To180(rotation.x), To180(rotation.z), To180(rotation.y));
    }
}
