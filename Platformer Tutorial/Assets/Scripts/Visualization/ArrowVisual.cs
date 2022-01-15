using UnityEngine;

namespace Visualization
{
    public class ArrowVisual : Visual, ILineDrawable, IColorChangeable
    {
        protected VisualDrawer visualDrawer;
        protected LineRenderer lineRend;
        private SpriteRenderer spriteRend;

        const float LINE_LENGTH_MULT = 0.2f;

        public void DrawVisual(Vector2 pos1, Vector2 pos2, Color col, float width = 0.2f)
        {
            obj = new GameObject(Name, new System.Type[] { typeof(LineRenderer) });
            lineRend = obj.GetComponent<LineRenderer>();

            lineRend.positionCount = 2;
            lineRend.material = Resources.Load<Material>("Line Material");
            lineRend.widthMultiplier = width;

            GameObject childObj = new GameObject(Name, new System.Type[] { typeof(SpriteRenderer) });
            childObj.transform.SetParent(obj.transform);
            childObj.transform.position = pos2;

            spriteRend = childObj.GetComponent<SpriteRenderer>();
            spriteRend.sprite = Resources.Load<Sprite>("Triangle Sprite");
            spriteRend.transform.localScale = Vector3.one * 0.4f;

            MoveVisual(pos1, pos2);
            ChangeVisualColor(col);
        }

        public void MoveVisual(Vector2 pos1, Vector2 pos2)
        {
            lineRend.SetPosition(0, pos1);
            lineRend.SetPosition(1, pos2);
            MoveArrow(pos1, pos2);
        }

        private void MoveArrow(Vector2 pos1, Vector2 pos2)
        {
            Vector2 dir = pos1 - pos2;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            spriteRend.transform.position = pos2;
            spriteRend.transform.rotation = Quaternion.Euler(0, 0, angle + 90);
        }

        public void ChangeVisualColor(Color col)
        {
            lineRend.startColor = col;
            lineRend.endColor = col;
            spriteRend.color = col;
        }
    }
}
