using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TsuyoshiLibrary
{
    public class Player : MonoBehaviour
    {
        [SerializeField] float _speed = 0.1f;

        private void Update()
        {
            CheckPos();
        }

        private void CheckPos()
        {
        }

        public void MovePos(float v, float h)
        {
            this.transform.position += new Vector3(h, 0, v) * _speed;
        }

        public void SetPos(Vector3 pos)
        {
            this.transform.position = pos;
        }
    }
}
