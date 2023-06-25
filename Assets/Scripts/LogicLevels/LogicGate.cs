namespace LogicLevels
{
    public class LogicGate
    {
        private bool _triggered = false;

        public string ID { get; set; }

        public bool Triggered => _triggered;

        /// <summary>
        /// Triggers gate, if condition was met.
        /// </summary>
        public void Trigger()
        {
            _triggered = true;
            LogicLevelController.Instance.CheckGates();
        }

        public void Reset() => _triggered = false;
    }
}
