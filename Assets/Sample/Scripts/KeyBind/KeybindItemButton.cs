using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class KeybindItemButton : MonoBehaviour
    {
        [SerializeField]
        private Button _bindButton;

        public Button BindButton => _bindButton;

        [SerializeField]
        private Button _deleteButton;

        public Button DeleteButton => _deleteButton;

        [SerializeField]
        private Text _keyNameText;

        public void Rebound(InputBinding? inputBinding)
        {
            if (!inputBinding.HasValue)
            {
                _keyNameText.text = "";
                return;
            }

            _keyNameText.text = InputControlPath.ToHumanReadableString(inputBinding.Value.effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        }
    }
}