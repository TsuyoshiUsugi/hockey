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

        private void Update()
        {
            SetBar();
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
            _bar.transform.localScale = new Vector3(0.1f, 0.1f, Vector3.Distance(_mallet1.transform.position, _mallet2.transform.position));    //�X�P�[����ς��Ē��������킹��
            Vector3 direction = _mallet1.transform.position - _mallet2.transform.position;
            _bar.transform.rotation = Quaternion.LookRotation(direction);   //��_�Ԃ̃x�N�g�����o���A��]��K�p
        }
    }
}
