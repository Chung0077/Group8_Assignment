using AISystem.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AISystem
{
    public class Settings : ScriptableObject
    {
        public NAV_MODE navMode;
        public float speed;
        public float angularSpeed;
        public float acceleration;
        public float stoppingDistance;


        public bool needSystemEnabled;
    }
}
