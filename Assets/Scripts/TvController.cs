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
    private Renderer rend;
    private Material screenMaterial;
    private Material[] materials;

    void Start()
    {
        Sensitivity = 0.4f;
        _rotation = Vector3.zero;

        PanelR.ClickEvent += RightHitTween;
        PanelL.ClickEvent += LeftHitTween;
        _initPos = transform.position;

        InitMaterials();
        SetShaderValue(0);
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
            CalculateNoise();
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
            CalculateNoise();
        });
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), _signal.ToString());
    }

    private void InitMaterials()
    {
        rend = GetComponent<Renderer>();
        screenMaterial = Resources.Load<Material>("MAT_News");
        materials = rend.materials;
        materials[1] = screenMaterial;
        rend.materials = materials;
    }

    private void CalculateNoise()
    {
        if (_signal == 0)
        {
            SetShaderValue(1);
            return;
        }

        var value = 10.0f - Mathf.Abs(_signal);
        if (value > 0)
        {
            value = (_signal / 10f) * Mathf.Sign(_signal);
            value = 1 - value;
        }
        else
        {
            value = 0;
        }

        SetShaderValue(value);
    }

    private void SetShaderValue(float value)
    {
        rend.materials[1].SetFloat("_COLOR", value);
    }
}