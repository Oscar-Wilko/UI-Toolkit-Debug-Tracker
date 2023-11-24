using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCam : MonoBehaviour
{
    // References
    private DebugTabs _tabs;
    private DebugManager _manager;
    private DebugCustomiser _custom;

    // Variables
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
        _custom = FindObjectOfType<DebugCustomiser>();
    }

    void Update()
    {
        if (!_tabs || !_manager || !_custom)
            return;
        if (!_manager._debugMode)
            SnapBack();
        else if (_tabs._selectedTab == DebugTabs.Tabs.None && !Input.GetKey(_custom.data._panKey))
        {
            Turn();
            Move();
        }
    }

    /// <summary>
    /// Reset back to zero in position and angle
    /// </summary>
    private void SnapBack()
    {
        transform.localEulerAngles = Vector3.zero;
        transform.localPosition = Vector3.zero;
        _xRot = transform.rotation.eulerAngles.x;
        _yRot = transform.rotation.eulerAngles.y;
    }

    /// <summary>
    /// Move camera based on player input
    /// </summary>
    private void Move()
    {
        Vector3 move = transform.right * Input.GetAxis("Horizontal")
                     + transform.forward * Input.GetAxis("Vertical");
        transform.position += move * Time.deltaTime * (Input.GetKey(_custom.data._fastKey) ? _fastMoveSpeed : _moveSpeed);
    }

    /// <summary>
    /// Turn camera based on player input
    /// </summary>
    private void Turn()
    {
        _xRot -= Input.GetAxis("Mouse Y") * _zSens;
        while (_xRot > 180)
            _xRot -= 360;
        _xRot = Mathf.Clamp(_xRot, _minRot, _maxRot);
        _yRot += Input.GetAxis("Mouse X") * _xSens;
        transform.rotation = Quaternion.Euler(_xRot, _yRot, 0);
    }

    /// <summary>
    /// Look to position given by vector
    /// </summary>
    /// <param name="pos">Vector3 of position</param>
    public void LookToPosition(Vector3 pos)
    {
        transform.LookAt(pos);
        _xRot = transform.rotation.eulerAngles.x;
        _yRot = transform.rotation.eulerAngles.y;
    }

    /// <summary>
    /// Look to position given by float array
    /// </summary>
    /// <param name="pos">Float[] of position</param>
    public void LookToPosition(float[] pos)
    {
        if (pos.Length < 3)
            return;
        _manager._debugMode = true;
        LookToPosition(new Vector3(pos[0], pos[1], pos[2]));
    }
}
