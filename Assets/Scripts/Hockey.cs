using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TsuyoshiLibrary
{
    public class Hockey : MonoBehaviour
    {
        Rigidbody _rb;

        private void Start()
        {
            TryGetComponent(out _rb);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.gameObject.TryGetComponent(out Goal goal))
            {
                transform.position = Vector3.zero;
                _rb.velocity = Vector3.zero;

            }
        }
    }
}
