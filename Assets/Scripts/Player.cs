using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TsuyoshiLibrary
{
    /// <summary>
    /// player��script
    /// �O����ړ��������Ăяo�����Ƃňړ�����
    /// ��ɒ��ڈʒu�𑀍삷��\��
    /// </summary>
    public class Player : MonoBehaviour
    {
        /// <summary>�ǂ����player��script��</summary>
        [SerializeField] OwnerPlayer _player;
        [SerializeField] float _speed = 0.1f;
        FieldInfo _field;

        float _offset = 1;  //player(�}���b�g)�̃T�C�Y

        private void Start()
        {
            _field = FindObjectOfType<FieldInfo>();
            _offset = transform.localScale.x;
        }

        private void Update()
        {
            ClampPos();
        }

        /// <summary>
        /// player���J�x�̊O�ɂłȂ��悤�ɂ���
        /// </summary>
        private void ClampPos()
        {
            if (_player == OwnerPlayer.Player1)
            {
                float clampedX = Mathf.Clamp(transform.position.x, _field.LeftGoal.transform.position.x + _offset, _field.Split.transform.position.x - _offset);
                transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
            }
            else
            {
                float clampedX = Mathf.Clamp(transform.position.x, _field.Split.transform.position.x + _offset, _field.RightGoal.transform.position.x - _offset);
                transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
            }

            float clampedZ = Mathf.Clamp(transform.position.z, _field.Down.transform.position.z + _offset, _field.Top.transform.position.z - _offset);
            transform.position = new Vector3(transform.position.x, transform.position.y, clampedZ);
        }

        /// <summary>
        /// ���͂ɉ�����player�̈ʒu�𑀍삷��֐�
        /// </summary>
        /// <param name="v"></param>
        /// <param name="h"></param>
        public void MovePos(float v, float h)
        {
            this.transform.position += new Vector3(h, 0, v) * _speed * Time.deltaTime;
        }

        /// <summary>
        /// ����player���ړ�����֐�
        /// </summary>
        /// <param name="pos"></param>
        public void SetPos(Vector3 pos)
        {
            this.transform.position = pos;
        }
    }
}
