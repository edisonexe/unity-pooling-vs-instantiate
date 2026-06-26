using UnityEngine;
using UnityEngine.UI;

namespace PoolingBenchmark.UI.StartScreen
{
    [AddComponentMenu("PoolingBenchmark/UI/Start Screen View")]
    public class StartScreenView : MonoBehaviour
    {
        [SerializeField] private GameObject _contentRoot;
        [SerializeField] private Button _startBenchmarkBtn;
        
        public Button StartBenchmarkBtn => _startBenchmarkBtn;
        
        public void Show() => _contentRoot.SetActive(true);
        public void Hide() => _contentRoot.SetActive(false);

        private void OnValidate()
        {
            if (!_contentRoot) Debug.LogError("[StartScreenView] ContentRoot is null", this);
            if (!_startBenchmarkBtn) Debug.LogError("[StartScreenView] StartBenchmarkBtn is null", this);
        }
    }
}