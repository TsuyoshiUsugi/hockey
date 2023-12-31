using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TsuyoshiLibrary
{
    /// <summary>
    /// ゴールのscript
    /// packが当たると設定したプレイヤーのスコアを増やすマネージャーの関数を呼び出す
    /// </summary>
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
            if (collision.gameObject.tag == "Pack")
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
