using UnityEngine;
using UnityEngine.AI;

namespace BSDesigner.Unity.Runtime
{
    /// <summary>
    /// Movement component that uses a nav mesh agent
    /// </summary>
    public class NavMeshMovement : MonoBehaviour, IMovement
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;

        [SerializeField] private float _baseSpeed = 1f;

        [SerializeField] private float _distanceThreshold = 0.1f;

        public Vector3 Target
        {
            get => _navMeshAgent.destination;
            set
            {
                _navMeshAgent.isStopped = false;
                _navMeshAgent.SetDestination(value);
            }
        }

        public bool IsMovementEnabled
        {
            get => _navMeshAgent.isStopped;
            set => _navMeshAgent.isStopped = value;
        }

        public Transform Transform { get; }
        public float Speed
        {
            get => _speed;
            set
            {
                _speed = value;
                _navMeshAgent.speed = _speed * _baseSpeed;
            }
        }

        private float _speed = 1f;

        public bool HasArrivedOnTarget => Vector3.Distance(transform.position, _navMeshAgent.destination) < _distanceThreshold;

        void Start()
        {
            _navMeshAgent.speed = _baseSpeed * Speed;
        }

    }
}