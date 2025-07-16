using UnityEngine;
using Verse;

namespace LevelUp;

internal sealed class FleckDefExtension : DefModExtension
{
    internal readonly float scale = 1f;
    internal readonly float rotationRate = 1f;
    internal readonly Vector3 exactScale = new(1f, 1f, 1f);
    internal readonly Color instanceColor = Color.white;
    internal readonly Vector3? velocity;
    internal readonly float velocityAngle;
    internal readonly float velocitySpeed;
    internal readonly float? solidTimeOverride;
    internal readonly float? airTimeLeft;
    internal readonly float targetSize;
}
