
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;


sealed class MoveSystem : IEcsInitSystem, IEcsRunSystem {

    private EcsPool<MovableComponent> _movablePool;
    private EcsPool<MoveEventComponent> _movableEventPool;
    private EcsFilter _filter;

     public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        _filter = world.Filter<MovableComponent>().Inc<MoveEventComponent>().End();
        _movableEventPool = world.GetPool<MoveEventComponent>();
        _movablePool = world.GetPool<MovableComponent>();
    }

    public void Run (IEcsSystems systems) 
    {     
        foreach (var entity in _filter)
        {
            ref var movableComponent = ref _movablePool.Get(entity);
            ref var moveEventComponent = ref _movableEventPool.Get(entity);

            var direction = moveEventComponent.direction;
            movableComponent.NavMeshAgent.Move(direction * movableComponent.Speed * Time.deltaTime);

            _movableEventPool.Del(entity);
        }
    }
}
