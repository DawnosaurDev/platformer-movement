using UnityEngine;

namespace Visualization
{
    public class LineVisual : Visual, ILineDrawable, IColorChangeable
    {
        protected VisualDrawer visualDrawer;
        protected LineRenderer lineRend;

        const float LINE_LENGTH_MULT = 0.2f;

        public virtual void DrawVisual(Vector2 pos1, Vector2 pos2, Color col, float width = 0.2f)
        {
            obj = new GameObject(Name, new System.Type[] { typeof(LineRenderer) });
            lineRend = obj.GetComponent<LineRenderer>();

            lineRend.positionCount = 2;
            lineRend.material = Resources.Load<Material>("Line Material");
            lineRend.widthMultiplier = width;

            MoveVisual(pos1, pos2);
            ChangeVisualColor(col);
        }

        public virtual void MoveVisual(Vector2 pos1, Vector2 pos2)
        {
            lineRend.SetPosition(0, pos1);
            lineRend.SetPosition(1, pos2);
        }

        public virtual void ChangeVisualColor(Color col)
        {
            lineRend.startColor = col;
            lineRend.endColor = col;
        }
    }
}
