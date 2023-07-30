using Leopotam.EcsLite;

namespace Client {
    sealed class MovableSystem : IEcsInitSystem, IEcsRunSystem {

        private  EcsPool<MovableComponent> _pool;
        private  EcsFilter _filter;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<MovableComponent>().End();

            _pool = world.GetPool<MovableComponent>();
        }

        public void Run (IEcsSystems systems) 
        {     
            foreach (var entity in _filter)
            {
                ref MovableComponent movableComponent = ref _pool.Get(entity);

                //movableComponent._navMeshAgent.Move()
            }
        }
    }
}