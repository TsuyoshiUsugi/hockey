using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using static UnityEditor.Experimental.GraphView.GraphView;

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

        void RayCastToPoint()
        {
            _ray = new Ray(this.transform.position, _dir);
            var hits = Physics.RaycastAll(_ray);
            if (hits.Length > 1)
            {
                for (int i = 0; i < hits.Length; i++)   //Player��Pack��Bar�I�u�W�F�N�g��ڕW�n�_�ɂ��Ă��܂��Ɠr���Ńp�b�N���~�܂��Ă��܂��̂ŏ��O
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
                yield return null; // 1�t���[���ҋ@
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
        /// Pack���ǂ̊O�ɂłȂ��悤�ɂ���
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

            return new Vector3(randomDirection.x, 0, randomDirection.z).normalized;
        }

    }
}
