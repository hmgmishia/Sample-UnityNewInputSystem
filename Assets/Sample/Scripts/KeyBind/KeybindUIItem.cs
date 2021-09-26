using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public class KeybindUIItem : MonoBehaviour
    {
        [SerializeField]
        private InputActionReference _reference;

        [SerializeField]
        private KeybindItemButton[] _buttons;

        public event Action OnStartKeybind;
        public event Action OnCompletedKeybind;
        public event Action OnCanceledKeybind;

        private int _bindingCount;

        private InputActionRebindingExtensions.RebindingOperation _rebindingOperation;

        private void Start()
        {
            Assert.IsTrue(_buttons.Length > 0, "一番目のキー設定のボタンがありません");

            BindingCountFix();
            var index = 0;
            foreach (var buttons in _buttons)
            {
                if (!buttons.gameObject.activeSelf)
                {
                    continue;
                }

                //compositeだけループ
                while (index < _reference.action.bindings.Count && _reference.action.bindings[index].isComposite)
                {
                    index += 1;
                }

                var innerIndex = index;

                buttons.BindButton.onClick.AddListener(() =>
                {
                    OnStartKeybind?.Invoke();
                    _rebindingOperation = _reference.action.PerformInteractiveRebinding()
                        .WithTargetBinding(innerIndex)
                        .WithCancelingThrough("<Keyboard>/escape")
                        .OnCancel(operation => { OnCanceledKeybind?.Invoke(); })
                        .OnComplete(operation =>
                        {
                            var inputAction = _reference.action;
                            var binding = inputAction.bindings;
                            //指定なしの個数を取得
                            var count = binding.Count(x => (!x.hasOverrides | x.overridePath is "") & x.path is "" || !(x.path is "") & x.overridePath is "");
                            //もしキーバインドされている数 != バインドされているキーの種類数であれば 同じキー割り当てなので無効化
                            if (inputAction.controls.Count != _bindingCount - count)
                            {
                                inputAction.ApplyBindingOverride(innerIndex, "");
                            }

                            Rebound();
                            _rebindingOperation?.Dispose();
                            _rebindingOperation = null;
                            OnCompletedKeybind?.Invoke();
                        })
                        .Start();
                });
                buttons.DeleteButton.onClick.AddListener(() =>
                {
                    _reference.action.ApplyBindingOverride(innerIndex, "");
                    Rebound();
                });
                index += 1;
            }

            Rebound();
        }

        private void BindingCountFix()
        {
            var binding = _reference.action.bindings;
            var buttonCount = _buttons.Length;
            _bindingCount = binding.Count(x => !x.isComposite);

            var sub = buttonCount - _bindingCount;
            var abs = Mathf.Abs(sub);
            for (var i = 0; i < abs; ++i)
            {
                _buttons[buttonCount - abs + i].gameObject.SetActive(false);
            }
        }

        public void Rebound()
        {
            var index = 0;
            var binding = _reference.action.bindings;
            foreach (var buttons in _buttons)
            {
                while (index < binding.Count && binding[index].isComposite)
                {
                    index += 1;
                }

                if (index >= binding.Count)
                {
                    buttons.gameObject.SetActive(false);
                    continue;
                }

                buttons.Rebound(binding[index]);
                index += 1;
            }
        }
    }
}