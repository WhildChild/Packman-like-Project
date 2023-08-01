using UnityEngine;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine.AI;

sealed class PlayerSpawnSystem : IEcsInitSystem {

    private readonly EcsCustomInject<PlayerSettings> _playerSettings = default;
     
    public void Init (IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var playerEntity = world.NewEntity();
      
        ref var playerComponent = ref world.GetPool<PlayerComponent>().Add(playerEntity);
        ref var playerInputComponent = ref world.GetPool<PlayerInputComponent>().Add(playerEntity);
        ref var movableComponent = ref world.GetPool<MovableComponent>().Add(playerEntity);

        playerComponent.gameObject = GameObject.Instantiate(_playerSettings.Value.Prefab, _playerSettings.Value.SpawnPosition, Quaternion.identity);
        movableComponent.NavMeshAgent = playerComponent.gameObject.GetComponent<NavMeshAgent>();
        movableComponent.Speed = _playerSettings.Value.MoveSpeed;
    }
}
