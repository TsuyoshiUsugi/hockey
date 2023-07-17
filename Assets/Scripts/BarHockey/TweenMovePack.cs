using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

namespace TsuyoshiLibrary
{
    /// <summary>
    /// パックのscript
    /// ・スピードを設定できる
    /// ・ゴールにあたると真ん中に移動
    /// ・playerにあたると円の中心点の差分をdirにいれる
    /// ・移動はDoTweenを使った移動
    /// </summary>
    public class TweenMovePack : MonoBehaviour
    {
        [Header("設定値")]
        [SerializeField, Tooltip("移動にかかる時間")] float _moveDuration = 10; 
        [SerializeField, Tooltip("独自に作成するイージング")] AnimationCurve _curve;
        [SerializeField] Ease _ease = Ease.InQuint;
        [SerializeField] bool _customEase = false;
        [Header("確認用")]
        [SerializeField] Vector3 _dir = Vector3.zero;
        [SerializeField] Vector3 _targetPoint = Vector3.zero;
        RaycastHit _hit;
        Tween _currentTween = null;
        Ray _ray;

        private void Start()
        {
            _dir = GetRandomDirection();
            RayCastToPoint();
            TweenMove(_hit.transform.position);
        }

        private void Update()
        {
            ShowReflectionPredictionPoint();
        }

        /// <summary>
        /// 進行方向にRayを放つ
        /// </summary>
        void ShowReflectionPredictionPoint()
        {
            
            Debug.DrawRay(_ray.origin, _ray.direction * 100, Color.blue);
            
        }

        void RayCastToPoint()
        {
            _ray = new Ray(this.transform.position, _dir);
            var hits = Physics.RaycastAll(_ray);
            if (hits.Length > 1)
            {
                for (int i = 0; i < hits.Length; i++)   //PlayerとPackオブジェクトを目標地点にしてしまうと途中でパックが止まってしまうので除外
                {
                    if (hits[i].transform.gameObject.tag != "Player" || hits[i].transform.gameObject.tag == "Pack")
                    {
                        _hit = hits[i];
                    }
                }
            }
            else
            {
                _hit = hits[0];
            }
        }

        void TweenMove(Vector3 point)
        {
            if (_customEase)
            {
                _currentTween = transform.DOMove(point, _moveDuration).SetEase(_curve);
                
            }
            else
            {
                _currentTween = transform.DOMove(point, _moveDuration).SetEase(_ease);
            }
            _targetPoint = point;
        }

        private void OnCollisionEnter(Collision collision)
        {
            
            if (collision.gameObject.tag == "Goal")  //ゴールに着いたら中心へ瞬間移動
            {
                Debug.Log("Goal");
                _currentTween.Kill();
                transform.position = Vector3.zero;
                _dir = GetRandomDirection();
                RayCastToPoint();
                TweenMove(_hit.point);
            }
            else if (collision.gameObject.tag == "Player")  //playerにあたったら中心点の差分を進行方向に
            {
                Debug.Log("Player");
                _dir = (transform.position - collision.gameObject.transform.position).normalized;
                _dir.y = 0;
                RayCastToPoint();
                TweenMove(_hit.point);
            }
            else　//その他の物体(今の所カベのみ)にあたったら入射角と法線を計算して反射角を割り出す
            {
                Debug.Log("Other");
                var inDirection = _dir;
                var inNormal = collision.contacts[0].normal;
                float dot = Vector3.Dot(inDirection, inNormal);
                Vector3 reflection = inDirection - 2f * dot * inNormal;
                _dir = new Vector3(reflection.x, 0, reflection.z);
                RayCastToPoint();
                TweenMove(_hit.point);
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

            return new Vector3(randomDirection.x, 0f, randomDirection.z).normalized;
        }

    }
}
