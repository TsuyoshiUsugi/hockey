using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TsuyoshiLibrary
{
    public class Goal : MonoBehaviour
    {
        [SerializeField, Tooltip("スコアを追加する方を指定")] AddPointPlayer _player;
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
    /// スコアを追加する方を指定する為のenum
    /// </summary>
    public enum AddPointPlayer
    {
        Player1,
        Player2,
    }


}
