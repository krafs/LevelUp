using UnityEngine;
using Verse;

namespace LevelUp
{
    public interface IAnimation : IExposable
    {
        string Label { get; }

        void Prepare();

        void Execute(LevelingInfo levelingInfo);

        void DrawGraphic(Rect rect);
    }
}