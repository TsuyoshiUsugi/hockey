using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TsuyoshiLibrary
{
    /// <summary>
    /// スコアとUIと設定を管理する
    /// </summary>
    public class Manager : MonoBehaviour
    {
        [SerializeField] int _p1Score = 0;
        [SerializeField] int _p2Score = 0;
        [SerializeField] Text _scoreText = null;
        [SerializeField] Text _packNumText = null;
        [SerializeField] GameObject _pack;
        List<GameObject> _packs = new List<GameObject>();
        FieldInfo _fieldInfo;
        float _offset = 1;

        private void Start()
        {
            _fieldInfo = FindAnyObjectByType<FieldInfo>();
            _offset = _pack.transform.localScale.x;
        }

        private void Update()
        {
            SetText();
        }

        /// <summary>
        /// スコアを呼び出す処理
        /// ゴールから呼び出す
        /// </summary>
        /// <param name="addPointPlayer"></param>
        public void AddScore(OwnerPlayer addPointPlayer)
        {
            if (addPointPlayer == OwnerPlayer.Player1)
            {
                _p1Score++;
            }
            else
            {
                _p2Score++;
            }
        }

        private void SetText()
        {
            _scoreText.text = $"{_p1Score}:{_p2Score}";
            _packNumText.text = $"パック数：{_packs.Count + 1}";     //元から一つあるので+1
        }

        /// <summary>
        /// スコアを0にする
        /// </summary>
        public void ResetScore() 
        {
            _p1Score = 0; 
            _p2Score = 0 ;
        }

        /// <summary>
        /// packが0になるまで生成したpackを消す
        /// 元から一つあるpackはリストに入っていないので必ず一つ残る
        /// </summary>
        public void ResetPack()
        {
            while (_packs.Count > 0) DestroyPack();
            _pack.transform.position = Vector3.zero;
        }

        /// <summary>
        /// packを真ん中のライン上に生成する
        /// あとで消すときの為にリストに入れる
        /// </summary>
        public void GeneratePack()
        {
            var randomPos = Random.Range(_fieldInfo.Down.transform.position.z + _offset, _fieldInfo.Top.transform.position.z - _offset);
            var pack = Instantiate(_pack, new Vector3(0, 0, randomPos), Quaternion.identity);
            _packs.Add(pack);
        }

        /// <summary>
        /// packの破壊処理
        /// </summary>
        public void DestroyPack()
        {
            if (_packs.Count == 0) return;
            Destroy(_packs[0]);
            _packs.RemoveAt(0);
        }
    }
}
