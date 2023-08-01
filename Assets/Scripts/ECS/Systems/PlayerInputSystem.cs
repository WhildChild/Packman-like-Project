using Leopotam.EcsLite;
using UnityEngine;

sealed class PlayerInputSystem : IEcsRunSystem, IEcsInitSystem 
{
    private EcsPool<MoveEventComponent> _moveEventPool;
    private EcsPool<PlayerInputComponent> _inputEventPool;
    private EcsFilter _filter;
    private PlayerInput _inputActions;
    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _filter = world.Filter<PlayerInputComponent>().Inc<PlayerComponent>().Inc<MovableComponent>().End();
        _moveEventPool = world.GetPool<MoveEventComponent>();
        _inputActions = new PlayerInput();
        _inputActions.Enable();
    }

    public void Run (IEcsSystems systems)
    {
        foreach (var entity in _filter)
        {
            var direction = Vector2ToVector3(_inputActions.Player.Move.ReadValue<Vector2>());
            ref var moveEventComponent = ref _moveEventPool.Add(entity);
            moveEventComponent.direction = direction;
        }
    }


    private Vector3 Vector2ToVector3(Vector2 inputValue)
    {
        return new Vector3(inputValue.x, 0, inputValue.y);
    }
}
