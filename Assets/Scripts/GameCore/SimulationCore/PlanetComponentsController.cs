using System.Collections.Generic;
using GameCore.SimulationCore.Handlers;
using LogicLevels;
using UnityEngine;

namespace GameCore.SimulationCore
{
    public class PlanetComponentsController : MonoBehaviour
    {
        public static PlanetComponentsController Instance;
        private void Awake() => Instance = this;
    
        private List<PlanetComponent> _allGravityComponents = new ();

        public List<PlanetComponent> AllGravityComponents
        {
            get => _allGravityComponents;
            set => _allGravityComponents = value;
        }

        [SerializeField] private Transform _planetsHolder;
        [SerializeField] private GameObject _planetPrefab, _loadPrefab;

        public void AddNewGravityComponent(PlanetComponent gravityComponent)
        {
            if (_allGravityComponents.Contains(gravityComponent)) return;
            gravityComponent.OtherComponents = new List<PlanetComponent>(_allGravityComponents);
            foreach (var createdComponent in _allGravityComponents)
                createdComponent.AddGravityComponent(gravityComponent);

            _allGravityComponents.Add(gravityComponent);
        }

        void RemovePlanet(PlanetComponent planetComponent) => _allGravityComponents.Remove(planetComponent);

        void RemovePlanetOnPlanets(PlanetComponent planetComponent)
        {
            foreach (var createdComponent in planetComponent.OtherComponents)
                createdComponent.OtherComponents.Remove(planetComponent);
        }
        public void ResetLevel()
        {
            DestroyClones();
            foreach (var component in _allGravityComponents) component.Reset();
        }

        void DestroyClones()
        {
            var copyList = new List<PlanetComponent>(_allGravityComponents);
            foreach (var component in copyList)
            {
                if (!component.IsOriginalPlanet)
                {
                    RemovePlanetOnPlanets(component);
                    RemovePlanet(component);
                    component.DestroySelf();
                }
            }
        }

        public void ClearLevel()
        {
            foreach (var component in _allGravityComponents) Destroy(component.Handler.gameObject);
            _allGravityComponents = new();
        }

        public GameObject CreatePlanet()
        {
            var newPlanet = Instantiate(_planetPrefab, Camera.main.ScreenToWorldPoint(Input.mousePosition),
                Quaternion.Euler(0, 0, 0), _planetsHolder);

            if (LogicLevelController.Instance != null && !LogicLevelController.Instance.IsLevelInEditMode)
            {
                if (LogicLevelController.Instance.PlanetActions > 0)
                    LogicLevelController.Instance.PlanetCreated();
                else if (LogicLevelController.Instance.PlanetActions == 0)
                {
                    Destroy(newPlanet);
                    return null;
                }

                newPlanet.transform.GetChild(0).GetComponent<PlanetComponentHandler>().isCreatedByPlayerInLogicLevel = true;
            }

            return newPlanet;
        }
        public GameObject LoadPlanet()
        {
            return Instantiate(_loadPrefab, Vector3.zero, Quaternion.Euler(0, 0, 0), _planetsHolder);
        }

        public void DestroyPlanet(PlanetComponentHandler handler)
        {
            if (LogicLevelController.Instance != null && !LogicLevelController.Instance.IsLevelInEditMode)
            {
                if (handler.isCreatedByPlayerInLogicLevel)
                {
                    LogicLevelController.Instance.PlanetRemoved();
                }
                else return;
            }

            RemovePlanetOnPlanets(handler.MyComponent);
            RemovePlanet(handler.MyComponent);
            Destroy(handler.transform.parent.gameObject);
        }
    }
}
