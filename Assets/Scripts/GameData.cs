using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Snake/Game Data")]
public class GameData : ScriptableObject
{
    [SerializeField]
    private Vector2Int gridDimensions;
    public Vector2Int GridDimensions => gridDimensions;
    [SerializeField]
    private Vector3 startPositionCenter = Vector3.zero;
    public Vector3 StartPositionCenter => startPositionCenter;
    public Vector3Int RandomGridCoordinatesLocal()
    {
        return new Vector3Int(
            Random.Range(0, gridDimensions.x),
            0,
            Random.Range(0, gridDimensions.y)
            );
    }
    public Vector3Int RandomGridCoordinatesWorld()
    {
        var wallDimensions = GridDimensions + new Vector2Int(2, 2);
        var startPositionWorld = new Vector3Int(
            (int)(startPositionCenter.x - ((float)wallDimensions.x / 2.0f)),
            0,
            (int)(startPositionCenter.y - ((float)wallDimensions.y / 2.0f))
            ) + new Vector3Int(1, 0, 1);
        return startPositionWorld + RandomGridCoordinatesLocal();
    }
}
