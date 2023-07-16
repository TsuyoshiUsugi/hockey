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

        //�㉺���E�̃J�x�B�ړ��\�͈͂���邽�ߎg�p
        [SerializeField] GameObject _split;
        [SerializeField] GameObject _top;
        [SerializeField] GameObject _down;
        [SerializeField] GameObject _goal;

        float _offset = 1;  //player(�}���b�g)�̃T�C�Y

        private void Start()
        {
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
                float clampedX = Mathf.Clamp(transform.position.x, _goal.transform.position.x + _offset, _split.transform.position.x - _offset);
                transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);

            }
            else
            {
                float clampedX = Mathf.Clamp(transform.position.x, _split.transform.position.x + _offset, _goal.transform.position.x - _offset);
                transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
            }

            float clampedZ = Mathf.Clamp(transform.position.z, _down.transform.position.z + _offset, _top.transform.position.z - _offset);
            transform.position = new Vector3(transform.position.x, transform.position.y, clampedZ);
        }

        /// <summary>
        /// ���͂ɉ�����player�̈ʒu�𑀍삷��֐�
        /// </summary>
        /// <param name="v"></param>
        /// <param name="h"></param>
        public void MovePos(float v, float h)
        {
            this.transform.position += new Vector3(h, 0, v) * _speed;
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
