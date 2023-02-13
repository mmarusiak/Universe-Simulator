public class BasicPlanetEditor : PlanetEditor
{
    public static BasicPlanetEditor Instance;

    private void Awake() => Instance = this;
}
