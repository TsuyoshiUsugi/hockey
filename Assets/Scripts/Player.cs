using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TsuyoshiLibrary
{
    public class Player : MonoBehaviour
    {
        [SerializeField] float _speed = 0.1f;
        [SerializeField] OwnerPlayer _player;
        [SerializeField] GameObject _split;
        [SerializeField] GameObject _top;
        [SerializeField] GameObject _down;
        [SerializeField] GameObject _goal;
        float _offset = 1;

        private void Start()
        {
            _offset = transform.localScale.x;
        }

        private void Update()
        {
            ClampPos();
        }

        private void ClampPos()
        {
            if (_player == OwnerPlayer.Player1)
            {
                float clampedX = Mathf.Clamp(transform.position.x, _goal.transform.position.x + _offset, _split.transform.position.x - _offset);
                transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);

            }
            else
            {
                float clampedX = Mathf.Clamp(transform.position.x, _split.transform.position.x + _offset, _goal.transform.position.x - _offset);
                transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
            }

            float clampedZ = Mathf.Clamp(transform.position.z, _down.transform.position.z + _offset, _top.transform.position.z - _offset);
            transform.position = new Vector3(transform.position.x, transform.position.y, clampedZ);
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
