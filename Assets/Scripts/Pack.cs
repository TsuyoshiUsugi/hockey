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

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.GetComponent<Goal>())
            {
                transform.position = Vector3.zero;
                _dir = Vector3.zero;

            }
            else if (collision.gameObject.GetComponent<Player>() || collision.gameObject.GetComponent<Pack>())
            {
                _dir = (transform.position - collision.gameObject.transform.position).normalized;
                _rb.velocity = _dir * _speed;
            }
            else
            {
                var inDirection = _dir;
                var inNormal = collision.contacts[0].normal;
                float dot = Vector3.Dot(inDirection, inNormal);
                Vector3 reflection = inDirection - 2f * dot * inNormal;
                _dir = reflection.normalized;
                _rb.velocity = _dir * _speed;

                Debug.Log($"入射ベクトル：{inDirection}法線ベクトル:{inNormal}反射ベクトル：{reflection}");
            }
        }
    }
}
