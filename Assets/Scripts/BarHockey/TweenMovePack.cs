using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace TsuyoshiLibrary
{
    /// <summary>
    /// �p�b�N��script
    /// �E�X�s�[�h��ݒ�ł���
    /// �E�S�[���ɂ�����Ɛ^�񒆂Ɉړ�
    /// �Eplayer�ɂ�����Ɖ~�̒��S�_�̍�����dir�ɂ����
    /// �E�ړ���DoTween���g�����ړ�
    /// </summary>
    public class TweenMovePack : MonoBehaviour
    {
        [Header("�ݒ�l")]
        [SerializeField, Tooltip("�ړ��ɂ����鎞�Ԃ̊")] float _moveDuration = 10; 
        [SerializeField, Tooltip("�Ǝ��̃C�[�W���O���g����")] bool _customEase = false;
        [SerializeField, Tooltip("�Ǝ��ɍ쐬����C�[�W���O")] AnimationCurve _curve;
        [SerializeField, Tooltip("�����炠��C�[�W���O")] Ease _ease = Ease.InQuint;
        [SerializeField, Tooltip("�A������Hit�����ۂɉ��t���[��Skip���邩")] float _skipFrame = 10; 
        [SerializeField] float _offset = 1;
        [Header("�m�F�p")]
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
        /// �i�s������Ray�����
        /// </summary>
        void ShowReflectionPredictionPoint()
        {
            Debug.DrawRay(_ray.origin, _ray.direction * 100, Color.blue);
        }

        /// <summary>
        /// _dir������Ray�����
        /// ���������I�u�W�F�N�g��_hit�ɓ����
        /// </summary>
        void RayCastToPoint()
        {
            _ray = new Ray(this.transform.position, _dir);
            var hits = Physics.RaycastAll(_ray);

            for (int i = 0; i < hits.Length; i++)   //Player��Pack��Bar�I�u�W�F�N�g��ڕW�n�_�ɂ��Ă��܂��Ɠr���Ńp�b�N���~�܂��Ă��܂��̂ŏ��O
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
        /// Tween��p�����ړ����s��
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
        /// Pack��Tween�̏I���_���ǂ̊O�ɂłȂ��悤�ɂ���
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
            

            if (collision.gameObject.tag == "Goal")  //�S�[���ɒ������璆�S�֏u�Ԉړ�
            {
                _currentTween.Kill();   //����܂ł�Tween��Kill
                var randomPos = Random.Range(_field.Down.transform.position.z + _offset, _field.Top.transform.position.z - _offset);    
                transform.position = new Vector3(0, transform.position.y, randomPos);   //���S�̏c�Ƀ����_���Ȉʒu�Ɉړ�
                _dir = GetRandomDirection();    //�����_���Ɉړ�����������Z�o
                RayCastToPoint();   //���g�̈ʒu����_dir�Ɍ�������Ray�����
                TweenMove(_hit.point);  //Ray�̓��������ꏊ��Tween
            }
            else if (collision.gameObject.tag == "Player")  //player�ɂ��������璆�S�_�̍�����i�s������
            {
                _currentTween.Kill();   //����܂ł�Tween��Kill
                _dir = (transform.position - collision.gameObject.transform.position).normalized;
                _dir.y = 0;
                RayCastToPoint();
                TweenMove(_hit.point);
                _prehitObj = collision.gameObject;
            }
            else�@if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Bar" || collision.gameObject.tag == "Pack")   //�ǂƃv���C���[�̃o�[�������̓p�b�N�ɂ���������@�����擾���Ĕ��ˊp������o���Ē��˕Ԃ�
            {
                _currentTween.Kill();   //����܂ł�Tween��Kill
                CaluculateReflectionAngle(collision);
                RayCastToPoint();
                TweenMove(_hit.point);
                _prehitObj = collision.gameObject;
            }
        }

        /// <summary>
        /// ���ˊp���v�Z����
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
        /// 360�x���烉���_���ȕ�����Ԃ�
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
