using UnityEngine;
using UnityEngine.UI;

namespace RLD
{
    public class RTPrefabLibDbUI : MonoBehaviour
    {
        private RTActiveLibDropDown _activeLibDropDown;
        private RTPrefabScrollView _prefabScrollView;
        private RTHoveredPrefabNameLabel _hoveredPrefabNameLabel;
        private RTPrefabSearch _prefabSearch;

        public RTActiveLibDropDown ActiveLibDropDown { get { return _activeLibDropDown; } }
        public RTPrefabScrollView PrefabScrollView { get { return _prefabScrollView; } }
        public RTHoveredPrefabNameLabel HoveredPrefabNameLabel { get { return _hoveredPrefabNameLabel; } }
        public RTPrefabSearch PrefabSearch { get { return _prefabSearch; } }

        private void Awake()
        {
            _activeLibDropDown = gameObject.GetComponentInChildren<RTActiveLibDropDown>();
            _prefabScrollView = gameObject.GetComponentInChildren<RTPrefabScrollView>();
            _hoveredPrefabNameLabel = gameObject.GetComponentInChildren<RTHoveredPrefabNameLabel>();
            _prefabSearch = gameObject.GetComponentInChildren<RTPrefabSearch>();

            _prefabScrollView.PrefabPreviewHoverEnter += OnPrefabPreviewHoverEnter;
            _prefabScrollView.PrefabPreviewHoverExit += OnPrefabPreviewHoverExit;
            PrefabSearch.SearchField.onValueChanged.AddListener((p) => { OnPrefabSearchFieldValueChanged(p); });
        }

        private void OnPrefabPreviewHoverEnter(RTPrefab prefab)
        {
            HoveredPrefabNameLabel.PrefabName = prefab.UnityPrefab != null ? prefab.UnityPrefab.name : string.Empty;
        }

        private void OnPrefabPreviewHoverExit(RTPrefab prefab)
        {
            HoveredPrefabNameLabel.PrefabName = string.Empty;
        }

        private void OnPrefabSearchFieldValueChanged(string value)
        {
            PrefabScrollView.FilterPrefabsByName(value);
        }
    }
}
