using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlaneToward
{
    Top,
    Bottom,
    Right,
    Left,
}

public class Plane : MonoBehaviour
{
    public int[,] Top = new int[4, 5]
    {
        { 0,0,2,0,0},
        { 1,1,1,1,1},
        { 0,0,1,0,0},
        { 0,1,1,1,0},
    };

    public int[,] Bottom = new int[4, 5]
    {
        { 0,1,1,1,0},
        { 0,0,1,0,0},
        { 1,1,1,1,1},
        { 0,0,2,0,0},
    };

    public int[,] Right = new int[5, 4]
    {
        { 0,0,1,0},
        { 1,0,1,0},
        { 1,1,1,2},
        { 1,0,1,0},
        { 0,0,1,0},
    };

    public int[,] Left = new int[5, 4]
    {
        { 0,1,0,0},
        { 0,1,0,1},
        { 2,1,1,1},
        { 0,1,0,1},
        { 0,1,0,0},
    };

    private Vector2Int headGridPos = new Vector2Int(0, 4);
    private PlaneToward toward = PlaneToward.Top;

    public bool SetData(int[,] checkerboard)
    {
        Vector2Int offset = new Vector2Int(0, 0);
        int[,] Data = Top;
        switch (toward)
        {
            case PlaneToward.Top:
                offset = new Vector2Int(-2, 0);
                Data = Top;
                break;
            case PlaneToward.Bottom:
                offset = new Vector2Int(-2, -3);
                Data = Bottom;
                break;
            case PlaneToward.Right:
                offset = new Vector2Int(-3, -2);
                Data = Right;
                break;
            case PlaneToward.Left:
                offset = new Vector2Int(0, -2);
                Data = Left;
                break;
        }

        for (int i = 0; i < Data.GetLength(0); i++)
        {
            for (int j = 0; j < Data.GetLength(1); j++)
            {
                Vector2Int pos = headGridPos + offset + new Vector2Int(i, j);
                if (pos.x >= 0 && pos.x < checkerboard.GetLength(0) && pos.y >= 0 && pos.y < checkerboard.GetLength(1))
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
        }

        for (int i = 0; i < Data.GetLength(0); i++)
        {
            for (int j = 0; j < Data.GetLength(1); j++)
            {
                Vector2Int pos = headGridPos + offset + new Vector2Int(i, j);
                checkerboard[pos.x, pos.y] = Data[i, j];
            }
        }
        return true;
    }
}
