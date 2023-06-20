using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities.UniverseLibraries.Timer;

namespace Utilities.UniverseLibraries
{
    [RequireComponent(typeof(TimersController))]
    public class UniverseScenes : MonoBehaviour
    {
        public static UniverseScenes Instance;
        [SerializeField] private int timeBetweenNextTips;
        [SerializeField] private Text tipsText;
        private static string _targetScene;
        private UniverseTimer _timer = new ();

        private string[] _tips =
        {
            "Loading level with saved slices will simulate a bit different.",
            "Share your level to workshop to gain stars and fame.",
            "Complete all campaign levels to get some achievements.",
            "Share your feedback with me on Discord or Steam reviews.",
            "You can add some image to your planet by editing it in view editor."
        };

        public void Awake() => Instance = this;

        async void Start()
        {
            tipsText.text = NextTip();
            await LoadSceneAsync(_targetScene);
        }

        public static void LoadScene(string target)
        {
            _targetScene = target;
            SceneManager.LoadScene("Scenes/LoadingScreen");
        }
        
        public async Task LoadSceneAsync(string scene)
        {
            AsyncOperation loader = SceneManager.LoadSceneAsync(scene);
            loader.allowSceneActivation = false;
            TimersController.Instance.StartTimer(_timer);

            while (!loader.isDone)
            {
                await LoadNextTip();

                if (loader.progress >= 0.9f) loader.allowSceneActivation = true;
                
                await Task.Yield();
            }
        }

        private async Task LoadNextTip()
        {
            if (_timer.Time < timeBetweenNextTips) return;

            // Fade out
            int fader = 0;
            while (fader < 40)
            {
                tipsText.color = new Color(1, 1, 1, 1 / fader);
                fader++;
                await Task.Yield();
            }

            tipsText.text = NextTip();

            // Fade in
            while (fader > 0)
            {
                tipsText.color = new Color(1, 1, 1, 1 / fader);
                fader--;
                await Task.Yield();
            }
            
            TimersController.Instance.StartTimer(_timer);
        }

        private string NextTip() => _tips[Random.Range(0, _tips.Length)];
        
    }
}
