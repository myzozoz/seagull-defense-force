using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Tower", menuName = "ScriptableObjects/Tower")]
public class TowerSO : ScriptableObject
{
    public new string name;
    public Tile towerTile;
    public int cost;
}
