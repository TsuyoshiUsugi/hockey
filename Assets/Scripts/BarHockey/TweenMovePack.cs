using System.Collections;
using UnityEngine;
using DG.Tweening;

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
        [SerializeField, Tooltip("移動にかかる時間の基準")] float _moveDuration = 10; 
        [SerializeField, Tooltip("独自のイージングを使うか")] bool _customEase = false;
        [SerializeField, Tooltip("独自に作成するイージング")] AnimationCurve _curve;
        [SerializeField, Tooltip("元からあるイージング")] Ease _ease = Ease.InQuint;
        [SerializeField, Tooltip("連続してHitした際に何フレームSkipするか")] float _skipFrame = 10; 
        [SerializeField] float _offset = 1;
        [Header("確認用")]
        [SerializeField] bool _isInSkipFrame = false;
        [SerializeField] Vector3 _dir = Vector3.zero;
        [SerializeField] Vector3 _targetPoint = Vector3.zero;
        [SerializeField] GameObject _prehitObj = null;
        Ray _ray;
        FieldInfo _field;
        RaycastHit _hit;
        Tween _currentTween = null;

        public float Speed { get => _moveDuration; set => _moveDuration = value; }

        private void Start()
        {
            _field = FindAnyObjectByType<FieldInfo>();
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

        /// <summary>
        /// _dirを元にRayを放つ
        /// 当たったオブジェクトを_hitに入れる
        /// </summary>
        void RayCastToPoint()
        {
            _ray = new Ray(this.transform.position, _dir);
            var hits = Physics.RaycastAll(_ray);

            for (int i = 0; i < hits.Length; i++)   //PlayerとPackとBarオブジェクトを目標地点にしてしまうと途中でパックが止まってしまうので除外
            {
                if (hits[i].transform.gameObject.tag == "Player") continue;
                if (hits[i].transform.gameObject.tag == "Pack") continue;
                if (hits[i].transform.gameObject.tag == "Bar") continue;
                _hit = hits[i];
            }
        }

        private IEnumerator WaitForFrames()
        {
            _isInSkipFrame = true;
            for (int i = 0; i < _skipFrame; i++)
            {
                yield return null; 
            }
            _isInSkipFrame = false;
        }
        /// <summary>
        /// Tweenを用いた移動を行う
        /// </summary>
        /// <param name="point"></param>
        void TweenMove(Vector3 point)
        {
            point = ClampPos(point);
            var moveTime = Vector3.Magnitude(point - transform.position) / _moveDuration;

            if (_customEase)
            {
                _currentTween = transform.DOMove(point, moveTime).SetEase(_curve);
            }
            else
            {
                _currentTween = transform.DOMove(point, moveTime).SetEase(_ease);
            }
            _targetPoint = point;
        }

        /// <summary>
        /// PackのTweenの終着点が壁の外にでないようにする
        /// </summary>
        private Vector3 ClampPos(Vector3 point)
        {
            float clampedX = Mathf.Clamp(point.x, _field.LeftGoal.transform.position.x - _offset, _field.RightGoal.transform.position.x + _offset);
            float clampedZ = Mathf.Clamp(point.z, _field.Down.transform.position.z - _offset, _field.Top.transform.position.z + _offset);
            return new Vector3(clampedX, point.y, clampedZ);
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log(collision.gameObject.tag);
            StartCoroutine(nameof(WaitForFrames));
            

            if (collision.gameObject.tag == "Goal")  //ゴールに着いたら中心へ瞬間移動
            {
                _currentTween.Kill();   //それまでのTweenをKill
                var randomPos = Random.Range(_field.Down.transform.position.z + _offset, _field.Top.transform.position.z - _offset);    
                transform.position = new Vector3(0, transform.position.y, randomPos);   //中心の縦にランダムな位置に移動
                _dir = GetRandomDirection();    //ランダムに移動する方向を算出
                RayCastToPoint();   //自身の位置から_dirに向かってRayを放つ
                TweenMove(_hit.point);  //Rayの当たった場所にTween
            }
            else if (collision.gameObject.tag == "Player")  //playerにあたったら中心点の差分を進行方向に
            {
                _currentTween.Kill();   //それまでのTweenをKill
                _dir = (transform.position - collision.gameObject.transform.position).normalized;
                _dir.y = 0;
                RayCastToPoint();
                TweenMove(_hit.point);
                _prehitObj = collision.gameObject;
            }
            else　if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Bar" || collision.gameObject.tag == "Pack")   //壁とプレイヤーのバーもしくはパックにあたったら法線を取得して反射角を割り出して跳ね返る
            {
                _currentTween.Kill();   //それまでのTweenをKill
                CaluculateReflectionAngle(collision);
                RayCastToPoint();
                TweenMove(_hit.point);
                _prehitObj = collision.gameObject;
            }
        }

        /// <summary>
        /// 反射角を計算する
        /// </summary>
        /// <param name="collision"></param>
        private void CaluculateReflectionAngle(Collision collision)
        {
            var inDirection = _dir;
            var inNormal = collision.contacts[0].normal;
            float dot = Vector3.Dot(inDirection, inNormal);
            Vector3 reflection = inDirection - 2f * dot * inNormal;
            _dir = new Vector3(reflection.x, 0, reflection.z);
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
