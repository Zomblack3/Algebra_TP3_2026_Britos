using System;
using UnityEngine;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
    [SerializeField] private Vector3Int size = new Vector3Int(10, 10, 10);
    
    [SerializeField] private float spacing = 5f;
    [SerializeField] private float pointSize = 1f;
    
    [SerializeField] private List<GameObject> objs = new List<GameObject>();

    private float maxX;
    private float maxY;
    private float maxZ;

    public int inControl = Movement.inControl;

    public List<Vector3> points = new List<Vector3>();

    private void Start()
    {
        points.Clear();
        
        GeneratePoints();
    }

    private void Update()
    {
        CheckCollisions();
    }

    private void GeneratePoints()
    {
        points.Clear();

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                for (int z = 0; z < size.z; z++)
                {
                    Vector3 position = transform.position + new Vector3(x, y, z) * spacing;

                    points.Add(position);
                }
            }
        }

        maxX = points[size.x - 1].x;
        maxY = points[size.y - 1].y;
        maxZ = points[size.z - 1].z;
    }

    private void CheckCollisions()
    {
        for (int i = 0; i < objs.Capacity; i++)
        {
            if (isOnGrid(inControl) && isOnGrid(i) && i != inControl)
            {
                if (CheckAABB(objs[inControl].transform.position, objs[inControl].transform.localScale, objs[i].transform.position, objs[i].transform.localScale))
                    Debug.Log($"{objs[inControl].name} colisiono con {objs[i].name}");
            }
        }
    }

    private bool CheckAABB(Vector3 posA, Vector3 sizeA, Vector3 posB, Vector3 sizeB)
    {
        Vector3 minA = posA - (sizeA * 0.5f);
        Vector3 maxA = posA + (sizeA * 0.5f);

        Vector3 minB = posB - (sizeB * 0.5f);
        Vector3 maxB = posB + (sizeB * 0.5f);

        bool colisionX = minA.x <= maxB.x && maxA.x >= minB.x;
        bool colisionY = minA.y <= maxB.y && maxA.y >= minB.y;
        bool colisionZ = minA.z <= maxB.z && maxA.z >= minB.z;

        return colisionX && colisionY && colisionZ;
    }

    private bool isOnGrid(int index)
    {
        if (objs[index].transform.position.x < maxX && objs[index].transform.position.x >= 0)
        {
            if (objs[index].transform.position.y < maxY && objs[index].transform.position.y >= 0)
            {
                if (objs[index].transform.position.z < maxZ && objs[index].transform.position.z >= 0)
                {
                    return true;
                }
            }
        }

        return false;
    }
    
    private void OnDrawGizmos()
    {
        if (points == null)
            return;
        
        Gizmos.color = Color.white;
        
        foreach (Vector3 point in points)
        {
            Gizmos.DrawSphere(point, pointSize);
        }
    }
}

