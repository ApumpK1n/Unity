using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlaneToward
{
    Top,
    Right,
    Bottom,
    Left,
}

public class Plane : MonoBehaviour
{
    //2:机头 1:机翼 3:中心 影响旋转
    public int[,] Top = new int[4, 5]
    {
        { 0,0,2,0,0},
        { 1,1,3,1,1},
        { 0,0,1,0,0},
        { 0,1,1,1,0},
    };

    public int[,] Bottom = new int[4, 5]
    {
        { 0,1,1,1,0},
        { 0,0,1,0,0},
        { 1,1,3,1,1},
        { 0,0,2,0,0},
    };

    public int[,] Right = new int[5, 4]
    {
        { 0,0,1,0},
        { 1,0,1,0},
        { 1,1,3,2},
        { 1,0,1,0},
        { 0,0,1,0},
    };

    public int[,] Left = new int[5, 4]
    {
        { 0,1,0,0},
        { 0,1,0,1},
        { 2,3,1,1},
        { 0,1,0,1},
        { 0,1,0,0},
    };

    private int centerNum = 3;
    private int headNum = 2;
    private int bodyNum = 1;

    public Vector2Int centerPos = new Vector2Int(5, 5);
    private PlaneToward toward = PlaneToward.Top;
    private int[,] checkerboard;

    private int[,] GetPlaneData()
    {
        int[,] Data = Top;

        switch (toward)
        {
            case PlaneToward.Top:
                Data = Top;
                break;
            case PlaneToward.Bottom:
                Data = Bottom;
                break;
            case PlaneToward.Right:
                Data = Right;
                break;
            case PlaneToward.Left:
                Data = Left;
                break;
        }
        return Data;
    }

    private Vector2Int GetOffset()
    {
        Vector2Int offset = new Vector2Int(0, 0); // (0, 0)相对于头部的偏移
        switch (toward)
        {
            case PlaneToward.Top:
                offset = new Vector2Int(-1, -2);
                break;
            case PlaneToward.Bottom:
                offset = new Vector2Int(-2, -2);
                break;
            case PlaneToward.Right:
                offset = new Vector2Int(-2, -2);
                break;
            case PlaneToward.Left:
                offset = new Vector2Int(-2, -1);
                break;
        }
        return offset;
    }

    private bool CanSetPlane(int[,] checkerboard, Vector2Int extraOffset)
    {
        int[,] Data = GetPlaneData();
        Vector2Int offset = GetOffset();
        offset += extraOffset;

        //Debug.Log("===========================>");
        //for (int i = 0; i < checkerboard.GetLength(0); i++)
        //{
        //    Debug.Log($"{checkerboard[i, 0]} {checkerboard[i, 1]} {checkerboard[i, 2]} {checkerboard[i, 3]} {checkerboard[i, 4]} {checkerboard[i, 5]} {checkerboard[i, 6]} {checkerboard[i, 7]} {checkerboard[i, 8]}");
        //}

        for (int i = 0; i < Data.GetLength(0); i++)
        {
            for (int j = 0; j < Data.GetLength(1); j++)
            {
                Vector2Int pos = centerPos + offset + new Vector2Int(i, j);
                if (pos.x < 0 || pos.x >= checkerboard.GetLength(0) || pos.y < 0 || pos.y >= checkerboard.GetLength(1))
                {
                    return false;
                }
                bool cantAdd = (Data[i, j] != 0  && checkerboard[pos.x, pos.y] != 0); 
                if (cantAdd) 
                {
                    return false;
                }
            }
        }
        return true;
    }

    public bool SetData(int[,] checkerboard, Vector2Int extraOffset)
    {
        this.checkerboard = checkerboard;
        Vector2Int temp = centerPos;
        int[,] Data = GetPlaneData();
        Vector2Int offset = GetOffset();
        offset += extraOffset;
        for (int i = 0; i < Data.GetLength(0); i++)
        {
            for (int j = 0; j < Data.GetLength(1); j++)
            {
                Vector2Int pos = centerPos + offset + new Vector2Int(i, j);
                checkerboard[pos.x, pos.y] += Data[i, j];

                if (checkerboard[pos.x, pos.y] == centerNum)
                {
                    temp = pos;
                }
            }
        }
        centerPos = temp;
        return true;
    }

    public void ResetData()
    {
        Vector2Int offset = GetOffset();
        int[,] Data = GetPlaneData();

        for (int i = 0; i < Data.GetLength(0); i++)
        {
            for (int j = 0; j < Data.GetLength(1); j++)
            {
                Vector2Int pos = centerPos + offset + new Vector2Int(i, j);
                if (Data[i, j] != 0 && checkerboard[pos.x, pos.y] != 0)
                {
                    checkerboard[pos.x, pos.y] = 0;
                }
                
            }
        }
    }

    public void RefreshData(int[,] checkerboard, Vector2Int extraOffset)
    {
        if (!CanSetPlane(checkerboard, extraOffset)) return;
        SetData(checkerboard, extraOffset);
    }

    public void Toward()
    {
        int[,] copy = (int[,])checkerboard.Clone();
        PlaneToward preToward = toward;
        ResetData();

        int t = (int)toward + 1;

        t %= Enum.GetValues(typeof(PlaneToward)).Length;
        toward = (PlaneToward)t;

        Vector2Int extraOffset = new Vector2Int(0, 0);
        if (!CanSetPlane(checkerboard, extraOffset))
        {
            checkerboard = copy;
            toward = preToward;
            return;
        }
        SetData(checkerboard, extraOffset);

        RotatePlane();
    }

    public void Move(Vector2Int offset)
    {
        int[,] copy = (int[,])checkerboard.Clone();
        ResetData();

        if (!CanSetPlane(checkerboard, offset)) 
        {
            checkerboard = copy;
            return;
        } 

        SetData(checkerboard, offset);
    }

    public void RotatePlane()
    {
        Vector3 vector = (int)toward * -Vector3.forward * 90;
        transform.rotation = Quaternion.Euler(vector);

        FixRotatePlanePos();
    }

    private void FixRotatePlanePos()
    {
        switch (toward)
        {
            case PlaneToward.Top:
                transform.position += new Vector3(-0.5f, -0.5f, 0);
                break;
            case PlaneToward.Bottom:
                transform.position += new Vector3(0.5f, 0.5f, 0);
                break;
            case PlaneToward.Right:
                transform.position += new Vector3(-0.5f, 0.5f, 0);
                break;
            case PlaneToward.Left:
                transform.position += new Vector3(0.5f, -0.5f, 0);
                break;
        }
    }

    public void FixMovePlanePos()
    {
        switch (toward)
        {
            case PlaneToward.Top:
                transform.position += new Vector3(0.5f, 0, 0);
                break;
            case PlaneToward.Bottom:
                transform.position += new Vector3(0.5f, 1f, 0);
                break;
            case PlaneToward.Right:
                transform.position += new Vector3(0, 0.5f, 0);
                break;
            case PlaneToward.Left:
                transform.position += new Vector3(1f, 0.5f, 0);
                break;
        }
    }
}
