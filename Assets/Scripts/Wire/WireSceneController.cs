using UnityEngine;
using System.Collections;

public class WireSceneController : MonoBehaviour
{
    public static WireSceneController I;
    public bool HasWon = false;

    private void Start()
    {
        I = this;
    }

    public void WinStage()
    {
        HasWon = true;
        StartCoroutine(NextStage());
    }

    public IEnumerator NextStage()
    {
        yield return new WaitForSeconds(1f);
        UserInterfaceController.Instance.JumpStage3();
    }
}
