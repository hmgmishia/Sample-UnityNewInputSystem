using UnityEngine;

namespace Sample.KeyInput
{
    public class PlayerMoveInput : MonoBehaviour, IPlayerMoveInputMediator
    {
        private SampleInputActionAsset _actionAsset;

        private void Awake()
        {
            _actionAsset = new SampleInputActionAsset();
            _actionAsset.PCActions.Movement.Enable();
        }

        public Vector2 GetAxis()
        {
            return _actionAsset.PCActions.Movement.ReadValue<Vector2>();
        }
    }
}