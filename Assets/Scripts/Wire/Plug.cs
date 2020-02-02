using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plug : Wire
{
    public Transform SocketHolder;

    private bool tmp = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Socket")
        {
            if (!tmp)
            {
                tmp = true;
                return;
            }
            m_callback?.Invoke();
            m_puller = Instantiate(m_pullerPrefab, transform.position, Quaternion.identity);
            m_puller.Body.mass = 500f;
            m_puller.Body.isKinematic = true;
            m_puller.ManualPullTo(SocketHolder.position);
            m_collider.enabled = false;
            m_joint.connectedBody = m_puller.Body;
            SetWireGravity(false);
            WireSceneController.I.WinStage();
        }
    }
}
