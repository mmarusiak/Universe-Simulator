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

        /// <summary>
        /// Adds new planet component to controller's list.
        /// </summary>
        /// <param name="planetComponent">Component to be added.</param>
        public void AddNewPlanetComponent(PlanetComponent planetComponent)
        {
            if (_allGravityComponents.Contains(planetComponent)) return;
            planetComponent.OtherComponents = new List<PlanetComponent>(_allGravityComponents);
            foreach (var createdComponent in _allGravityComponents)
                createdComponent.AddGravityComponent(planetComponent);

            _allGravityComponents.Add(planetComponent);
        }

        /// <summary>
        /// Removes planet component from controller's list.
        /// </summary>
        /// <param name="planetComponent">Component to be removed</param>
        void RemovePlanet(PlanetComponent planetComponent) => _allGravityComponents.Remove(planetComponent);

        /// <summary>
        /// Removes planet component from every planet component' local lists.
        /// </summary>
        /// <param name="planetComponent">Component to be removed.</param>
        void RemovePlanetOnPlanets(PlanetComponent planetComponent)
        {
            foreach (var createdComponent in planetComponent.OtherComponents)
                createdComponent.OtherComponents.Remove(planetComponent);
        }
        
        /// <summary>
        /// Resets level
        /// </summary>
        public void ResetLevel()
        {
            DestroyClones();
            foreach (var component in _allGravityComponents) component.Reset();
        }
        
        /// <summary>
        /// Destroys all clones (slices).
        /// </summary>
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

        /// <summary>
        /// Creates new planet.
        /// </summary>
        /// <returns>New planet's Game Object or null if planet wasn't created (f.e. in logic levels).</returns>
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
        
        /// <summary>
        /// Loads planet when level is loaded from menu.
        /// </summary>
        /// <returns>Loaded planet Game Object.</returns>
        public GameObject LoadPlanet()
        {
            return Instantiate(_loadPrefab, Vector3.zero, Quaternion.Euler(0, 0, 0), _planetsHolder);
        }

        /// <summary>
        /// Destroys planet - removes from every list and destroys planet's Game Object.
        /// </summary>
        /// <param name="handler">Planet Component Handler to be destroyed.</param>
        public void DestroyPlanet(PlanetComponentHandler handler)
        {
            EditorsController.Instance.PlanetDestroyed(handler.MyComponent);
            if (LogicLevelController.Instance != null && !LogicLevelController.Instance.IsLevelInEditMode)
            {
                if (handler.isCreatedByPlayerInLogicLevel)
                {
                    LogicLevelController.Instance.PlanetRemoved();
                }
                else
                {
                    EditorsController.Instance.LastEditedComponent = handler.MyComponent;
                    return;
                }
            }

            RemovePlanetOnPlanets(handler.MyComponent);
            RemovePlanet(handler.MyComponent);
            Destroy(handler.transform.parent.gameObject);
        }
    }
}
