using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TsuyoshiLibrary
{
    public class Goal : MonoBehaviour
    {
        [SerializeField, Tooltip("スコアを追加する方を指定")] OwnerPlayer _player;
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
    /// どちらのplayerか指定する為のenum
    /// </summary>
    public enum OwnerPlayer
    {
        Player1,
        Player2,
    }


}
