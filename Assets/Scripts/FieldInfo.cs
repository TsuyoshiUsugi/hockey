using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TsuyoshiLibrary
{
    public class FieldInfo : MonoBehaviour
    {
        [SerializeField] GameObject _split;
        [SerializeField] GameObject _top;
        [SerializeField] GameObject _down;
        [SerializeField] GameObject _leftGoal;
        [SerializeField] GameObject _rightGoal;

        public GameObject Split => _split;
        public GameObject Top => _top;
        public GameObject Down => _down;
        public GameObject LeftGoal => _leftGoal;
        public GameObject RightGoal => _rightGoal;
    }
}
