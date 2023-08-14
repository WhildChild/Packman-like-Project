using Leopotam.EcsLite;
using UnityEngine;

sealed class PlayerInputSystem : IEcsRunSystem, IEcsInitSystem 
{
    private PlayerInput _inputActions = default;

    private EcsFilter _playerFilter = default;

    private EcsPool<PlayerInputEventComponent> _inputEventPool = default;


    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _playerFilter = world.Filter<PlayerComponent>().End();
        _inputEventPool = world.GetPool<PlayerInputEventComponent>();
        _inputActions = new PlayerInput();
        _inputActions.Enable();
    }

    public void Run (IEcsSystems systems)
    {
        foreach (var entity in _playerFilter)
        {
            if (_inputActions.Player.Move.WasPressedThisFrame())
            {
                var direction = _inputActions.Player.Move.ReadValue<Vector2>();
                ref var inputEventComponent = ref _inputEventPool.Add(entity);
                inputEventComponent.InputEventType = InputEventType.Movable;
                inputEventComponent.direction = direction;
            }
        }
    }
 
}
