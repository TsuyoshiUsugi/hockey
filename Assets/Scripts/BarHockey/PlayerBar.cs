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

        private void Update()
        {
            SetBar();
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
            _bar.transform.localScale = new Vector3(0.1f, 0.1f, Vector3.Distance(_mallet1.transform.position, _mallet2.transform.position));    //スケールを変えて長さを合わせる
            Vector3 direction = _mallet1.transform.position - _mallet2.transform.position;
            _bar.transform.rotation = Quaternion.LookRotation(direction);   //二点間のベクトルを出し、回転を適用
        }
    }
}
