using System.Collections.Generic;
using Menu.SavesScrollbar;
using UnityEngine;
using Utilities.UniverseLibraries;

namespace Menu
{
    public class MenuSaves : MonoBehaviour
    {
        private string _pathToSaves;
        [SerializeField] private GameObject saveContainerPrefab;
        private List<SaveContainer> _containers = new ();
        [SerializeField] private Transform savesParent;

        void Start()
        {
            _pathToSaves = Application.persistentDataPath + "/Saves";
            UniverseDirectories.CreateDirectoryIfNotExists(_pathToSaves);
            InitializeSavesLoader();
        }

        async void InitializeSavesLoader()
        {
            savesParent.gameObject.SetActive(false);
            var saves = UniverseDirectories.GetFoldersInDirectory(_pathToSaves);
            foreach (var saveName in saves)
            {
                SaveContainer container = Instantiate(saveContainerPrefab,  savesParent).GetComponent<SaveContainer>();
                await container.Initialize(saveName);
                _containers.Add(container);
            }
            SortSaves();
            savesParent.gameObject.SetActive(true);
        }

        /// <summary>
        /// Sorts saves by dates.
        /// </summary>
        void SortSaves()
        {
            _containers.Sort((x, y) => y.LastModified.CompareTo(x.LastModified));
            for (int i = 0; i < _containers.Count; i++)
            {
                _containers[i].GetComponent<RectTransform>().position = new Vector3(400, 550 - 350 * i);
            }
        }
    }
}
