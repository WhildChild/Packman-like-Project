using Leopotam.EcsLite;

/// <summary>
/// —истема нужна только на случай, если кроме движени€ у нас будут другие действи€ в игре (с точки зрени€ ввода с клавиатуры)
/// ¬ этом случае нужно дополнить список InputEventType и реализовать дл€ него действи€.
/// ѕеренаправление действий ввода в другие системы.
/// </summary>
public sealed class InputEventsExecuteSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter _inputEventFilter = default;

    private EcsPool<PlayerInputEventComponent> _inputEventPool = default;
    private EcsPool<MoveEventComponent> _moveEventPool = default;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _inputEventFilter = world.Filter<PlayerInputEventComponent>().End();
        _inputEventPool = world.GetPool<PlayerInputEventComponent>();
        _moveEventPool = world.GetPool<MoveEventComponent>();
    }

    public void Run (IEcsSystems systems)
    {
        foreach (var entity in _inputEventFilter)
        {
            ref var inputEventComponent = ref _inputEventPool.Get(entity);
            
            switch (inputEventComponent.InputEventType) 
            {
                case InputEventType.Movable:
                    ref var moveEventComponent = ref _moveEventPool.Add(entity);
                    moveEventComponent.Direction = inputEventComponent.direction;
                    break;
            }

            _inputEventPool.Del(entity);
        }
    }
}
