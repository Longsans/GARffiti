using System;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class BaseDrawStrategy : IDisposable
    {
        public abstract void UpdateCursorPosition();
        public abstract void Draw();
        public abstract void Dispose();

        public BaseDrawStrategy(ARCursor arCursor)
        {
            cursor = arCursor;
        }

        protected GameObject stroke;
        protected ARCursor cursor;
    }
}
