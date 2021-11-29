using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public abstract class BaseDrawStrategy : IDisposable
    {
        // Param is the brush instance created from prefabs
        public UnityEvent<GameObject> DrawPhaseStarted = new UnityEvent<GameObject>();

        public abstract void Draw();
        public abstract void Dispose();

        public BaseDrawStrategy(ARCursor arCursor)
        {
            cursor = arCursor;
        }

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

        protected GameObject stroke;
        protected ARCursor cursor;
    }
}
