using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentTile : MonoBehaviour
{
    public List<EnvironmentTile> Connections { get; set; }
    public EnvironmentTile Parent { get; set; }
    public Vector3 Position { get; set; }
    public float Global { get; set; }
    public float Local { get; set; }
    public bool Visited { get; set; }
    public bool IsAccessible { get; set; }
    public bool isPaddock { get; set; }
    public bool hasFence { get; set; }
    public bool hasWaterBowl { get; set;}
    public bool hasFoodBowl { get; set; }
    public bool isPath { get; set; }
    public bool isEntrance { get; set; }


    //Terrain Painting

    public bool isDirt { get; set; }
    public bool isSand { get; set; }
    public bool isLightGrass { get; set; }
    public bool isDarkGrass { get; set; }

    public bool hasPaint { get; set; }
}
