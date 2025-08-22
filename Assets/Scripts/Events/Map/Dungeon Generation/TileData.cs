using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum RoomType
{
    Normal,
    Boss,
    Shop,
    Hidden,
}

[Serializable]
public class TileData
{
    public Vector2Int position;
    public TileBase tile;
}

public class RoomData
{
    public bool cleared = false;

    public Dictionary<Vector2Int, Vector2Int> doorPositions = new();

    public Dictionary<Vector2Int, TileData> tiles = new();

    public RoomType roomType = RoomType.Normal; // Default to Normal
}