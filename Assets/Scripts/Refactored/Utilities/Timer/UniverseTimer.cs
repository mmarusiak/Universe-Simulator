public class UniverseTimer
{
    private float _time;

    public float Time
    {
        get => _time;
        set => SetTimer(value);
    }

    void SetTimer(float value)
    {
        _time = value;
    }
}
