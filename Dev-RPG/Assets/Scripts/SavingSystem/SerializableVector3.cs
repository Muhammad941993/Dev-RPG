using System;
using UnityEngine;

namespace RPG.savingSystem
{
    [Serializable]
    public class SerializableVector3
    {
        private float _x, _y, _z;

        public SerializableVector3(Vector3 vector)
        {
            _x = vector.x;
            _y = vector.y;
            _z = vector.z;
        }

        public Vector3 ToVector3()
        {
            return new Vector3(_x, _y, _z);
        }
    }
}