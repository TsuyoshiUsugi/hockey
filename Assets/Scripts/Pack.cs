using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TsuyoshiLibrary
{
    /// <summary>
    /// �p�b�N��script
    /// �E�X�s�[�h��ݒ�ł���
    /// �E�S�[���ɂ�����Ɛ^�񒆂Ɉړ�
    /// �Eplayer��pack�ɂ�����Ɖ~�̒��S�_�̍�����dir�ɂ���Đi��
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
            if (collision.gameObject.GetComponent<Goal>())  //�S�[���ɒ������璆�S�֏u�Ԉړ�
            {
                transform.position = Vector3.zero;
                _dir = GetRandomDirection();
            }
            else if (collision.gameObject.GetComponent<Player>() || collision.gameObject.GetComponent<Pack>())  //player��pack�ɂ��������璆�S�_�̍�����i�s������
            {
                _dir = (transform.position - collision.gameObject.transform.position).normalized;
                _rb.velocity = _dir * _speed;
            }
            else�@//���̑��̕���(���̏��J�x�̂�)�ɂ�����������ˊp�Ɩ@�����v�Z���Ĕ��ˊp������o��
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
        /// 360�x���烉���_���ȕ�����Ԃ�
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
