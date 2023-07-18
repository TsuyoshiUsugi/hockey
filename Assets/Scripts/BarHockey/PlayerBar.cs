using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TsuyoshiLibrary
{
    /// <summary>
    /// プレイヤーが操作するエアホッケーのバー
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
        /// 一つ目のマレットの位置を移動する
        /// </summary>
        /// <param name="v"></param>
        /// <param name="h"></param>
        public void MoveMalletPosOne(float v, float h)
        {
            _mallet1.transform.position += new Vector3(v, 0, h) * _speed * Time.deltaTime;
        }
        
        /// <summary>
        /// 二つ目のマレットの位置を移動する
        /// </summary>
        /// <param name="v"></param>
        /// <param name="h"></param>
        public void MoveMalletPosTwo(float v, float h)
        {
            _mallet2.transform.position += new Vector3(v, 0, h) * _speed * Time.deltaTime;
        }

        /// <summary>
        /// 一つ目のマレットの位置を決める
        /// </summary>
        /// <param name="pos"></param>
        public void SetMalletPosOne(Vector3 pos)
        {
            _mallet1.transform.position = pos;
        }

        /// <summary>
        /// 二つ目のマレットの位置を決める
        /// </summary>
        public void SetMalletPosTwo(Vector3 pos)
        {
            _mallet2.transform.position = pos;
        }

        /// <summary>
        /// マレットの位置を元にバーを設定する
        /// </summary>
        void SetBar()
        {
            _bar.transform.position = (_mallet1.transform.position + _mallet2.transform.position) / 2;  //二つのマレットの中心に位置を移動
            _bar.transform.localScale = new Vector3(0.5f, 0.1f, Vector3.Distance(_mallet1.transform.position, _mallet2.transform.position));    //スケールを変えて長さを合わせる
            Vector3 direction = _mallet1.transform.position - _mallet2.transform.position;
            _bar.transform.rotation = Quaternion.LookRotation(direction);   //二点間のベクトルを出し、回転を適用
        }

        /// <summary>
        /// playerがカベの外にでないようにする
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
