using TMPro;
using UnityEngine;

namespace PoolingBenchmark.UI.SimulationStatsPanel
{
    [AddComponentMenu("PoolingBenchmark/UI/Simulation Stats View")]
    public sealed class SimulationStatsView : MonoBehaviour
    {
        [Header("UI Containers")]
        [SerializeField] private GameObject _contentRoot;
        [SerializeField] private GameObject[] _naiveOnlyRows;
        [SerializeField] private GameObject[] _poolOnlyRows;

        [Header("Text Rows")]
        [SerializeField] private TMP_Text _execModeText;
        [SerializeField] private TMP_Text _fpsText;
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


        public GameObject ContentRoot => _contentRoot;
        public GameObject[] NaiveOnlyRows => _naiveOnlyRows;
        public GameObject[] PoolOnlyRows => _poolOnlyRows;
        
        public TMP_Text ExecModeText => _execModeText;
        public TMP_Text FPSText => _fpsText;
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

        public void Show() => _contentRoot.SetActive(true);
        public void Hide() => _contentRoot.SetActive(false);

        private void OnValidate()
        {
            if (!_contentRoot) Debug.LogError("[SimulationStatsView] Stats Panel GameObject reference is missing!", this);
            if (!_execModeText) Debug.LogError("[SimulationStatsView] ExecModeText reference is missing!", this);
            if (!_activeProjsText) Debug.LogError("[SimulationStatsView] ActiveProjsText reference is missing!", this);
            if (!_activeTargetsText) Debug.LogError("[SimulationStatsView] ActiveTargetsText reference is missing!", this);
            if (!_totalCreatedProjsText) Debug.LogError("[SimulationStatsView] TotalCreatedProjsText reference is missing!", this);
            if (!_totalCreatedTargetsText) Debug.LogError("[SimulationStatsView] TotalCreatedTargetsText reference is missing!", this);
            if (!_projsPoolSizeText) Debug.LogError("[SimulationStatsView] ProjsPoolSizeText reference is missing!", this);
            if (!_targetsPoolSizeText) Debug.LogError("[SimulationStatsView] TargetsPoolSizeText reference is missing!", this);
            if (!_availableProjsText) Debug.LogError("[SimulationStatsView] AvailableProjsText reference is missing!", this);
            if (!_availableTargetsText) Debug.LogError("[SimulationStatsView] AvailableTargetsText reference is missing!", this);
            if (!_reusedProjsText) Debug.LogError("[SimulationStatsView] ReusedProjsText reference is missing!", this);
            if (!_reusedTargetsText) Debug.LogError("[SimulationStatsView] ReusedTargetsText reference is missing!", this);
            if (!_fpsText)  Debug.LogError("[SimulationStatsView] FPSText reference is missing!", this);
            
            if (_naiveOnlyRows == null || _naiveOnlyRows.Length == 0) 
                Debug.LogError("[SimulationStatsView] Naive Only Rows array is unassigned or empty!", this);
                
            if (_poolOnlyRows == null || _poolOnlyRows.Length == 0) 
                Debug.LogError("[SimulationStatsView] Pool Only Rows array is unassigned or empty!", this);
        }
    }
}