using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System.Collections;

// tileMap坐标系为向右为正，向上为正
// tileMap row和col相反

// 数据坐标系向右为正，向下为正
//A B C 
//B
//C

public class Grid : MonoBehaviour
{
    public SwitchableTile wallTile;
    public PlaneTile planeTile;
    public Tilemap tileMap;
    public Plane objectPlane;

    private int col = 9;
    private int row = 9;

    private int[,] checkerboard;

    private List<Plane> planes = new List<Plane>();



    public void OnEnable()
    {
        UserInput._OnClick += _OnClick;
        UserInput._OnDrag += _OnDrag;
    }

    private void _OnClick()
    {
        Collider2D collider = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition), Layers.PlaneMask);
        if (collider)
        {
            Vector3 world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (IsTileOfType<PlaneTile>(tileMap, tileMap.WorldToCell(world)))
            {
                Plane plane = collider.GetComponent<Plane>();
                plane.Toward();
            }
        }

        foreach (Plane plane in planes)
        {
            plane.RefreshData(checkerboard, new Vector2Int(0, 0));
        }
        Draw();
    }

    public bool IsTileOfType<T>(Tilemap tilemap, Vector3Int position) where T : TileBase
    {
        TileBase targetTile = tilemap.GetTile(position);

        if (targetTile != null && targetTile is T)
        {
            return true;
        }

        return false;
    }

    private void _OnDrag(Vector3 pre, Vector3 touch)
    {
        Vector3 preWorld = Camera.main.ScreenToWorldPoint(pre);
        Vector3 touchWorld = Camera.main.ScreenToWorldPoint(touch);

        Vector3Int cell = tileMap.WorldToCell(touchWorld) - tileMap.WorldToCell(preWorld);
        Vector2Int offset = CovertTileMapPosToDataPos(new Vector2Int(cell.x, cell.y));

        Collider2D collider = Physics2D.OverlapPoint(touchWorld, Layers.PlaneMask);
        if (collider)
        {
            if (Mathf.Abs(offset.x) > 0 || Mathf.Abs(offset.y) > 0)
            {
                offset.x = Mathf.Min(offset.x, 1);
                offset.y = Mathf.Min(offset.y, 1);
                offset.x = Mathf.Max(offset.x, -1);
                offset.y = Mathf.Max(offset.y, -1);
                Plane plane = collider.GetComponent<Plane>();
                plane.Move(offset);
                SetPlanePos(plane);

            }
        }
        foreach (Plane plane in planes)
        {
            plane.RefreshData(checkerboard, new Vector2Int(0, 0));
        }

        Draw();
    }

    private Vector2Int CovertDataPosToTileMapPos(Vector2Int data)
    {
        Vector2Int cellPos = new Vector2Int(0, 0)
        {
            x = data.y,
            y = -data.x
        };
        return cellPos;
    }

    private Vector2Int CovertTileMapPosToDataPos(Vector2Int cellPos)
    {
        Vector2Int _ = new Vector2Int(0, 0)
        {
            y = cellPos.x,
            x = -cellPos.y
        };
        return _;
    }

    private void SetPlanePos(Plane plane)
    {
        Vector2Int cellPos = CovertDataPosToTileMapPos(plane.centerPos);
        Vector3 pos = tileMap.CellToWorld(new Vector3Int(cellPos.x, cellPos.y, 0));

        plane.transform.position = pos;

        plane.FixMovePlanePos();
    }

    public void Start()
    {
        checkerboard = new int[row, col];

        Plane plane = Instantiate(objectPlane);
        SetPlanePos(plane);

        Plane plane1 = Instantiate(objectPlane);
        plane1.centerPos = new Vector2Int(1, 2);
        SetPlanePos(plane1);

        planes.Add(plane);
        planes.Add(plane1);
        foreach (Plane p in planes)
        {
            p.RefreshData(checkerboard, new Vector2Int(0, 0));
        }
        Draw();
    }

    public void Draw()
    {
        Vector3Int coordinate = new Vector3Int(0, 0, 0);

        for (int i = 0; i < tileMap.size.x; i++)
        {
            for (int j = 0; j < tileMap.size.y; j++)
            {
                Vector2Int pos = CovertDataPosToTileMapPos(new Vector2Int(i, j));
                coordinate.x = pos.x;
                coordinate.y = pos.y;

                TileBase tileBase = tileMap.GetTile(coordinate);
                if (tileBase)
                {
                    if (checkerboard[i, j] != 0)
                    {
                        tileMap.SetTile(coordinate, null);
                        tileMap.SetTile(coordinate, planeTile);
                    }
                    else
                    {
                        tileMap.SetTile(coordinate, null);
                        tileMap.SetTile(coordinate, wallTile);
                    }
                }
            }
        }
    }
}