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
        [SerializeField] Vector3 _dir = Vector3.zero;
        [SerializeField] Vector3 _targetPoint = Vector3.zero;
        RaycastHit _hit;
        Tween _currentTween = null;
        Ray _ray;
        bool _isInSkipFrame = false;
        FieldInfo _field;
        [SerializeField] GameObject _prehitObj = null;

        public float Speed { get => _moveDuration; set => _moveDuration = value; }

        private void Start()
        {
            _field = FindAnyObjectByType<FieldInfo>();
            _dir = GetRandomDirection();
            RayCastToPoint();
            TweenMove(_hit.point);
            _isInSkipFrame = false; //最初はスキップカウントしない
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

            point = ClampPos(point);

            var moveTime = Vector3.Magnitude(point - transform.position) / _moveDuration;
            Debug.Log(moveTime);
            //var moveTime = _moveDuration;

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

            float clampedX = Mathf.Clamp(point.x, _field.LeftGoal.transform.position.x + _offset, _field.RightGoal.transform.position.x - _offset);
            float clampedZ = Mathf.Clamp(point.z, _field.Down.transform.position.z + _offset, _field.Top.transform.position.z - _offset);
            return new Vector3(clampedX, point.y, clampedZ);

        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log(collision.gameObject.tag);
            
            
            if (collision.gameObject.tag == "Goal")  //ゴールに着いたら中心へ瞬間移動
            {
                _currentTween.Kill();
                var randomPos = Random.Range(_field.Down.transform.position.z + _offset, _field.Top.transform.position.z - _offset);
                transform.position = new Vector3(0, 0, randomPos);
                _dir = GetRandomDirection();
                RayCastToPoint();
                TweenMove(_hit.point);
            }
            else if (collision.gameObject.tag == "Player")  //playerにあたったら中心点の差分を進行方向に
            {
                Debug.Log(SkipCheck(collision.gameObject));
                if (SkipCheck(collision.gameObject)) return;
                _dir = (transform.position - collision.gameObject.transform.position).normalized;
                _dir.y = 0;
                RayCastToPoint();
                TweenMove(_hit.point);
                _prehitObj = collision.gameObject;
            }
            else　if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Bar" || collision.gameObject.tag == "Pack")   //壁とプレイヤーのバーもしくはパックにあたったら法線を取得して反射角を割り出して跳ね返る
            {
                Debug.Log(SkipCheck(collision.gameObject));
                if (SkipCheck(collision.gameObject)) return;
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
        /// 当たり判定をスキップするかチェックする
        /// </summary>
        bool SkipCheck(GameObject hitObj)
        {
            if (!_isInSkipFrame) return false;      //スキップフレーム中じゃないならFalse
            if (hitObj != _prehitObj) return false; //スキップフレーム中に当たったオブジェクトが同じオブジェクトじゃなかったらfalse
            
            return true;    //スキップフレーム中で当たったオブジェクトが前と同じならtrue
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
