using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System.Collections;

//A B C 
//B
//C

public class Grid : MonoBehaviour
{

    public Tile planeTile;
    public Tilemap tileMap;

    private int col = 9;
    private int row = 9;

    private int[,] checkerboard;

    private List<Plane> planes = new List<Plane>();

    public void Start()
    {
        checkerboard = new int[row, col];
        planes.Add(new Plane());

        foreach (Plane plane in planes)
        {
            plane.SetData(checkerboard);
        }
        StartCoroutine(Draw());
        //Draw();
    }

    public IEnumerator Draw()
    {
        Vector3Int currentCell = tileMap.WorldToCell(transform.position);

        Vector3 tilePosition;
        Vector3Int coordinate = new Vector3Int(0, 0, 0);
        for (int i = 0; i < tileMap.size.x; i++)
        {
            for (int j = 0; j < tileMap.size.y; j++)
            {
                coordinate.x = i; coordinate.y = j;
                tilePosition = tileMap.CellToWorld(coordinate);
                TileBase tileBase = tileMap.GetTile(coordinate);
                if (tileBase && tileBase is SwitchableTile)
                {
                    SwitchableTile tile = (SwitchableTile)tileBase;
                    tile.sprite = tile.GetSprite(1);
                }
   
                Debug.Log(string.Format("Position of tile [{0}, {1}] = ({2}, {3})", coordinate.x, coordinate.y, tilePosition.x, tilePosition.y));
                yield return new WaitForSeconds(0.5f);
                tileMap.RefreshTile(coordinate);
            }
        }
    }
}