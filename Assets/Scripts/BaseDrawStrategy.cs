using UnityEngine;

namespace Assets.Scripts
{
    public abstract class BaseDrawStrategy
    {
        public abstract void UpdateCursorPosition();
        public abstract void Draw();

        public BaseDrawStrategy(ARCursor arCursor)
        {
            cursor = arCursor;
        }

        protected GameObject stroke;
        protected ARCursor cursor;
    }
}
