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

        private void Start()
        {
            TryGetComponent(out _rb);
        }

        private void FixedUpdate()
        {
            _rb.velocity = _dir * _speed;
        }

        void CalculateDir(Vector3 hitObj)
        {
            _dir = (transform.position - hitObj);
        }

        private void OnCollisionEnter(Collision collision)
        {

            if (collision.gameObject.GetComponent<Goal>())
            {
                transform.position = Vector3.zero;
                _dir = Vector3.zero;

            }
            else if (collision.gameObject.GetComponent<Player>())
            {
                CalculateDir(collision.gameObject.transform.position);
            }
            else
            {
                var inDirection = _rb.velocity;
                var inNormal = collision.contacts[0].normal;
                float dot = Vector3.Dot(inDirection, inNormal);
                Vector3 reflection = inDirection - 2f * dot * inNormal;
                _dir = reflection.normalized;

                Debug.Log($"入射ベクトル：{inDirection}法線ベクトル:{inNormal}反射ベクトル：{reflection}");
            }
        }
    }
}
