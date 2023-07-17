using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TsuyoshiLibrary
{
    /// <summary>
    /// バーホッケーの入力マネージャ
    /// 各プレイヤーのコントローラーのLRスティック入力を受け取る
    /// </summary>
    public class BarHockeyInputManager : MonoBehaviour
    {
        [SerializeField] PlayerBar _player1Bar;
        [SerializeField] PlayerBar _player2Bar;
        [SerializeField] Manager _manager;
        string _p1LeftStickVerticalInputName = "Joystick1LeftVertical";
        string _p1LeftStickHorizontalInputName = "Joystick1LeftHorizontal";
        string _p1RightStickVerticalInputName = "Joystick1RightVertical";
        string _p1RightStickHorizontalInputName = "Joystick1RightHorizontal";
        string _p2LeftStickVerticalInputName = "Joystick2LeftVertical";
        string _p2LeftStickHorizontalInputName = "Joystick2LeftHorizontal";
        string _p2RightStickVerticalInputName = "Joystick2RightVertical";
        string _p2RightStickHorizontalInputName = "Joystick2RightHorizontal";

        float lv1;  //プレイヤー１の左スティックの縦入力
        float lh1;  //プレイヤー１の左スティックの横入力
        float rv1;  //プレイヤー１の右スティックの縦入力
        float rh1;  //プレイヤー１の右スティックの横入力
        [SerializeField] float lv2;
        [SerializeField] float lh2;
        [SerializeField] float rv2;
        [SerializeField] float rh2;

        private void Update()
        {
            GetPlayersInput();
            _player1Bar.MoveMalletPosOne(lv1, lh1);
            _player1Bar.MoveMalletPosTwo(rv1, rh1);
            _player2Bar.MoveMalletPosOne(lv2, lh2);
            _player2Bar.MoveMalletPosTwo(rv2, rh2);

            GetSettingInput();
        }

        void GetPlayersInput()
        {
            lv1 = Input.GetAxis(_p1LeftStickVerticalInputName);
            lh1 = Input.GetAxis(_p1LeftStickHorizontalInputName);
            rv1 = Input.GetAxis(_p1RightStickVerticalInputName);
            rh1 = Input.GetAxis(_p1RightStickHorizontalInputName);

            lv2 = Input.GetAxis(_p2LeftStickVerticalInputName);
            lh2 = Input.GetAxis(_p2LeftStickHorizontalInputName);
            rv2 = Input.GetAxis(_p2RightStickVerticalInputName);
            rh2 = Input.GetAxis(_p2RightStickHorizontalInputName);
        }

        /// <summary>
        /// 設定関連の入力処理を受け取る
        /// </summary>
        private void GetSettingInput()
        {
            if (Input.GetKeyDown(KeyCode.U)) _manager.ResetScore();
            if (Input.GetKeyDown(KeyCode.Y)) _manager.ResetPack();
            if (Input.GetKeyDown(KeyCode.I)) _manager.GeneratePack();
            if (Input.GetKeyDown(KeyCode.O)) _manager.DestroyPack();
        }
    }
}
