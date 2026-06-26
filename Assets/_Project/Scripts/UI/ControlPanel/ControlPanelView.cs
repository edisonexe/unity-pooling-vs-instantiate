using UnityEngine;
using UnityEngine.UI;

namespace PoolingBenchmark.UI.ControlPanel
{
    [AddComponentMenu("PoolingBenchmark/UI/Control Panel View")]
    public class ControlPanelView : MonoBehaviour
    {
        [SerializeField] private GameObject _contentRoot;
        [SerializeField] private Button _toggleModeBtn;

        public Button ToggleModeBtn => _toggleModeBtn;

        public void Show() => _contentRoot.SetActive(true);
        public void Hide() => _contentRoot.SetActive(false);
        
        private void OnValidate()
        {
            if (!_contentRoot) Debug.LogError("[ControlPanelView] Content Root is null", this);
            if (!_toggleModeBtn) Debug.LogError("[ControlPanelView] ToggleModeBtn is null", this);
        }
    }
}