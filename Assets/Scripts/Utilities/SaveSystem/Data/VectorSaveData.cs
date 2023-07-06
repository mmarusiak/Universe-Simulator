using Newtonsoft.Json;
using UnityEngine;

namespace Utilities.SaveSystem.Data
{
    public class VectorSaveData
    {
        [JsonProperty]
        private float X { get; set; }
        [JsonProperty]
        private float Y { get; set; }

        [JsonConstructor]
        public VectorSaveData(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static implicit operator Vector2 (VectorSaveData data) => new(data.X, data.Y);
        public static implicit operator Vector3 (VectorSaveData data) => new(data.X, data.Y);
        public static implicit operator VectorSaveData(Vector2 vec) => new(vec.x, vec.y);
    }
}
