using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TsuyoshiLibrary
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] Player _player1;
        [SerializeField] Player _player2;
        string _p1HInputName = "Horizontal";
        string _p1VInputName = "Vertical";
        string _p2HInputName = "Horizontal1";
        string _p2VInputName = "Vertical1";
        float _h1;
        float _v1;
        float _h2;
        float _v2;

        Manager _manager;

        private void Start()
        {
            _manager = FindObjectOfType<Manager>();
        }

        private void Update()
        {
            _h1 = Input.GetAxis(_p1HInputName);
            _v1 = Input.GetAxis(_p1VInputName);
            _h2 = Input.GetAxis(_p2HInputName);
            _v2 = Input.GetAxis(_p2VInputName);
            
            _player1.MovePos(_v1, _h1);
            
            _player2.MovePos(_v2, _h2);

            if (Input.GetKeyDown(KeyCode.U))
            {
                _manager.ResetScore();
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                _manager.GeneratePack();
            }
        }



    }
}
