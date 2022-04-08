using UnityEngine;
namespace Asteroids
{
    public sealed class Player : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _acceleration;
        [SerializeField] private float _hp;
        [SerializeField] private Rigidbody2D _bullet;
        [SerializeField] public Rigidbody2D _rigidbodyPlayer;
        [SerializeField] private Transform _barrel;
        [SerializeField] private float _force;

        private Camera _camera;
        private IMove _moveTransform;
        private IRotation _rotation;
       
        private void Start()
        {
            _camera = Camera.main;
            _moveTransform = new AccelerationMove(transform, _speed, _acceleration);
            _rotation = new RotationShip(transform);
           
        }
        private void Update()
        {
            var direction = UnityEngine.Input.mousePosition - _camera.WorldToScreenPoint(transform.position);
            _rotation.Rotation(direction);

            _moveTransform.Move(UnityEngine.Input.GetAxis("Horizontal"),
                UnityEngine.Input.GetAxis("Vertical"), Time.deltaTime);
            Input();
        }

        private void Input()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (_moveTransform is AccelerationMove accelerationMove)
                {
                    accelerationMove.AddAcceleration();
                }

            }
            if (UnityEngine.Input.GetKeyUp(KeyCode.LeftShift))
            {
                if (_moveTransform is AccelerationMove accelerationMove)
                {
                    accelerationMove.RemoveAcceleration();
                }
            }
            if (UnityEngine.Input.GetButtonDown("Fire1"))
            {
                var temAmmunition = Instantiate(_bullet, _barrel.position,
                _barrel.rotation);
                temAmmunition.AddForce(_barrel.up * _force);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (_hp <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                _hp--;
            }
        }
        public void Move(float x, float y, float z)
        {
            if (_rigidbodyPlayer)
            {
                _rigidbodyPlayer.AddForce(new Vector3(x, y, z) * _speed);
            }
            else
            {
                Debug.Log("NO Rigidbody");
            }
        }
    }
}

