using System.Collections.Generic;
using GameCore.SimulationCore;
using UnityEngine;
using Utilities.SaveSystem.Data;

namespace LogicLevels
{
    public class LogicAreaGate : MonoBehaviour
    {
        [SerializeField] private Vector2 position;
        [SerializeField] private Vector2 size;
        [SerializeField] private float timeInZone;

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

        private LogicGate _myGate;
        private readonly List<LogicAreaComponent> _planetsInZone = new();

        void Start()
        {
            _myGate = LogicLevelController.Instance.AddNewGate(this);
            LogicLevelController.Instance.AreaDataList.Add(this);
            InitializeArea();

            DrawSelf();
        }

        void DrawSelf()
        {
            // will replace it with some image
            LineRenderer renderer = GetComponent<LineRenderer>();
            renderer.positionCount = 4;
            Vector3[] pos = 
            {
                position,
                (position + new Vector2(size.x, 0)),
                (position + size),
                (position + new Vector2(0, size.y))
            };
            renderer.SetPositions(pos);
        }

        void InitializeArea()
        {
            if (size.x < 0)
            {
                size.x *= -1;
                position.x -= size.x;
            }

            if (size.y < 0)
            {
                size.y *= -1;
                position.y -= size.y;
            }
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
        
            List<LogicAreaComponent> copyOf_planetsInZone = new List<LogicAreaComponent>(_planetsInZone);
            foreach (var planet in copyOf_planetsInZone)
            {
                var lastPlanetInZone = planet.PlanetComponent;
            
                // check if last planet is still in zone
                if (lastPlanetInZone.CurrentPosition.x >= position.x &&
                    lastPlanetInZone.CurrentPosition.x <= position.x + size.x &&
                    lastPlanetInZone.CurrentPosition.y >= position.y &&
                    lastPlanetInZone.CurrentPosition.y <= position.y + size.y)
                {
                    planet.Time += Time.deltaTime;
                    CheckGate(planet);
                } 
                else _planetsInZone.Remove(planet);
            }
        
            // find if there are new planets in zone
            foreach (var planet in PlanetComponentsController.Instance.AllGravityComponents)
            {
                if (planet.CurrentPosition.x >= position.x && planet.CurrentPosition.x < position.x + size.x &&
                    planet.CurrentPosition.y >= position.y && planet.CurrentPosition.y < position.y + size.y)
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
            if (detector.Time >= timeInZone) _myGate.Trigger();
        }
    }
}
