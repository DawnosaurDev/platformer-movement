using UnityEngine;

namespace Visualization
{
    public interface ILineDrawable
    {
        public void DrawVisual(Vector2 pos1, Vector2 pos2, Color lineColor, float lineWidth = 0.2f);
        public void MoveVisual(Vector2 pos1, Vector2 pos2);
    }
}
