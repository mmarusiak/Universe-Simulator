using LogicLevels;
using LogicLevels.Gates.AreaGate;
using Newtonsoft.Json;

namespace Utilities.SaveSystem.Data
{
    public class LogicAreaData
    {
        [JsonProperty] public VectorSaveData Position, Size;
        [JsonProperty] public float TimeInZone;

        [JsonConstructor]
        public LogicAreaData(VectorSaveData pos, VectorSaveData size, float timeInZone)
        {
            Position = pos;
            Size = size;
            TimeInZone = timeInZone;
        }

        public LogicAreaData(LogicAreaGate gate)
        {
            Position = gate.Position;
            Size = gate.Size;
            TimeInZone = gate.TimeInZone;
        }

        public static implicit operator LogicAreaData(LogicAreaGate data) => new (data);
    }
}