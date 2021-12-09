﻿using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public abstract class BaseDrawStrategy : IDisposable
    {
        // Param is the brush instance created from prefabs
        public UnityEvent<Stroke> DrawPhaseStarted = new UnityEvent<Stroke>();
        public UnityEvent<Stroke> DrawPhaseEnded = new UnityEvent<Stroke>();

        private bool _drawingStarted = false;

        #region Drawing
        public virtual bool DrawStart(Vector2 cursorPos)
        {
            DrawPhaseStarted.Invoke(_stroke);
            _drawingStarted = true;
            return _drawingStarted;
        }
        public virtual void Draw(Vector2 cursorPos)
        {
            if (!_drawingStarted)
                return;

            UpdateCursorPosition(cursorPos);
            _stroke.DrawTo(cursor.transform.position);
        }
        public virtual void DrawEnd()
        {
            if (!_drawingStarted)
                return;

            _stroke?.Finished();
            DrawPhaseEnded.Invoke(_stroke);
            _stroke = null;

            _drawingStarted = false;
        }
        #endregion

        #region 3D model manipulation
        public virtual bool PlacingStarted(Vector2 cursorPos)
        {
            _placingStarted = true;

            GameObject newObj = GameObject.Instantiate(Settings.Selected3DModel, cursor.transform.position, Quaternion.identity);
            _modelScript = newObj.GetComponent<ModelScript>();
            _modelScript.MoveTo(cursor.transform.position);

            return _placingStarted;
        }
        public virtual void PlacingMove(Vector2 cursorPos)
        {
            if (!_placingStarted)
                return;

            UpdateCursorPosition(cursorPos);
            _modelScript.MoveTo(cursor.transform.position);
        }
        public virtual void PlacingEnded()
        {
            if (!_placingStarted)
                return;

            _modelScript = null;
            _placingStarted = false;
        }
        #endregion

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
        protected ModelScript _modelScript;
        protected bool _placingStarted;
    }
}
