using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TsuyoshiLibrary
{
    /// <summary>
    /// playerのscript
    /// 外から移動処理を呼び出すことで移動する
    /// 後に直接位置を操作する予定
    /// </summary>
    public class Player : MonoBehaviour
    {
        /// <summary>どちらのplayerのscriptか</summary>
        [SerializeField] OwnerPlayer _player;
        [SerializeField] float _speed = 0.1f;
        FieldInfo _field;

        float _offset = 1;  //player(マレット)のサイズ

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
        /// playerがカベの外にでないようにする
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
        /// 入力に応じてplayerの位置を操作する関数
        /// </summary>
        /// <param name="v"></param>
        /// <param name="h"></param>
        public void MovePos(float v, float h)
        {
            this.transform.position += new Vector3(h, 0, v) * _speed * Time.deltaTime;
        }

        /// <summary>
        /// 直接playerを移動する関数
        /// </summary>
        /// <param name="pos"></param>
        public void SetPos(Vector3 pos)
        {
            this.transform.position = pos;
        }
    }
}
