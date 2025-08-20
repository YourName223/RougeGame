using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tiles/Custom Rule Tile")]
public class CustomRuleTile : RuleTile
{
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref UnityEngine.Tilemaps.TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
    }

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject instantiatedGameObject)
    {
        if (instantiatedGameObject != null)
        {
            var sr = instantiatedGameObject.GetComponent<SpriteRenderer>();

            if (sr != null)
            {
                UnityEngine.Tilemaps.TileData tileData = new();
                GetTileData(position, tilemap, ref tileData);
                sr.sprite = tileData.sprite;

                sr.sortingOrder = 5;
            }

        }

        return base.StartUp(position, tilemap, instantiatedGameObject);
    }
}