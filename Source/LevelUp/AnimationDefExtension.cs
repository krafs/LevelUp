using System;
using UnityEngine;
using Verse;

namespace LevelUp
{
    [Serializable]
    public class AnimationDefExtension : DefModExtension
    {
        private readonly float scale = 1f;
        private readonly float rotation;
        private readonly float rotationRate = 1f;
        private readonly Vector3 exactScale = new Vector3(1f, 1f, 1f);
        private readonly Color instanceColor = Color.white;
        private readonly Vector3? velocity;
        private readonly float velocityAngle;
        private readonly float velocitySpeed;
        private readonly float? solidTimeOverride;
        private readonly float? airTimeLeft;
        private readonly float targetSize;

        public float Scale => scale;
        public float Rotation => rotation;
        public float RotationRate => rotationRate;
        public Vector3 ExactScale => exactScale;
        public Color InstanceColor => instanceColor;
        public Vector3? Velocity => velocity;
        public float VelocityAngle => velocityAngle;
        public float VelocitySpeed => velocitySpeed;
        public float? SolidTimeOverride => solidTimeOverride;
        public float? AirTimeLeft => airTimeLeft;
        public float TargetSize => targetSize;
    }
}