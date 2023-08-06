using UnityEngine;

namespace LogicLevels.AreaActions
{
    public class AreaGate
    {
        private Vector2 _position, _size;

        public AreaGate(Vector2 pos, Vector2 size)
        {
            this._position = pos;
            this._size = size;
        }

        public Vector2 Position => _position;
        public Vector2 Size => _size;

        public void UpdatePosition(Vector2 position) => this._position = position;
        public void UpdateSize(Vector2 size) => this._size = size;
        
        /// <summary>
        /// Resizing area by moving mouse
        /// </summary>
        /// <param name="mousePosDelta">Initial position of mouse when started dragging - end position of mouse when drag ended.</param>
        public void Resize(Vector2 mousePosDelta)
        {
            
        }
        
        public void UpdateSizeAndPosition(Vector2 position, Vector2 size)
        {
            UpdatePosition(position);
            UpdateSize(size);
        }
        
        
    }
}