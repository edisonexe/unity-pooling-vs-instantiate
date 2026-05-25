using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

namespace PoolingBenchmark.UI
{
    [AddComponentMenu("PoolingBenchmark/UI/Stress Test UI View")]
    public sealed class StressTestUIView : MonoBehaviour, IInitializable
    {
        [Header("UI Containers")]
        [SerializeField] private GameObject _statsPanel;
        [SerializeField] private GameObject[] _naiveOnlyRows;
        [SerializeField] private GameObject[] _poolOnlyRows;

        [Header("Text Rows")]
        [SerializeField] private TMP_Text _execModeText;
        [SerializeField] private TMP_Text _activeProjsText;
        [SerializeField] private TMP_Text _activeTargetsText;
        [SerializeField] private TMP_Text _totalCreatedProjsText;
        [SerializeField] private TMP_Text _totalCreatedTargetsText;
        [SerializeField] private TMP_Text _projsPoolSizeText;
        [SerializeField] private TMP_Text _targetsPoolSizeText;
        [SerializeField] private TMP_Text _availableProjsText;
        [SerializeField] private TMP_Text _availableTargetsText;
        [SerializeField] private TMP_Text _reusedProjsText;
        [SerializeField] private TMP_Text _reusedTargetsText;

        [Header("Settings")]
        [SerializeField] private Button _toggleBtn;

        public GameObject StatsPanel => _statsPanel;
        public GameObject[] NaiveOnlyRows => _naiveOnlyRows;
        public GameObject[] PoolOnlyRows => _poolOnlyRows;
        
        public TMP_Text ExecModeText => _execModeText;
        public TMP_Text ActiveProjsText => _activeProjsText;
        public TMP_Text ActiveTargetsText => _activeTargetsText;
        public TMP_Text TotalCreatedProjsText => _totalCreatedProjsText;
        public TMP_Text TotalCreatedTargetsText => _totalCreatedTargetsText;
        public TMP_Text ProjsPoolSizeText => _projsPoolSizeText;
        public TMP_Text TargetsPoolSizeText => _targetsPoolSizeText;
        public TMP_Text AvailableProjsText => _availableProjsText;
        public TMP_Text AvailableTargetsText => _availableTargetsText;
        public TMP_Text ReusedProjsText => _reusedProjsText;
        public TMP_Text ReusedTargetsText => _reusedTargetsText;
        
        public Button ToggleBtn => _toggleBtn;

        public void Initialize()
        {
            ValidateInspector();
            
            if (_statsPanel) _statsPanel.SetActive(false);
        }
        
        private void ValidateInspector()
        {
            if (!_statsPanel) Debug.LogError("[StressTestUIView] Stats Panel GameObject reference is missing!", this);
            if (!_execModeText) Debug.LogError("[StressTestUIView] ExecModeText reference is missing!", this);
            if (!_activeProjsText) Debug.LogError("[StressTestUIView] ActiveProjsText reference is missing!", this);
            if (!_activeTargetsText) Debug.LogError("[StressTestUIView] ActiveTargetsText reference is missing!", this);
            if (!_totalCreatedProjsText) Debug.LogError("[StressTestUIView] TotalCreatedProjsText reference is missing!", this);
            if (!_totalCreatedTargetsText) Debug.LogError("[StressTestUIView] TotalCreatedTargetsText reference is missing!", this);
            if (!_projsPoolSizeText) Debug.LogError("[StressTestUIView] ProjsPoolSizeText reference is missing!", this);
            if (!_targetsPoolSizeText) Debug.LogError("[StressTestUIView] TargetsPoolSizeText reference is missing!", this);
            if (!_availableProjsText) Debug.LogError("[StressTestUIView] AvailableProjsText reference is missing!", this);
            if (!_availableTargetsText) Debug.LogError("[StressTestUIView] AvailableTargetsText reference is missing!", this);
            if (!_reusedProjsText) Debug.LogError("[StressTestUIView] ReusedProjsText reference is missing!", this);
            if (!_reusedTargetsText) Debug.LogError("[StressTestUIView] ReusedTargetsText reference is missing!", this);
            if (!_toggleBtn) Debug.LogError("[StressTestUIView] ToggleBtn reference is missing!", this);
            
            if (_naiveOnlyRows == null || _naiveOnlyRows.Length == 0) 
                Debug.LogError("[StressTestUIView] Naive Only Rows array is unassigned or empty!", this);
                
            if (_poolOnlyRows == null || _poolOnlyRows.Length == 0) 
                Debug.LogError("[StressTestUIView] Pool Only Rows array is unassigned or empty!", this);
        }
    }
}