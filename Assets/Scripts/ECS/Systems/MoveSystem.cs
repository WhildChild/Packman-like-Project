using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

sealed class MoveSystem : IEcsInitSystem, IEcsRunSystem {

    private readonly EcsCustomInject<MazeSettings> _mazeSettings = default;

    private EcsPool<MovableComponent> _movablePool = default;
    private EcsPool<MoveEventComponent> _movableEventPool = default;
    private EcsPool<MazeComponent> _mazePool = default;
    private EcsPool<CellStatusChangeEventComponent> _cellStatusChangeEventPool = default;

    private EcsFilter _movableEntitiesFilter = default;
    private EcsFilter _mazeEntityFilter = default;
    

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        _movableEntitiesFilter = world.Filter<MovableComponent>().Inc<MoveEventComponent>().End();
        _mazeEntityFilter = world.Filter<MazeComponent>().End();
        _movableEventPool = world.GetPool<MoveEventComponent>();
        _movablePool = world.GetPool<MovableComponent>();
        _mazePool = world.GetPool<MazeComponent>();
        _cellStatusChangeEventPool = world.GetPool<CellStatusChangeEventComponent>();
    }

    public void Run (IEcsSystems systems) 
    {
        foreach (var mazeEntity in _mazeEntityFilter)
        {
            foreach (var movableEntity in _movableEntitiesFilter)
            {
                ref var movableComponent = ref _movablePool.Get(movableEntity);
                ref var moveEventComponent = ref _movableEventPool.Get(movableEntity);
                ref var mazeComponent = ref _mazePool.Get(mazeEntity);

                if (moveEventComponent.Direction.x == 1) 
                {
                    TryMoveRight(ref mazeComponent, ref movableComponent, movableEntity);
                }
                if (moveEventComponent.Direction.x == -1)
                {
                    TryMoveLeft(ref mazeComponent, ref movableComponent, movableEntity);
                }

                if (moveEventComponent.Direction.y == 1)
                {
                    TryMoveUp(ref mazeComponent, ref movableComponent, movableEntity);
                }
                if (moveEventComponent.Direction.y == -1)
                {
                    TryMoveDown(ref mazeComponent, ref movableComponent, movableEntity);
                }

                _movableEventPool.Del(movableEntity);
            }
        }
    }

    #region MoveMethods
    private void TryMoveUp(ref MazeComponent mazeComponent, ref MovableComponent movableComponent, int movableEntity)
    {
       ref  var currentCell = ref mazeComponent.cells[movableComponent.cellCoordinates.x, movableComponent.cellCoordinates.y];
        
        if (currentCell.TopWall != null) return;

       ref var nextCell = ref mazeComponent.cells[movableComponent.cellCoordinates.x, movableComponent.cellCoordinates.y + 1];

        if (nextCell.BottomWall != null) return; 

        movableComponent.Transform.localPosition = new Vector3(nextCell.GameObject.transform.localPosition.x, _mazeSettings.Value.CellSize, nextCell.GameObject.transform.localPosition.z);
        movableComponent.cellCoordinates.y++;

        SendChangeStatusEvent(movableEntity, currentCell, nextCell);
    }
    private void TryMoveDown(ref MazeComponent mazeComponent, ref MovableComponent movableComponent, int movableEntity)
    {
        ref var currentCell = ref mazeComponent.cells[movableComponent.cellCoordinates.x , movableComponent.cellCoordinates.y];
        
        if (currentCell.BottomWall != null) return;

        ref var nextCell = ref mazeComponent.cells[movableComponent.cellCoordinates.x , movableComponent.cellCoordinates.y - 1];

        if (nextCell.TopWall != null) return;

        movableComponent.Transform.localPosition = new Vector3(nextCell.GameObject.transform.localPosition.x, _mazeSettings.Value.CellSize, nextCell.GameObject.transform.localPosition.z);
        movableComponent.cellCoordinates.y--;
        SendChangeStatusEvent(movableEntity, currentCell, nextCell);
    }
    private void TryMoveLeft(ref MazeComponent mazeComponent, ref MovableComponent movableComponent, int movableEntity)
    {
        ref var currentCell = ref mazeComponent.cells[movableComponent.cellCoordinates.x, movableComponent.cellCoordinates.y];

        if (currentCell.LeftWall != null) return;

        ref var nextCell = ref mazeComponent.cells[movableComponent.cellCoordinates.x-1, movableComponent.cellCoordinates.y];

        if (nextCell.RightWall != null) return;

        movableComponent.Transform.localPosition = new Vector3(nextCell.GameObject.transform.localPosition.x, _mazeSettings.Value.CellSize, nextCell.GameObject.transform.localPosition.z);
        movableComponent.cellCoordinates.x--;
        SendChangeStatusEvent(movableEntity, currentCell, nextCell);
    }
    private void TryMoveRight(ref MazeComponent mazeComponent, ref MovableComponent movableComponent, int movableEntity)
    {
        ref var currentCell = ref mazeComponent.cells[movableComponent.cellCoordinates.x, movableComponent.cellCoordinates.y];

        if (currentCell.RightWall != null)  return; 

        ref var nextCell = ref mazeComponent.cells[movableComponent.cellCoordinates.x + 1, movableComponent.cellCoordinates.y];

        if (nextCell.LeftWall != null)  return; 

        movableComponent.Transform.localPosition = new Vector3(nextCell.GameObject.transform.localPosition.x, _mazeSettings.Value.CellSize, nextCell.GameObject.transform.localPosition.z);
        movableComponent.cellCoordinates.x++;
        SendChangeStatusEvent(movableEntity, currentCell, nextCell);
    }
    #endregion

    private void SendChangeStatusEvent(int movableEntity, Cell currentCell, Cell nextCell)
    {
        ref var cellStatusChangeComponent = ref _cellStatusChangeEventPool.Add(movableEntity);
        var currentCellInfo = new CellStatusChangeInfo()
        {
            OldStatus = CellStatus.HavePlayer,
            NewStatus = CellStatus.Empty,
            Cell = currentCell
        };
        var nextCellInfo = new CellStatusChangeInfo()
        {
            OldStatus = nextCell.CellStatus,
            NewStatus = CellStatus.HavePlayer,
            Cell = nextCell
        };

        cellStatusChangeComponent.StatusChangeInfoList = new System.Collections.Generic.List<CellStatusChangeInfo> { currentCellInfo, nextCellInfo };
    }
}
