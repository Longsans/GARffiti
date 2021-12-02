using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public abstract class BaseDrawStrategy : IDisposable
    {
        // Param is the brush instance created from prefabs
        public UnityEvent<Stroke> DrawPhaseStarted = new UnityEvent<Stroke>();
        public UnityEvent<Stroke> DrawPhaseEnded = new UnityEvent<Stroke>();

        public virtual bool DrawStart(Vector2 cursorPos)
        {
            DrawPhaseStarted.Invoke(_stroke);
            return true;
        }
        public virtual void Draw(Vector2 cursorPos)
        {
            UpdateCursorPosition(cursorPos);
            _stroke.DrawTo(cursor.transform.position);
        }
        public virtual void DrawEnd()
        {
            _stroke?.Finished();
            DrawPhaseEnded.Invoke(_stroke);
            _stroke = null;
        }

        public abstract void Dispose();

        public BaseDrawStrategy(ARCursor arCursor)
        {
            cursor = arCursor;
        }

        protected abstract void UpdateCursorPosition(Vector2 position);
        protected Material CreateMaterial()
        {
            if (Settings.Texture != null)
            {
                Material material = new Material(Resources.Load<Material>("Materials/Stroke Std. Material"));
                material.color = Settings.BrushColor;
                return material;
            }
            else
            {
                return new Material(Resources.Load<Material>("Materials/Stroke Material"));
            }
        }

        protected Stroke _stroke;
        protected ARCursor cursor;
    }
}
