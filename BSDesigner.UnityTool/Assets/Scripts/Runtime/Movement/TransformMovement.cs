using UnityEngine;

namespace BSDesigner.Unity.Runtime
{
    /// <summary>
    /// Movement component that uses the agent transform.
    /// </summary>
    public class TransformMovement : MonoBehaviour, IMovement
    {
        [SerializeField] private float _baseSpeed = 1f;

        [SerializeField] private float _distanceThreshold = 0.1f;

        public Vector3 Target { get; set; }

        public float Speed { get; set; } = 1f;
        public bool HasArrivedOnTarget => Vector3.Distance(transform.position, Target) < _distanceThreshold;
        public bool IsMovementEnabled { get; set; }

        public Transform Transform => this.transform;

        private void Update()
        {
            if (!HasArrivedOnTarget)
            {
                transform.position = Vector3.MoveTowards(transform.position, Target, _baseSpeed * Speed);
            }
        }
    }
}