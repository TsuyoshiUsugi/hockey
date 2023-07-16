using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TsuyoshiLibrary
{
    /// <summary>
    /// キーボードの入力を受け取るマネージャー
    /// 1P2Pの入力を受け取る
    /// Uでリセット
    /// Iでpack数UP
    /// Oでpack数Down
    /// </summary>
    public class KeyboardInputManager : MonoBehaviour
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
            GetPlayersMoveInput();
            CallPlayersMoveMethod();
            GetSettingInput();
        }

        /// <summary>
        /// 設定関連の入力処理を受け取る
        /// </summary>
        private void GetSettingInput()
        {
            if (Input.GetKeyDown(KeyCode.U))　_manager.Reset();
            if (Input.GetKeyDown(KeyCode.I)) _manager.GeneratePack();
            if (Input.GetKeyDown(KeyCode.O))　_manager.DestroyPack();
        }

        /// <summary>
        /// 各playerscriptの移動処理を呼び出す
        /// </summary>
        private void CallPlayersMoveMethod()
        {
            _player1.MovePos(_v1, _h1);
            _player2.MovePos(_v2, _h2);
        }

        /// <summary>
        /// 各プレイヤーのインプットを受け取る
        /// </summary>
        private void GetPlayersMoveInput()
        {
            _h1 = Input.GetAxis(_p1HInputName);
            _v1 = Input.GetAxis(_p1VInputName);
            _h2 = Input.GetAxis(_p2HInputName);
            _v2 = Input.GetAxis(_p2VInputName);
        }
    }
}
