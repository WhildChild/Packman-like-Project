using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System.Linq;
using UnityEngine;

public sealed class PeaseSpawnSystem : IEcsInitSystem, IEcsRunSystem
{
    private readonly EcsCustomInject<MazeSettings> _mazeSettings = default;

    private EcsFilter _spawnEventFilter = default;
    private EcsFilter _mazeFilter = default;

    private EcsPool<MazeComponent> _mazePool = default;
    private EcsPool<PeaseSpawnEventComponent> _peaseSpawnEventPool = default;
    private EcsPool<PeasePoolObjectsComponent> _peasePoolObjectsPool = default;
    private EcsPool<CellStatusChangeEventComponent> _cellStatusChangeEventPool = default;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _spawnEventFilter = world.Filter<PeasePoolObjectsComponent>().Inc<PeaseSpawnEventComponent>().End();
        _mazeFilter = world.Filter<MazeComponent>().End();
        _mazePool = world.GetPool<MazeComponent>();
        _peaseSpawnEventPool = world.GetPool<PeaseSpawnEventComponent>();
        _peasePoolObjectsPool = world.GetPool<PeasePoolObjectsComponent>();
        _cellStatusChangeEventPool = world.GetPool<CellStatusChangeEventComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach(var spawnerEntity in _spawnEventFilter)
        {
            foreach (var mazeEntity in _mazeFilter)
            {
                ref var mazeComponent = ref _mazePool.Get(mazeEntity);
                ref var spawnEventComponent = ref _peaseSpawnEventPool.Get(spawnerEntity);
                ref var peasePoolComponent = ref _peasePoolObjectsPool.Get(spawnerEntity);

                var cellsList = mazeComponent.cells.ToList();

                if (spawnEventComponent.spawnCount > cellsList.Count-1)
                {
                    spawnEventComponent.spawnCount = cellsList.Count-1;
                }

                var randomCells = mazeComponent.cells.ToList().OrderBy(x => new System.Random().Next()).Where(x=>x.CellStatus == CellStatus.Empty).Take(spawnEventComponent.spawnCount).ToList();
                if (randomCells.Any())
                {
                    ref var cellStatusChangeEventComponent = ref _cellStatusChangeEventPool.Add(mazeEntity);
                    cellStatusChangeEventComponent.StatusChangeInfoList = new System.Collections.Generic.List<CellStatusChangeInfo>();
                    foreach (var cell in randomCells)
                    {
                        var pease = peasePoolComponent.Get();

                        pease.transform.position = new Vector3(cell.GameObject.transform.position.x, _mazeSettings.Value.CellSize, cell.GameObject.transform.position.z);

                        cell.Pease = pease;

                        cellStatusChangeEventComponent.StatusChangeInfoList.Add(new CellStatusChangeInfo()
                        {
                            OldStatus = CellStatus.Empty,
                            NewStatus = CellStatus.HavePease,
                            Cell = cell
                        });
                    }   
                }
                _peaseSpawnEventPool.Del(spawnerEntity);
            }
        }
    }
}
