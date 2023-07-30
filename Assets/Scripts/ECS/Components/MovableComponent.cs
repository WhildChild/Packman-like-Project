using UnityEngine.AI;

namespace Client {
    struct MovableComponent
    {
        public float Speed;
        public bool IsMoving;

        public NavMeshAgent _navMeshAgent;
    }
}