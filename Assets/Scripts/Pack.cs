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
            CalculateDir();
            
        }

        private void FixedUpdate()
        {
            _rb.velocity = _dir * _speed;
        }

        private void CalculateDir()
        {
            if (transform.position == _previousPos) return;
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
                float dot = Vector3.Dot(inDirection, inNormal);
                Vector3 reflection = inDirection - 2f * dot * inNormal;
                _dir = reflection.normalized;

                Debug.Log($"入射ベクトル：{inDirection}法線ベクトル:{inNormal}反射ベクトル：{reflection}");
            }
        }
    }
}
