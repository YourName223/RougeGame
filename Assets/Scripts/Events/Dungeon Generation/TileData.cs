using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class TileData
{
    public Vector2Int position;
    public TileBase tile;
}

public class RoomData
{
    public List<TileData> tiles = new();
}