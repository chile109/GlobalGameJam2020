using DG.Tweening;
using UnityEngine;

public class TvController : MonoBehaviour
{
    public SidePanelController PanelR;
    public SidePanelController PanelL;
    public float Sensitivity;

    private Vector3 _initPos;
    private Vector3 _mouseReference;
    private Vector3 _mouseOffset;
    private Vector3 _rotation;
    private bool _isRotating;
    private int _signal = 9;

    void Start()
    {
        Sensitivity = 0.4f;
        _rotation = Vector3.zero;

        PanelR.ClickEvent += RightHitTween;
        PanelL.ClickEvent += LeftHitTween;
        _initPos = transform.position;

        //_signal = UnityEngine.Random.Range(-10, 10);
    }

    void Update()
    {
        if (_signal == 0)
            OnWin();

        if (_isRotating)
        {
            _mouseOffset = (Input.mousePosition - _mouseReference);
            _rotation.y = -_mouseOffset.x * Sensitivity;

            if (CanRotate())
                transform.Rotate(_rotation);

            _mouseReference = Input.mousePosition;
        }
    }

    private bool CanRotate()
    {
        var currentRot = (transform.eulerAngles.y > 180) ? transform.eulerAngles.y - 360 : transform.eulerAngles.y;
        if (Mathf.Abs(currentRot + _rotation.y) > 60f)
            return false;

        return true;
    }

    private void OnWin()
    {
        Debug.Log("Win");
    }

    void OnMouseDown()
    {
        _isRotating = true;

        _mouseReference = Input.mousePosition;
    }

    void OnMouseUp()
    {
        _isRotating = false;
    }

    void RightHitTween()
    {
        var doShake = transform.DOShakePosition(0.5f, 0.3f, 20);
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(doShake);
        mySequence.AppendCallback(() =>
        {
            transform.position = _initPos;
            _signal += 5;
        });
    }

    void LeftHitTween()
    {
        var doShake = transform.DOPunchPosition(transform.right * 0.5f, 0.3f, 20, 0.3f);
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(doShake);
        mySequence.AppendCallback(() =>
        {
            transform.position = _initPos;
            _signal -= 6;
        });
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), _signal.ToString());
    }
}