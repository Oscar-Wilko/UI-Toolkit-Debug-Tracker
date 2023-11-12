using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TestPlayer : MonoBehaviour
{
    private CharacterController _characterController;
    private Rigidbody _rb;
    private DebugTabs _tabs;
    private DebugManager _manager;

    private Vector3 _vel;
    private bool _isGrounded;

    public LayerMask _groundMask;
    public float _groundDistance;
    public Transform _groundTransform;
    public float _speed;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _characterController = GetComponent<CharacterController>();
        _tabs= FindObjectOfType<DebugTabs>();
        _manager = FindObjectOfType<DebugManager>();
    }

    void Update()
    {
        _characterController.enabled = CanMove();
        _rb.isKinematic = CanMove();
        if (CanMove()) 
        {
            CheckGrounded();
            Move();
            Fall();
        }
    }

    private bool CanMove()
    {
        if (_tabs._selectedTab != DebugTabs.Tabs.None)
            return false;
        if (_manager._debugMode)
            return false;
        return true;
    }

    /// <summary>
    /// Update grounded variable
    /// </summary>
    private void CheckGrounded()
    {
        _isGrounded = Physics.CheckSphere(_groundTransform.position, _groundDistance, _groundMask);
        if (_isGrounded && _vel.y < 0) _vel.y = -2f;
    }

    /// <summary>
    /// Move based on input
    /// </summary>
    private void Move()
    {
        Vector3 move = transform.right * Input.GetAxis("Horizontal") 
                     + transform.forward * Input.GetAxis("Vertical");

        _characterController.Move(move * _speed * Time.deltaTime);
    }

    /// <summary>
    /// Make player fall
    /// </summary>
    private void Fall()
    {
        _vel.y += -9.81f * Time.deltaTime;
        _characterController.Move(_vel * Time.deltaTime);
    }
}
