using System.Collections.Generic;
using GameCore.SimulationCore;
using UnityEngine;
using Utilities.UniverseLibraries;

namespace LogicLevels
{
    public class LogicAreaGate : MonoBehaviour
    {
        [SerializeField] private Vector2 position;
        [SerializeField] private Vector2 size;
        [SerializeField] private float timeInZone;

        [SerializeField] private GameObject panel;
        [SerializeField] private GameObject timer;
        [SerializeField] private SpriteRenderer timerSprite;
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

            DrawSelf();
        }

        void DrawSelf()
        {
            transform.position = position;
            panel.transform.localScale = size;
            timer.transform.localPosition = new Vector3(size.x / 2, size.y / -2, -1);
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
            float maxTime = 0;
            foreach (var planet in copyOf_planetsInZone)
            {
                // check if last planet is still in zone
                if (UniversePictures.AreSpritesOverlapping(panel.transform.GetChild(0).GetComponent<SpriteRenderer>(), planet.PlanetComponent.Renderer))
                {
                    planet.Time += Time.deltaTime;
                    if (maxTime < planet.Time) maxTime = planet.Time;
                    CheckGate(planet);
                } 
                else _planetsInZone.Remove(planet);
            }
            
            timerSprite.color = maxTime == 0 ? Color.white : new Color(1, 1, 1, 1 - maxTime/timeInZone);
        
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
            if (detector.Time >= timeInZone) _myGate.Trigger();
        }
    }
}
