using System.Collections.Generic;
using GameCore.SimulationCore;
using UnityEngine;
using UnityEngine.UI;
using Utilities.UniverseLibraries;

namespace LogicLevels.Gates.AreaGate
{
    public class LogicAreaGate : MonoBehaviour
    {
        [SerializeField] private Vector2 position;
        [SerializeField] private Vector2 size;
        [SerializeField] private float timeInZone;

        [SerializeField] private GameObject panel;
        [SerializeField] private GameObject timer;
        [SerializeField] private Text timerText;
        [SerializeField] private GameObject timerTextGameObject;
        public Vector2 Position
        {
            get => position;
            set
            {
                DrawSelf();
                position = value;
            }
        }

        public Vector2 Size
        {
            get => size;
            set
            {
                DrawSelf();
                size = value;
            }
        }

        public float TimeInZone
        {
            get => timeInZone;
            set => timeInZone = value;
        }

        public GameObject Panel => panel;

        private LogicGate _myGate;
        private List<LogicAreaComponent> _planetsInZone = new();

        void Start()
        {
            _myGate = LogicLevelController.Instance.AddNewGate(this);
            _myGate.OnGateReset.AddListener(ResetAreaGate);
            LogicLevelController.Instance.AreaDataList.Add(this);
            if (timerText == null) CreateText();
            DrawSelf();
            GatesController.Instance.AddAreaGate(this);
        }
        
        public void DrawSelf()
        {
            transform.position = position;
            panel.transform.localScale = size;
            timer.transform.localPosition = new Vector3(size.x / 2, size.y / -2, -1);
            if (timerText == null) CreateText();
            timerTextGameObject.transform.position =
                Camera.main.WorldToScreenPoint(timer.transform.position) - new Vector3(0, timerText.fontSize * 2.25f);
        }

        void CreateText()
        {
            timerTextGameObject = Instantiate(timerTextGameObject, GameObject.Find("LogicGatesTexts").transform);
            timerTextGameObject.name = TimeInZone + " : " + position + " : " + size;
            timerText = timerTextGameObject.GetComponent<Text>();
            timerText.text = UniverseTools.RoundOutput(timeInZone, 2) + " s";
        }
        
        /// <summary>
        /// Helper void for logic area component list. It checks if planet is already in zone, or if it just entered it.
        /// </summary>
        /// <param name="planet">
        /// Planet to check.
        /// </param>
        /// <returns></returns>
        bool IsPlanetAlreadyInZone(PlanetComponent planet)
        {
            foreach (var p in _planetsInZone)
            {
                if (p.PlanetComponent == planet) return true;
            }
            return false;
        }

        void Update()
        {
            if (PlaybackController.Instance.Playback.IsPaused) return;
        
            CheckTimeForPlanets();
        }

        void CheckTimeForPlanets()
        {
            List<LogicAreaComponent> copyOf_planetsInZone = new List<LogicAreaComponent>(_planetsInZone);
            float minTime = TimeInZone;
            foreach (var planet in copyOf_planetsInZone)
            {
                // check if last planet is still in zone
                if (UniversePictures.AreSpritesOverlapping(panel.transform.GetChild(0).GetComponent<SpriteRenderer>(), planet.PlanetComponent.Renderer))
                {
                    planet.Time += Time.deltaTime;
                    if (minTime > TimeInZone - planet.Time) minTime = TimeInZone - planet.Time;
                    CheckGate(planet);
                } 
                else _planetsInZone.Remove(planet);
            }
            
            if(!_myGate.Triggered) timerText.text = UniverseTools.RoundOutput((minTime), 2) + " s";

            // find if there are new planets in zone
            foreach (var planet in PlanetComponentsController.Instance.AllGravityComponents)
            {
                if (UniversePictures.AreSpritesOverlapping(panel.transform.GetChild(0).GetComponent<SpriteRenderer>(), planet.Renderer))
                {
                    if (!IsPlanetAlreadyInZone(planet)) _planetsInZone.Add(planet);
                    break;
                }
            }
        }

        /// <summary>
        /// Checks if gate was already triggered, if planet was in area for long enough.
        /// </summary>
        /// <param name="detector">
        /// Gate area to check.
        /// </param>
        /// <returns></returns>
        void CheckGate(LogicAreaComponent detector)
        {
            // we will also probably need some indication of completion of that gate
            if (detector.Time >= timeInZone)
            {
                timerText.text = "Gate reached!";
                _myGate.Trigger();
            }
        }

        void ResetAreaGate()
        {
            _planetsInZone = new();
            timerText.text = UniverseTools.RoundOutput(timeInZone, 2) + " s";
        }
    }
}
