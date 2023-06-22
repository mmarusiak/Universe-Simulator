namespace LogicLevels
{
    public class LogicGate
    {
        private bool _triggered = false;

        public string ID { get; set; }

        public bool Triggered => _triggered;

        public void Trigger()
        {
            _triggered = true;
            LogicLevelController.Instance.CheckGates();
        }
    }
}
