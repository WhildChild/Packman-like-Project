using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System;
using System.Collections.Generic;
using UnityEngine;


public class PeaseSpawnTimerSystem : IEcsInitSystem, IEcsRunSystem
{
    private readonly EcsCustomInject<PeaseSettings> _peaseSettings = default;
    private EcsFilter _spawnerEntityFilter = default;

    private EcsPool<PeasePoolObjectsComponent> _peasePoolObjectsPool = default;
    private EcsPool<PeaseSpawnEventComponent> _peaceSpawnEventPool = default;

    private float _time = 0f;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        var spawnerEntity = world.NewEntity();
        _peasePoolObjectsPool = world.GetPool<PeasePoolObjectsComponent>();
        _peaceSpawnEventPool = world.GetPool<PeaseSpawnEventComponent>();

        ref var peaseSpawnerComponent = ref _peasePoolObjectsPool.Add(spawnerEntity);
        peaseSpawnerComponent.PeasePool = new List<GameObject>();
        peaseSpawnerComponent.PeasePrefab = _peaseSettings.Value.Prefab;
        peaseSpawnerComponent.PeasePoolGO = new GameObject("Pease Pool");

        ref var peaseSpawnEventComponent = ref _peaceSpawnEventPool.Add(spawnerEntity);

        peaseSpawnEventComponent.spawnCount = _peaseSettings.Value.StartPeaseCount;
        _time = _peaseSettings.Value.SpawnRate;

        _spawnerEntityFilter = world.Filter<PeasePoolObjectsComponent>().End();
    }

    public void Run(IEcsSystems systems)
    {
        if (!_peaseSettings.Value.NeedSpawnPerTime) return;
        _time -= Time.deltaTime;

        foreach (var spawnerEntity in _spawnerEntityFilter)
        {
            if (_time < 0f)
            {
                ref var spawnerEventComponent =  ref _peaceSpawnEventPool.Add(spawnerEntity);
                spawnerEventComponent.spawnCount = _peaseSettings.Value.PeaseSpawnCountPerTick;
                _time = _peaseSettings.Value.SpawnRate;
            }
        } 
    }

}
