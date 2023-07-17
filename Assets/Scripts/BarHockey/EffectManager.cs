using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TsuyoshiLibrary
{
    public class EffectManager : MonoBehaviour
    {
        [SerializeField] ParticleSystem _hitWallParticle;
        [SerializeField] ParticleSystem _goalParticle;

        public void GenerateParticle(Vector3 pos, ParticleType type)
        {
            switch (type)
            {
                case ParticleType.Hit:

                    break;
            }
        }

        public enum ParticleType
        {
            Hit,
            Goal,
        }
    }
}
