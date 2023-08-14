using Leopotam.EcsLite;

sealed class CellStatusChangeSystem : IEcsInitSystem, IEcsRunSystem 
{
    private EcsFilter _cellStatusChangeEventFilter = default;
    private EcsFilter _peasePoolObjectsFilter = default;

    private EcsPool<CellStatusChangeEventComponent> _cellStatusChangeEventPool = default;
    private EcsPool<PeaseRemoveEventComponent> _peaseRemoveEventPool = default;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _cellStatusChangeEventFilter = world.Filter<CellStatusChangeEventComponent>().End();
        _cellStatusChangeEventPool = world.GetPool<CellStatusChangeEventComponent>();

        _peasePoolObjectsFilter = world.Filter<PeasePoolObjectsComponent>().End();
        _peaseRemoveEventPool = world.GetPool<PeaseRemoveEventComponent>();
    }

    public void Run (IEcsSystems systems)
    {
        foreach (var cellStatusChangeEventEntity in _cellStatusChangeEventFilter) 
        {
            ref var cellStatusChangeEventComponent = ref _cellStatusChangeEventPool.Get(cellStatusChangeEventEntity);

            foreach (var eventInfo in  cellStatusChangeEventComponent.StatusChangeInfoList) 
            {
                if (eventInfo.OldStatus == CellStatus.HavePease &&
                eventInfo.NewStatus == CellStatus.HavePlayer)
                {
                    foreach (var peasePoolObjectsEntity in _peasePoolObjectsFilter)
                    {
                        ref var peaceRemoveEventComponent = ref _peaseRemoveEventPool.Add(peasePoolObjectsEntity);
                        peaceRemoveEventComponent.Cell = eventInfo.Cell;
                    }
                }                
                eventInfo.Cell.CellStatus = eventInfo.NewStatus;
            }
            _cellStatusChangeEventPool.Del(cellStatusChangeEventEntity);
        }
    }
}
