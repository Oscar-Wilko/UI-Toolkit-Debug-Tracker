using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TestPlayer : MonoBehaviour
{
    public float _speed;
    private CharacterController _characterController;

    private Vector3 _vel;
    private bool _isGrounded;

    public LayerMask _groundMask;
    public float _groundDistance;
    public Transform _groundTransform;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        _isGrounded = Physics.CheckSphere(_groundTransform.position, _groundDistance, _groundMask);

        if (_isGrounded && _vel.y < 0)
        {
            _vel.y = -2f;
        }

        Vector3 move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");

        _characterController.Move(move * _speed * Time.deltaTime);

        _vel.y += -9.81f * Time.deltaTime;

        _characterController.Move(_vel * Time.deltaTime);
    }
}
