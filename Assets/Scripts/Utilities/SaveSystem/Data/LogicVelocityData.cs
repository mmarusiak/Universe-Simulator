using LogicLevels;
using Newtonsoft.Json;

namespace Utilities.SaveSystem.Data
{
    public class LogicVelocityData
    {
        [JsonProperty] public float TargetVelocity;

        [JsonConstructor]
        public LogicVelocityData(float target)
        {
            TargetVelocity = target;
        }

        public LogicVelocityData(LogicVelocityGate gate)
        {
            TargetVelocity = gate.Velocity;
        }

        public static implicit operator LogicVelocityData(LogicVelocityGate gate) => new(gate);
    }
}