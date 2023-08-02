using GameCore.SimulationCore;

namespace LogicLevels
{
    /// <summary>
    /// It holds planet that entered trigger's area and time that is in the area.
    /// </summary>
    public class LogicAreaComponent
    {
        public PlanetComponent PlanetComponent;
        public float Time;

        public LogicAreaComponent(PlanetComponent component, float time)
        {
            PlanetComponent = component;
            this.Time = time;
        }

        public static implicit operator LogicAreaComponent(PlanetComponent component) => new (component, 0);
    }
}
