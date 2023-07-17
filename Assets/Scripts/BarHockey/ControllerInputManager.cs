using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TsuyoshiLibrary
{
    /// <summary>
    /// �e�v���C���[�̃R���g���[���[��LR�X�e�B�b�N���͂��󂯎��}�l�[�W��
    /// </summary>
    public class ControllerInputManager : MonoBehaviour
    {
        [SerializeField] string _p1LeftStickVerticalInputName;
        [SerializeField] string _p1LeftStickHorizontalInputName;
        [SerializeField] string _p1RightStickVerticalInputName;
        [SerializeField] string _p1RightStickHorizontalInputName;
        [SerializeField] PlayerBar _player1Bar;
        [SerializeField] PlayerBar _player2Bar;

        private void Update()
        {
            var lv = Input.GetAxis(_p1LeftStickVerticalInputName);
            var lh = Input.GetAxis(_p1LeftStickHorizontalInputName);
            var rv = Input.GetAxis(_p1RightStickVerticalInputName);
            var rh = Input.GetAxis(_p1RightStickHorizontalInputName);
            _player1Bar.MoveMalletPosOne(lv, lh);
            _player1Bar.MoveMalletPosTwo(rv, rh);
        }
    }
}
