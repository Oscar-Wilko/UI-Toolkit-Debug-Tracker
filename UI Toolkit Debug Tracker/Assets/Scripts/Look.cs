using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Look : MonoBehaviour
{
    public Transform _body;
    public float _minRot;
    public float _maxRot;
    public float _xSens;
    public float _zSens;
    private float _xRot;

    private DebugTabs _tabs;
    private DebugManager _manager;

    private void Awake()
    {
        _tabs = FindObjectOfType<DebugTabs>();
        _manager = FindObjectOfType<DebugManager>();
    }

    private void Update()
    {
        if (CanTurn()) 
            Turn();
    }

    private bool CanTurn()
    {
        if (!_tabs || !_manager)
            return true;
        if (_tabs._selectedTab != DebugTabs.Tabs.None)
            return false;
        if (_manager._debugMode)
            return false;
        return true;
    }

    /// <summary>
    /// Turn camera with mouse input
    /// </summary>
    private void Turn()
    {
        _xRot -= Input.GetAxis("Mouse Y") * _zSens;
        _xRot = Mathf.Clamp(_xRot, _minRot, _maxRot);
        transform.localRotation = Quaternion.Euler(_xRot, 0, 0);
        _body.Rotate(Vector3.up * Input.GetAxis("Mouse X") * _xSens);
    }
}
