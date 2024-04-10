using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Cell
{
    public HashSet<GameObject> Objects { get; } = new HashSet<GameObject>();
}

public class Grid_controller : MonoBehaviour
{
    UnityEngine.Grid _grid;
    Dictionary<Vector3Int, Cell> _cells = new Dictionary<Vector3Int, Cell>();

    private void Awake()
    {
        _grid = gameObject.GetOrAddComponent<UnityEngine.Grid>();
    }

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
