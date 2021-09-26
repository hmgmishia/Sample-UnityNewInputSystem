using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class KeybindUI : MonoBehaviour
    {
        [SerializeField]
        private InputActionAsset _actionAsset;

        [SerializeField]
        private KeybindUIItem[] _keybindUIItems;

        [SerializeField]
        private GameObject _overlay;

        [SerializeField]
        private Button _applyButton;

        [SerializeField]
        private Button _cancelButton;

        private const string OverrideBindJsonKeyName = "BindKeyJson";

        private void Awake()
        {
        }

        private void Start()
        {
            Load(true);
            foreach (var keybindUIItem in _keybindUIItems)
            {
                keybindUIItem.OnStartKeybind += () => _overlay.SetActive(true);
                keybindUIItem.OnCompletedKeybind += () =>
                {
                    _overlay.SetActive(false);
                    _cancelButton.interactable = true;
                    _applyButton.interactable = true;
                };
                keybindUIItem.OnCanceledKeybind += () => _overlay.SetActive(false);
            }

            _applyButton.onClick.AddListener(() =>
            {
                PlayerPrefs.SetString(OverrideBindJsonKeyName, _actionAsset.SaveBindingOverridesAsJson());
                Load();
            });
            _cancelButton.onClick.AddListener(() => { Load(); });
            _overlay.SetActive(false);
        }

        private void Load(bool isInit = false)
        {
            var json = PlayerPrefs.GetString(OverrideBindJsonKeyName, "");
            _actionAsset.LoadBindingOverridesFromJson(json);
            _cancelButton.interactable = false;
            _applyButton.interactable = false;
            if (isInit)
            {
                return;
            }

            foreach (var keybindUIItem in _keybindUIItems)
            {
                keybindUIItem.Rebound();
            }
        }
    }
}