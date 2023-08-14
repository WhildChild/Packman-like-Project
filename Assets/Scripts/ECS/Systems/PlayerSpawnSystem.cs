using UnityEngine;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine.AI;
using System.Collections.Generic;

sealed class PlayerSpawnSystem : IEcsInitSystem {

    private readonly EcsCustomInject<PlayerSettings> _playerSettings = default;
    private readonly EcsCustomInject<MazeSettings> _mazeSettings = default;

    public void Init (IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var playerEntity = world.NewEntity();
        var mazeFilter = world.Filter<MazeComponent>().End();
        

        ref var playerComponent = ref world.GetPool<PlayerComponent>().Add(playerEntity);
        ref var playerInputComponent = ref world.GetPool<PlayerInputEventComponent>().Add(playerEntity);
        ref var movableComponent = ref world.GetPool<MovableComponent>().Add(playerEntity);
        

        foreach (var mazeEntity in mazeFilter)
        {
            ref var mazeComponent = ref world.GetPool<MazeComponent>().Get(mazeEntity);
            var spawnPosition = CalculateSpawnPosition(mazeComponent);
            playerComponent.gameObject = GameObject.Instantiate(_playerSettings.Value.Prefab, spawnPosition, Quaternion.identity);
            movableComponent.cellCoordinates = CalculateCellCoordinate();
            movableComponent.Transform = playerComponent.gameObject.transform;

            ref var cellStatusChangeComponent = ref world.GetPool<CellStatusChangeEventComponent>().Add(playerEntity);
            cellStatusChangeComponent.StatusChangeInfoList = new List<CellStatusChangeInfo>()
            { 
                new CellStatusChangeInfo() 
                { 
                    OldStatus = CellStatus.Empty,
                    NewStatus = CellStatus.HavePlayer,
                    Cell = mazeComponent.cells[movableComponent.cellCoordinates.x, movableComponent.cellCoordinates.y]
                }
            };
        }
    }

    private Vector3 CalculateSpawnPosition(MazeComponent mazeComponent)
    {
        float x = 0,  z = 0;
        float y = _mazeSettings.Value.CellSize;
        //Calculate x
        if (_playerSettings.Value.SpawnPosition.x >= _mazeSettings.Value.SideSize)
        {
            x = mazeComponent.cells[_mazeSettings.Value.SideSize - 1, 0].x + _mazeSettings.Value.CellSize;
        }
        else
        {
            x = mazeComponent.cells[_playerSettings.Value.SpawnPosition.x, 0].x + _mazeSettings.Value.CellSize;
        }

        //Calculate z
        if (_playerSettings.Value.SpawnPosition.y >= _mazeSettings.Value.SideSize)
        {
            z = mazeComponent.cells[0, _mazeSettings.Value.SideSize - 1].y + _mazeSettings.Value.CellSize;
        }
        else
        {
            z = mazeComponent.cells[0, _playerSettings.Value.SpawnPosition.y].y + _mazeSettings.Value.CellSize;
        }

        return new Vector3(x, y, z);
    }

    private Vector2Int CalculateCellCoordinate()
    {
        int x , y ;

        if (_playerSettings.Value.SpawnPosition.x > _mazeSettings.Value.SideSize -1)
            x = _mazeSettings.Value.SideSize - 1;
        else
            x = _playerSettings.Value.SpawnPosition.x;

        if (_playerSettings.Value.SpawnPosition.y > _mazeSettings.Value.SideSize -1)
            y = _mazeSettings.Value.SideSize -1;
        else
            y = _playerSettings.Value.SpawnPosition.y;

        return new Vector2Int(x, y);
    }
}
