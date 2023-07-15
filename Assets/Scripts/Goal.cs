using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TsuyoshiLibrary
{
    public class Goal : MonoBehaviour
    {
        [SerializeField, Tooltip("�X�R�A��ǉ���������w��")] AddPointPlayer _player;
        Manager _manager;

        private void Start()
        {
            _manager = FindObjectOfType<Manager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            other.TryGetComponent(out Hockey hockey);

            if (hockey)
            {
                _manager.AddScore(_player);
            }
        }
    }

    /// <summary>
    /// �X�R�A��ǉ���������w�肷��ׂ�enum
    /// </summary>
    public enum AddPointPlayer
    {
        Player1,
        Player2,
    }


}
