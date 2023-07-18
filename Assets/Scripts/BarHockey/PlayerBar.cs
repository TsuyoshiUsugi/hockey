using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TsuyoshiLibrary
{
    /// <summary>
    /// �v���C���[�����삷��G�A�z�b�P�[�̃o�[
    /// </summary>
    public class PlayerBar : MonoBehaviour
    {
        [SerializeField] GameObject _mallet1;
        [SerializeField] GameObject _mallet2;
        [SerializeField] GameObject _bar;
        [SerializeField] float _speed = 10;
        [SerializeField] OwnerPlayer _player;
        [SerializeField] float _offset = 1.1f;
        [SerializeField] float _barLengthLimit = 10;
        FieldInfo _field;
        
        private void Start()
        {
            _field = FindAnyObjectByType<FieldInfo>();
        }

        private void Update()
        {
            SetBar();
            ClampPos();
        }

        /// <summary>
        /// ��ڂ̃}���b�g�̈ʒu���ړ�����
        /// </summary>
        /// <param name="v"></param>
        /// <param name="h"></param>
        public void MoveMalletPosOne(float v, float h)
        {
            _mallet1.transform.position += new Vector3(v, 0, h) * _speed * Time.deltaTime;
        }
        
        /// <summary>
        /// ��ڂ̃}���b�g�̈ʒu���ړ�����
        /// </summary>
        /// <param name="v"></param>
        /// <param name="h"></param>
        public void MoveMalletPosTwo(float v, float h)
        {
            _mallet2.transform.position += new Vector3(v, 0, h) * _speed * Time.deltaTime;
        }

        /// <summary>
        /// ��ڂ̃}���b�g�̈ʒu�����߂�
        /// </summary>
        /// <param name="pos"></param>
        public void SetMalletPosOne(Vector3 pos)
        {
            _mallet1.transform.position = pos;
        }

        /// <summary>
        /// ��ڂ̃}���b�g�̈ʒu�����߂�
        /// </summary>
        public void SetMalletPosTwo(Vector3 pos)
        {
            _mallet2.transform.position = pos;
        }

        /// <summary>
        /// �}���b�g�̈ʒu�����Ƀo�[��ݒ肷��
        /// </summary>
        void SetBar()
        {
            _bar.transform.position = (_mallet1.transform.position + _mallet2.transform.position) / 2;  //��̃}���b�g�̒��S�Ɉʒu���ړ�
            _bar.transform.localScale = new Vector3(0.5f, 0.1f, Vector3.Distance(_mallet1.transform.position, _mallet2.transform.position));    //�X�P�[����ς��Ē��������킹��
            Vector3 direction = _mallet1.transform.position - _mallet2.transform.position;
            _bar.transform.rotation = Quaternion.LookRotation(direction);   //��_�Ԃ̃x�N�g�����o���A��]��K�p
        }

        /// <summary>
        /// player���J�x�̊O�ɂłȂ��悤�ɂ���
        /// </summary>
        private void ClampPos()
        {
            float clampedX, clampedX2, clampedZ, clampedZ2;

            if (_player == OwnerPlayer.Player1)
            {
                clampedX = Mathf.Clamp(_mallet1.transform.position.x, _field.LeftGoal.transform.position.x + _offset, _field.Split.transform.position.x - _offset);
                clampedX2 = Mathf.Clamp(_mallet2.transform.position.x, _field.LeftGoal.transform.position.x + _offset, _field.Split.transform.position.x - _offset);
            }
            else
            {
                clampedX = Mathf.Clamp(_mallet1.transform.position.x, _field.Split.transform.position.x + _offset, _field.RightGoal.transform.position.x - _offset);
                clampedX2 = Mathf.Clamp(_mallet2.transform.position.x, _field.Split.transform.position.x + _offset, _field.RightGoal.transform.position.x - _offset);
            }

            clampedZ = Mathf.Clamp(_mallet1.transform.position.z, _field.Down.transform.position.z + _offset, _field.Top.transform.position.z - _offset);
            clampedZ2 = Mathf.Clamp(_mallet2.transform.position.z, _field.Down.transform.position.z + _offset, _field.Top.transform.position.z - _offset);

            _mallet1.transform.position = new Vector3(clampedX, _mallet1.transform.position.y, clampedZ);
            _mallet2.transform.position = new Vector3(clampedX2, _mallet2.transform.position.y, clampedZ2);
        }

    }
}
