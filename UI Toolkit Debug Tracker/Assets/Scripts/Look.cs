using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Look : MonoBehaviour
{
    public float _minRot;
    public float _maxRot;
    public float _xSens;
    public float _zSens;
    public Transform _body;
    private float _xRot;

    private void Update()
    {
        _xRot -= Input.GetAxis("Mouse Y") * _zSens;
        _xRot = Mathf.Clamp(_xRot, _minRot, _maxRot);
        transform.localRotation = Quaternion.Euler(_xRot, 0, 0);
        _body.Rotate(Vector3.up * Input.GetAxis("Mouse X") * _xSens);
    }
}
