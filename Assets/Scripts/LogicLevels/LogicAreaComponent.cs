using GameCore.SimulationCore;

public class LogicAreaComponent
{
    public PlanetComponent PlanetComponent;
    public float time;

    public LogicAreaComponent(PlanetComponent component, float time)
    {
        PlanetComponent = component;
        this.time = time;
    }

    public static implicit operator LogicAreaComponent(PlanetComponent component) => new (component, 0);
}
