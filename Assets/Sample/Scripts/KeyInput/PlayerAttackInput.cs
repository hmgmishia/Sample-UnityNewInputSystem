using System;
using UnityEngine;

namespace Sample.KeyInput
{
    public class PlayerAttackInput : MonoBehaviour, IAttackInputMediator
    {
        private SampleInputActionAsset _actionAsset;

        public event Action OnAttack;

        private void Awake()
        {
            _actionAsset = new SampleInputActionAsset();
            _actionAsset.PCActions.Shoot.Enable();
        }

        private void Update()
        {
            if (_actionAsset.PCActions.Shoot.WasPressedThisFrame())
            {
                OnAttack?.Invoke();
            }
        }
    }
}