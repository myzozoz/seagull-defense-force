using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


//This class makes the assumption that the grid coordinates are from an offset hex coordinate with the axes
//  x=rows, from bottom to top
//  &=cols, from left to right;
//Cube coordinates use the q,r and s axes, with the constraint that q+r+x=0 and
//  q=y=columns, from left to right
//  r is the top-left to bottom-right axis, with values growing to the bottom-left
//  s is the top-right to bottom-left axis, with values growing to the top-left
//Note: when using the Unity Vector3Int representation for cube coordinates, x,y and z notation is used, in which case
//for a vector v: v.x=q, v.y=r, v.s=z . This might be confusing. The x-values also need to be flipped for the conversions :).
public static class Hexer
{
    public static Vector3Int GridToCube(Vector3Int g)
    {
        g.x *= -1;
        int q = g.y;
        int r = g.x - (g.y + (g.y & 1)) / 2;
        return new Vector3Int(q,r,-q-r);
    }

    public static Vector3Int CubeToGrid(Vector3Int c)
    {
        int y = c.x;
        int x = c.y + (c.x + (c.x & 1)) / 2;
        return new Vector3Int(-x,y, 0);
    }

    public static int Distance(Vector3Int a, Vector3Int b)
    {
        Vector3Int ac = GridToCube(a);
        Vector3Int bc = GridToCube(b);
        return (Mathf.Abs(bc.x - ac.x) + Mathf.Abs(bc.y - ac.y) + Mathf.Abs(bc.z - ac.z)) >> 1;
    }

    public static List<Vector3Int> GetNeighbors(Vector3Int g)
    {
        return GetCoordinatesInRange(g, 1);
    }

    public static List<Vector3Int> GetCoordinatesInRange(Vector3Int g, int rad)
    {
        Vector3Int c = GridToCube(g);
        HashSet<Vector3Int> gridNeighbours = new HashSet<Vector3Int>();
        for (int q = c.x-rad; q <= c.x+rad; q++)
        {
            for (int r = c.y-rad; r <= c.y+rad; r++)
            {
                for (int s = c.z-rad; s <= c.z+rad; s++)
                {
                    if (q+r+s == 0 && (new Vector3Int(q,r,s) != c))
                    {
                        gridNeighbours.Add(CubeToGrid(new Vector3Int(q, r, s)));
                    }
                }
            }
        }
        return gridNeighbours.ToList();
    }
    
    public static List<Vector3Int> Ring(Vector3Int g, int d)
    {
        d = d < 0 ? -d : d;
        Vector3Int c = GridToCube(g);
        HashSet<Vector3Int> tiles = new HashSet<Vector3Int>();
        for (int i = 0; i < d; i++)
        {
            tiles.Add(CubeToGrid(new Vector3Int(c.x + d, c.y - i, c.z - d + i)));
            tiles.Add(CubeToGrid(new Vector3Int(c.x - d, c.y + i, c.z + d - i)));

            tiles.Add(CubeToGrid(new Vector3Int(c.x - i, c.y + d, c.z - d + 1)));
            tiles.Add(CubeToGrid(new Vector3Int(c.x + i, c.y - d, c.z + d - i)));

            tiles.Add(CubeToGrid(new Vector3Int(c.x - i, c.y - d + i, c.z + d)));
            tiles.Add(CubeToGrid(new Vector3Int(c.x + i, c.y + d - i, c.z - d)));
        }
        return tiles.ToList();
    }

    public static int RingSize(int r)
    {
        r = r < 0 ? -r : r;
        return r == 0 ? 1 : 6 * r;
    }
}
