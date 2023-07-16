using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TsuyoshiLibrary
{
    /// <summary>
    /// �X�R�A��UI�Ɛݒ���Ǘ�����
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
        /// �X�R�A���Ăяo������
        /// �S�[������Ăяo��
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
            _packNumText.text = $"�p�b�N���F{_packs.Count + 1}";     //����������̂�+1
        }

        /// <summary>
        /// �X�R�A��0�ɂ���
        /// </summary>
        public void ResetScore() 
        {
            _p1Score = 0; 
            _p2Score = 0 ;
        }

        /// <summary>
        /// pack��0�ɂȂ�܂Ő�������pack������
        /// ����������pack�̓��X�g�ɓ����Ă��Ȃ��̂ŕK����c��
        /// </summary>
        public void ResetPack()
        {
            while (_packs.Count > 0) DestroyPack();
            _pack.transform.position = Vector3.zero;
        }

        /// <summary>
        /// pack��^�񒆂̃��C����ɐ�������
        /// ���Ƃŏ����Ƃ��ׂ̈Ƀ��X�g�ɓ����
        /// </summary>
        public void GeneratePack()
        {
            var randomPos = Random.Range(_fieldInfo.Down.transform.position.z + _offset, _fieldInfo.Top.transform.position.z - _offset);
            var pack = Instantiate(_pack, new Vector3(0, 0, randomPos), Quaternion.identity);
            _packs.Add(pack);
        }

        /// <summary>
        /// pack�̔j�󏈗�
        /// </summary>
        public void DestroyPack()
        {
            if (_packs.Count == 0) return;
            Destroy(_packs[0]);
            _packs.RemoveAt(0);
        }
    }
}
