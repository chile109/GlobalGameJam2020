using UnityEngine;
using System.Collections;

public class WireSceneController : MonoBehaviour
{
    public static WireSceneController I;
    public bool HasWon = false;
    public Transform CameraPos;

    private void Awake()
    {
        if (!Camera.main.enabled)
            Camera.main.enabled = true;
        Camera.main.transform.SetPositionAndRotation(CameraPos.position, CameraPos.rotation);
    }

    private void Start()
    {
        I = this;
        if (!Camera.main.enabled)
            Camera.main.enabled = true;
        Camera.main.transform.SetPositionAndRotation(CameraPos.position, CameraPos.rotation);
    }

    public void WinStage()
    {
        HasWon = true;
        WinEffect.show(Quaternion.Euler(90, 0, 0));
        StartCoroutine(NextStage());
    }

    public IEnumerator NextStage()
    {
        yield return new WaitForSeconds(1f);
        UserInterfaceController.Instance.JumpStage3();
    }
}
