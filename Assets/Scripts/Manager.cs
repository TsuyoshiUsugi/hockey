using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TsuyoshiLibrary
{
    public class Manager : MonoBehaviour
    {
        [SerializeField] int _p1Score = 0;
        [SerializeField] int _p2Score = 0;
        [SerializeField] Text _scoreText = null;
        [SerializeField] GameObject _pack;

        private void Update()
        {
            SetScoreText();
        }

        public void AddScore(AddPointPlayer addPointPlayer)
        {
            if (addPointPlayer == AddPointPlayer.Player1)
            {
                _p1Score++;
            }
            else
            {
                _p2Score++;
            }
        }

        private void SetScoreText()
        {
            _scoreText.text = $"{_p1Score}:{_p2Score}";
        }

        public void ResetScore() 
        {
            _p1Score = 0; 
            _p2Score = 0 ;
        }

        public void GeneratePack()
        {
            Instantiate(_pack);
        }
    }
}
