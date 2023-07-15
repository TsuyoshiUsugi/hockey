using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TsuyoshiLibrary
{
    public class Manager : MonoBehaviour
    {
        [SerializeField] int _p1Score = 0;
        [SerializeField] int _p2Score = 0;

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

        public void ResetScore(int playerNum) 
        {
            _p1Score = 0; 
            _p2Score = 0 ;
        }
    }
}
