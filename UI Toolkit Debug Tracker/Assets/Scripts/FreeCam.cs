using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCam : MonoBehaviour
{
    private DebugTabs _tabs;
    private DebugManager _manager;

    public float _moveSpeed;
    public float _fastMoveSpeed;
    public float _minRot;
    public float _maxRot;
    public float _xSens;
    public float _zSens;
    public float _xRot;
    public float _yRot;

    private void Awake()
    {
        _tabs = FindObjectOfType<DebugTabs>();
        _manager = FindObjectOfType<DebugManager>();
    }

    void Update()
    {
        if (!_manager._debugMode)
            SnapBack();
        else if (_tabs._selectedTab == DebugTabs.Tabs.None)
        {
            Turn();
            Move();
        }
    }

    private void SnapBack()
    {
        transform.localEulerAngles = Vector3.zero;
        transform.localPosition = Vector3.zero;
        _xRot = transform.rotation.eulerAngles.x;
        _yRot = transform.rotation.eulerAngles.y;
    }

    private void Move()
    {
        Vector3 move = transform.right * Input.GetAxis("Horizontal")
                     + transform.forward * Input.GetAxis("Vertical");
        transform.position += move * Time.deltaTime * (Input.GetKey(KeyCode.LeftShift) ? _fastMoveSpeed : _moveSpeed);
    }

    private void Turn()
    {
        _xRot -= Input.GetAxis("Mouse Y") * _zSens;
        while (_xRot > 180)
            _xRot -= 360;
        _xRot = Mathf.Clamp(_xRot, _minRot, _maxRot);
        _yRot += Input.GetAxis("Mouse X") * _xSens;
        transform.rotation = Quaternion.Euler(_xRot, _yRot, 0);
    }

    public void LookToPosition(Vector3 pos)
    {
        transform.LookAt(pos);
        _xRot = transform.rotation.eulerAngles.x;
        _yRot = transform.rotation.eulerAngles.y;
    }

    public void LookToPosition(float[] pos)
    {
        if (pos.Length < 3)
            return;
        _manager._debugMode = true;
        LookToPosition(new Vector3(pos[0], pos[1], pos[2]));
    }
}
