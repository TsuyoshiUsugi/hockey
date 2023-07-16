using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TsuyoshiLibrary
{
    /// <summary>
    /// �S�[����script
    /// pack��������Ɛݒ肵���v���C���[�̃X�R�A�𑝂₷�}�l�[�W���[�̊֐����Ăяo��
    /// </summary>
    public class Goal : MonoBehaviour
    {
        [SerializeField, Tooltip("�X�R�A��ǉ���������w��")] OwnerPlayer _player;
        Manager _manager;

        private void Start()
        {
            _manager = FindObjectOfType<Manager>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            collision.gameObject.TryGetComponent(out Pack hockey);

            if (hockey)
            {
                _manager.AddScore(_player);
            }
        }
    }

    /// <summary>
    /// �ǂ����player���w�肷��ׂ�enum
    /// </summary>
    public enum OwnerPlayer
    {
        Player1,
        Player2,
    }


}
