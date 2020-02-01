using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TvController : MonoBehaviour
{
    public float Sensitivity;
    private Vector3 _mouseReference;
    private Vector3 _mouseOffset;
    public Vector3 _rotation;
    private bool _isRotating;

    void Start()
    {
        Sensitivity = 0.4f;
        _rotation = Vector3.zero;
    }

    void Update()
    {
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

    void OnMouseDown()
    {
        _isRotating = true;

        _mouseReference = Input.mousePosition;
    }

    void OnMouseUp()
    {
        _isRotating = false;
    }
}