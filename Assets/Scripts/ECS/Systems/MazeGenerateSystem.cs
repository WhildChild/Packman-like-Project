using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEditor.AI;
using System.Collections.Generic;
using System.Linq;

sealed class MazeGenerateSystem : IEcsInitSystem 
{

    private readonly EcsCustomInject<MazeSettings> _mazeSettings = default;
    public void Init (IEcsSystems systems) 
    {
        var gameBoard = new GameObject("GameBoard");

        Cell[,] cells = new Cell[_mazeSettings.Value.Width, _mazeSettings.Value.Height];
        List<GameObject> walls = new List<GameObject>();
        //—павн €чеек
        for (int x = 0; x < cells.GetLength(0); x++)
        {
            for (int y = 0; y < cells.GetLength(1); y++)
            {
                var cell = SpawnCell(x,y, gameBoard.transform);
                cell.LeftWall = SpawnLeftWall(cell.GameObject.transform);
                cell.RightWall = SpawnRightWall(cell.GameObject.transform);
                cell.TopWall = SpawnTopWall(cell.GameObject.transform);
                cell.BottomWall = SpawnBottomWall(cell.GameObject.transform);

                if (x != 0) walls.Add(cell.LeftWall);
                if (x != _mazeSettings.Value.Width-1) walls.Add(cell.RightWall);
                if (y != 0) walls.Add(cell.BottomWall);
                if (y != _mazeSettings.Value.Height-1) walls.Add(cell.TopWall);

                cells[x,y] = cell; 
            }
        }

        RemoveWallsWithBacktracker(cells);

        RemoveRandomWalls(walls.Where(x => x.activeSelf).ToList());
        DestroyInactiveWalls(walls.Where(x=> !x.activeSelf).ToList());
        
        NavMeshBuilder.BuildNavMesh();
    }

    private void RemoveWallsWithBacktracker(Cell[,] cells)
    {
        Cell current = cells[0,0];
        current.IsVisited = true;

        Stack<Cell> stack = new Stack<Cell>();
        do
        {
            List<Cell> unvisitedNeighbours = new List<Cell>();
            int x = current.x;
            int y = current.y;
         

            if (x > 0 && !cells[x - 1, y].IsVisited)
            {
                unvisitedNeighbours.Add(cells[x-1, y]);
            }
            if (y > 0 && !cells[x , y-1].IsVisited)
            {
                unvisitedNeighbours.Add(cells[x, y-1]);
            }
            if (x < _mazeSettings.Value.Width - 1 && !cells[x + 1, y].IsVisited)
            {
                unvisitedNeighbours.Add(cells[x + 1, y]);
            }
            if (y < _mazeSettings.Value.Height - 1 && !cells[x , y + 1].IsVisited)
            {
                unvisitedNeighbours.Add(cells[x , y + 1]);
            }

            if (unvisitedNeighbours.Count > 0)
            {
                Cell chosen = unvisitedNeighbours[Random.Range(0, unvisitedNeighbours.Count)];
                RemoveWallsBetweenCells(current, chosen);
                chosen.IsVisited = true;
                current = chosen;
                stack.Push(chosen);
            }
            else
            {
                current = stack.Pop();
            }

        } while (stack.Count > 0);
    }

    private void RemoveRandomWalls(List<GameObject> walls)   
    {
        while (walls.Count > _mazeSettings.Value.WallsCount)        
        {
            var randomIndex = Random.Range(0, walls.Count);
            walls[randomIndex].SetActive(false);
            walls.RemoveAt(randomIndex);
        }
    }
    
    private void DestroyInactiveWalls(List<GameObject> walls)
    {
        foreach (var wall in walls)
        {
            GameObject.Destroy(wall);
        }
    }

    #region Private Help Methods
    
    private Cell SpawnCell(int x, int y, Transform parent)
    {
        Cell cell = new Cell() { x = x, y = y };
        cell.GameObject = new GameObject("Cell"); 
        cell.GameObject.transform.parent = parent;
        GameObject.Instantiate(_mazeSettings.Value.GroundPrefab, cell.GameObject.transform);

        cell.GameObject.transform.localPosition = new Vector3(x + _mazeSettings.Value.CellSize, 0, y + _mazeSettings.Value.CellSize);
        return cell;
    }
    private GameObject SpawnWall(float x, float y, Transform parent, bool isHorizontal)
    {
        var wall = GameObject.Instantiate(_mazeSettings.Value.WallPrefab, parent.transform);
        wall.transform.localPosition = new Vector3(x,_mazeSettings.Value.CellSize, y);
        if (isHorizontal)
        {
            wall.transform.localEulerAngles = new Vector3(0, 90, 0);
        }
        return wall;
    }

    private GameObject SpawnLeftWall(Transform parent)
    {
        return SpawnWall(-_mazeSettings.Value.CellSize,0, parent, false);
    }
    private GameObject SpawnRightWall(Transform parent)
    {
        return SpawnWall(_mazeSettings.Value.CellSize, 0, parent, false);
    }
    private GameObject SpawnTopWall(Transform parent)
    {
        return SpawnWall(0, _mazeSettings.Value.CellSize, parent, true);
    }
    private GameObject SpawnBottomWall(Transform parent)
    {
        return SpawnWall(0, -_mazeSettings.Value.CellSize, parent, true);
    }

    private void RemoveWallsBetweenCells(Cell current, Cell chosen)
    {
        if (current.x == chosen.x)
        {
            if (current.y > chosen.y)
            {
                current.BottomWall.SetActive(false);
                chosen.TopWall.SetActive(false);
            }
            else
            {
                chosen.BottomWall.SetActive(false);
                current.TopWall.SetActive(false);
            }
        }
        else
        {
            if (current.x > chosen.x)
            {
                current.LeftWall.SetActive(false);
                chosen.RightWall.SetActive(false);
            }
            else
            {
                chosen.LeftWall.SetActive(false);
                current.RightWall.SetActive(false);
            }
        }
    }
    #endregion
}
