using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using static UnityEditor.Experimental.GraphView.GraphView;

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
        [SerializeField, Tooltip("独自のイージングを使うか")] bool _customEase = false;
        [SerializeField, Tooltip("独自に作成するイージング")] AnimationCurve _curve;
        [SerializeField, Tooltip("元からあるイージング")] Ease _ease = Ease.InQuint;
        [SerializeField, Tooltip("連続してHitした際に何フレームSkipするか")] float _skipFrame = 10; 
        [Header("確認用")]
        [SerializeField] Vector3 _dir = Vector3.zero;
        [SerializeField] Vector3 _targetPoint = Vector3.zero;
        RaycastHit _hit;
        Tween _currentTween = null;
        Ray _ray;
        bool _isInSkipFrame = false;

        private void Start()
        {
            _dir = GetRandomDirection();
            RayCastToPoint();
            TweenMove(_hit.point);
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
                for (int i = 0; i < hits.Length; i++)   //PlayerとPackとBarオブジェクトを目標地点にしてしまうと途中でパックが止まってしまうので除外
                {
                    if (hits[i].transform.gameObject.tag == "Player") continue;
                    if (hits[i].transform.gameObject.tag == "Pack") continue;
                    if (hits[i].transform.gameObject.tag == "Bar") continue;

                    _hit = hits[i];

                }
            }
            else
            {
                _hit = hits[0];
            }
        }

        private IEnumerator WaitForFrames()
        {
            _isInSkipFrame = true;
            for (int i = 0; i < _skipFrame; i++)
            {
                yield return null; // 1フレーム待機
            }
            _isInSkipFrame = false;
        }

        void TweenMove(Vector3 point)
        {
            StartCoroutine(nameof(WaitForFrames));



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

        /// <summary>
        /// Packが壁の外にでないようにする
        /// </summary>
        private void ClampPos()
        {
            if (_player == OwnerPlayer.Player1)
            {
                float clampedX = Mathf.Clamp(_mallet1.transform.position.x, _field.LeftGoal.transform.position.x + _offset, _field.Split.transform.position.x - _offset);
                float clampedX2 = Mathf.Clamp(_mallet2.transform.position.x, _field.LeftGoal.transform.position.x + _offset, _field.Split.transform.position.x - _offset);
                _mallet1.transform.position = new Vector3(clampedX, _mallet1.transform.position.y, _mallet1.transform.position.z);
                _mallet2.transform.position = new Vector3(clampedX2, _mallet2.transform.position.y, _mallet2.transform.position.z);
            }
            else
            {
                float clampedX = Mathf.Clamp(_mallet1.transform.position.x, _field.Split.transform.position.x + _offset, _field.RightGoal.transform.position.x - _offset);
                float clampedX2 = Mathf.Clamp(_mallet2.transform.position.x, _field.Split.transform.position.x + _offset, _field.RightGoal.transform.position.x - _offset);
                _mallet1.transform.position = new Vector3(clampedX, _mallet1.transform.position.y, _mallet1.transform.position.z);
                _mallet2.transform.position = new Vector3(clampedX2, _mallet2.transform.position.y, _mallet2.transform.position.z);
            }

            float clampedZ = Mathf.Clamp(_mallet1.transform.position.z, _field.Down.transform.position.z + _offset, _field.Top.transform.position.z - _offset);
            float clampedZ2 = Mathf.Clamp(_mallet2.transform.position.z, _field.Down.transform.position.z + _offset, _field.Top.transform.position.z - _offset);
            _mallet1.transform.position = new Vector3(_mallet1.transform.position.x, _mallet1.transform.position.y, clampedZ);
            _mallet2.transform.position = new Vector3(_mallet2.transform.position.x, _mallet2.transform.position.y, clampedZ2);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_isInSkipFrame) return;
            Debug.Log(collision.gameObject.tag);
            if (collision.gameObject.tag == "Goal")  //ゴールに着いたら中心へ瞬間移動
            {
                _currentTween.Kill();
                transform.position = Vector3.zero;
                _dir = GetRandomDirection();
                RayCastToPoint();
                TweenMove(_hit.point);
            }
            else if (collision.gameObject.tag == "Player")  //playerにあたったら中心点の差分を進行方向に
            {
                _dir = (transform.position - collision.gameObject.transform.position).normalized;
                _dir.y = 0;
                RayCastToPoint();
                TweenMove(_hit.point);
            }
            else　if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Bar")   //壁とプレイヤーのバーにあたったら法線を取得して反射角を割り出して跳ね返る
            {
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

            return new Vector3(randomDirection.x, 0, randomDirection.z).normalized;
        }

    }
}
