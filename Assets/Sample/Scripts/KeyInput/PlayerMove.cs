using UnityEngine;

namespace Sample.KeyInput
{
    public class PlayerMove : MonoBehaviour
    {
        [SerializeField, Min(0)]
        private float speed = 1.0f;

        private IPlayerMoveInputMediator _moveInputMediator;

        private void Start()
        {
            _moveInputMediator = GetComponent<IPlayerMoveInputMediator>();
        }

        private void Update()
        {
            var axis = _moveInputMediator.GetAxis();
            transform.position += (transform.forward * axis.y + transform.right * axis.x).normalized * speed * Time.deltaTime;
        }
    }
}