
using Leopotam.EcsLite;

public class PeaseRemoveSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter _peaseRemoveEventFilter = default;

    private EcsPool<PeaseRemoveEventComponent> _peaseRemoveEventComponentPool = default;
    private EcsPool<PeasePoolObjectsComponent> _peasePoolObjcetsPool = default;
    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _peaseRemoveEventFilter = world.Filter<PeasePoolObjectsComponent>().Inc<PeaseRemoveEventComponent>().End();
        _peasePoolObjcetsPool = world.GetPool<PeasePoolObjectsComponent>();
        _peaseRemoveEventComponentPool = world.GetPool<PeaseRemoveEventComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in _peaseRemoveEventFilter) 
        {
            ref var peasePoolObjectsComponent = ref _peasePoolObjcetsPool.Get(entity);
            ref var peaseRemoveEventComponent = ref _peaseRemoveEventComponentPool.Get(entity);

            peasePoolObjectsComponent.Return(peaseRemoveEventComponent.Cell.Pease);
            peaseRemoveEventComponent.Cell.Pease = null;

            _peaseRemoveEventComponentPool.Del(entity);
        }
    }
}
    

