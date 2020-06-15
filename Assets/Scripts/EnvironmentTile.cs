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

    string originalMat;
    public void setOriginalMaterial(string o)
    {
        string[] fix = o.Split(' ');
        originalMat = fix[0];
        Debug.Log(originalMat);
    }

    public string getOriginalMat()
    {
        return originalMat;
    }

    //Terrain Painting
    string terrainPaint;

    public void setTerrainPaint(string p)
    {
        terrainPaint = p;
    }
    public string getTerrainPaint()
    {
        return terrainPaint;
    }

    public bool hasPaint { get; set; }

    GameObject controlObj;
    public void setControlObject(GameObject c)
    {
        controlObj = c;
    }

    public GameObject getControlObj()
    {
        return controlObj;
    }
}
