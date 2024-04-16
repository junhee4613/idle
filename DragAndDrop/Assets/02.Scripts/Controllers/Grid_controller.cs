using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class Cell
{
    public HashSet<GameObject> Objects { get; } = new HashSet<GameObject>();
}

public class Grid_controller
{
    public UnityEngine.Grid _grid;
    public Dictionary<Vector3Int, Cell> _cells = new Dictionary<Vector3Int, Cell>();
    public Dictionary<GameObject, Transform> hit_obstacle = new Dictionary<GameObject, Transform>();

    public void Add(GameObject go)
    {
        Vector3Int cellPos = _grid.WorldToCell(go.transform.position);

        Cell cell = GetCell(cellPos);
        if (cell == null)
            return;

        cell.Objects.Add(go);
    }

    public void Remove(GameObject go)
    {
        Vector3Int cellPos = _grid.WorldToCell(go.transform.position);

        Cell cell = GetCell(cellPos);
        if (cell == null)
            return;

        cell.Objects.Remove(go);
    }

    Cell GetCell(Vector3Int cellPos)
    {
        Cell cell = null;

        if (_cells.TryGetValue(cellPos, out cell) == false)
        {
            cell = new Cell();
            _cells.Add(cellPos, cell);
        }

        return cell;
    }
    
    IEnumerator Hit_box(GameObject obs, float range, float loop_time, float time)       //범위 (컴포넌트 상에선 2, 유니티 기본 그리드 상에선 0.2) 증가 
    {
        while (time > loop_time)
        {
            Vector3Int player_pos = _grid.WorldToCell(Managers.GameManager.Player_character.transform.position);
            if (!hit_obstacle.ContainsKey(obs))
            {
                hit_obstacle.Add(obs, obs.transform);
            }
            Vector3Int left = _grid.WorldToCell(hit_obstacle[obs].position + new Vector3(-range, 0));
            Vector3Int right = _grid.WorldToCell(hit_obstacle[obs].position + new Vector3(+range, 0));
            Vector3Int bottom = _grid.WorldToCell(hit_obstacle[obs].position + new Vector3(0, -range, 0));
            Vector3Int top = _grid.WorldToCell(hit_obstacle[obs].position + new Vector3(0, +range, 0));

            int minX = left.x;
            int maxX = right.x;
            int minY = bottom.y;
            int maxY = top.y;

            if (minX <= player_pos.x && maxX >= player_pos.x && minY <= player_pos.y && maxY >= player_pos.y)
            {
                if (!Managers.GameManager.Player.hit_statu)
                {
                    Managers.GameManager.Player.Hit();
                }
            }
            yield return null;
        }
        
    }
    public List<GameObject> GatherObjects(Vector3 pos, float range)
    {
        List<GameObject> objects = new List<GameObject>();

        Vector3Int left = _grid.WorldToCell(pos + new Vector3(-range, 0));
        Vector3Int right = _grid.WorldToCell(pos + new Vector3(+range, 0));
        Vector3Int bottom = _grid.WorldToCell(pos + new Vector3(0, 0, -range));
        Vector3Int top = _grid.WorldToCell(pos + new Vector3(0, 0, +range));

        int minX = left.x;
        int maxX = right.x;
        int minZ = bottom.z;
        int maxZ = top.z;

        for (int x = minX; x <= maxX; x++)
        {
            for (int z = minZ; z <= maxZ; z++)
            {
                if (_cells.ContainsKey(new Vector3Int(x, 0, z)) == false)
                    continue;

                objects.AddRange(_cells[new Vector3Int(x, 0, z)].Objects);

            }
        }
        return objects;
    }
}
