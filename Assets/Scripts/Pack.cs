using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TsuyoshiLibrary
{
    public class Pack : MonoBehaviour
    {
        [SerializeField] float _speed = 10; 
        [SerializeField] Vector3 _dir;
        Rigidbody _rb;
        Vector3 _previousPos;

        private void Start()
        {
            TryGetComponent(out _rb);
        }

        private void Update()
        {
        }

        private void FixedUpdate()
        {
            CalculateDir();
            _rb.velocity = _dir * _speed;
        }

        private void CalculateDir()
        {
            _dir = (transform.position - _previousPos).normalized;
            _previousPos = transform.position;
        }

        private void OnCollisionEnter(Collision collision)
        {

            if (collision.gameObject.GetComponent<Goal>())
            {
                transform.position = Vector3.zero;
                _rb.velocity = Vector3.zero;

            }
            else
            {
                // 入射ベクトル（速度）
                var inDirection = _rb.velocity;
                // 法線ベクトル
                var inNormal = collision.contacts[0].normal;
                // 反射ベクトル（速度）
                var result = Vector3.Reflect(inDirection, inNormal);
                _dir = result.normalized;
            }
        }
    }
}
