using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TsuyoshiLibrary
{
    public class Player : MonoBehaviour
    {
        [SerializeField] float _speed = 10;
        [SerializeField] string _hInput = "Horizontal";
        [SerializeField] string _vInput = "Vertical";
        float _h;
        float _v;
        Rigidbody _rigidbody;

        private void Start()
        {
            TryGetComponent(out _rigidbody);
        }

        private void Update()
        {
            _h = Input.GetAxisRaw(_hInput);
            _v = Input.GetAxisRaw(_vInput);
        }

        private void FixedUpdate()
        {
            _rigidbody.AddForce(new Vector3(_h, 0, _v) * _speed);
        }
    }
}
