using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

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
        [SerializeField, Tooltip("�ړ��ɂ����鎞��")] float _moveDuration = 10; 
        [SerializeField, Tooltip("�Ǝ��̃C�[�W���O���g����")] bool _customEase = false;
        [SerializeField, Tooltip("�Ǝ��ɍ쐬����C�[�W���O")] AnimationCurve _curve;
        [SerializeField, Tooltip("�����炠��C�[�W���O")] Ease _ease = Ease.InQuint;
        [SerializeField, Tooltip("�A������Hit�����ۂɉ��t���[��Skip���邩")] float _skipFrame = 10; 
        [Header("�m�F�p")]
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
            TweenMove(_hit.transform.position);
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

        void RayCastToPoint()
        {
            _ray = new Ray(this.transform.position, _dir);
            var hits = Physics.RaycastAll(_ray);
            if (hits.Length > 1)
            {
                for (int i = 0; i < hits.Length; i++)   //Player��Pack��Bar�I�u�W�F�N�g��ڕW�n�_�ɂ��Ă��܂��Ɠr���Ńp�b�N���~�܂��Ă��܂��̂ŏ��O
                {
                    if (hits[i].transform.gameObject.tag != "Player" || hits[i].transform.gameObject.tag == "Pack" || hits[i].transform.gameObject.tag == "Bar")
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

        private IEnumerator WaitForFrames()
        {
            _isInSkipFrame = true;
            for (int i = 0; i < _skipFrame; i++)
            {
                yield return null; // 1�t���[���ҋ@
            }
            _isInSkipFrame = false;
        }

        void TweenMove(Vector3 point)
        {
            if (_isInSkipFrame) return;

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
        
        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log(collision.gameObject.tag);
            if (collision.gameObject.tag == "Goal")  //�S�[���ɒ������璆�S�֏u�Ԉړ�
            {
                _currentTween.Kill();
                transform.position = Vector3.zero;
                _dir = GetRandomDirection();
                RayCastToPoint();
                TweenMove(_hit.point);
            }
            else if (collision.gameObject.tag == "Player")  //player�ɂ��������璆�S�_�̍�����i�s������
            {
                _dir = (transform.position - collision.gameObject.transform.position).normalized;
                _dir.y = 0;
                RayCastToPoint();
                TweenMove(_hit.point);
            }
            else�@if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Bar")   //�ǂƃv���C���[�̃o�[�ɂ���������@�����擾���Ĕ��ˊp������o���Ē��˕Ԃ�
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
        /// 360�x���烉���_���ȕ�����Ԃ�
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
