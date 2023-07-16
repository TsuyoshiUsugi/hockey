using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TsuyoshiLibrary
{
    /// <summary>
    /// パックのscript
    /// ・スピードを設定できる
    /// ・ゴールにあたると真ん中に移動
    /// ・playerとpackにあたると円の中心点の差分をdirにいれて進む
    /// </summary>
    public class Pack : MonoBehaviour
    {
        [SerializeField] float _speed = 10; 
        [SerializeField] Vector3 _dir = Vector3.zero;
        Rigidbody _rb;

        private void Start()
        {
            TryGetComponent(out _rb);
            _dir = GetRandomDirection();
        }

        private void FixedUpdate()
        {
            _rb.velocity = _dir * _speed;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.GetComponent<Goal>())  //ゴールに着いたら中心へ瞬間移動
            {
                transform.position = Vector3.zero;
                _dir = GetRandomDirection();
            }
            else if (collision.gameObject.GetComponent<Player>() || collision.gameObject.GetComponent<Pack>())  //playerとpackにあたったら中心点の差分を進行方向に
            {
                _dir = (transform.position - collision.gameObject.transform.position).normalized;
                _rb.velocity = _dir * _speed;
            }
            else　//その他の物体(今の所カベのみ)にあたったら入射角と法線を計算して反射角を割り出す
            {
                var inDirection = _dir;
                var inNormal = collision.contacts[0].normal;
                float dot = Vector3.Dot(inDirection, inNormal);
                Vector3 reflection = inDirection - 2f * dot * inNormal;
                _dir = reflection.normalized;
                _rb.velocity = _dir * _speed;
            }
        }

        /// <summary>
        /// 360度からランダムな方向を返す
        /// </summary>
        /// <returns></returns>
        public Vector3 GetRandomDirection()
        {
            float randomAngle = Random.Range(0f, 360f);
            Vector3 randomDirection = Quaternion.Euler(0f, randomAngle, 0f) * Vector3.forward;

            return randomDirection;
        }
    }
}
