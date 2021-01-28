using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;
using UnityEngine.Animations;

public class Wall : MonoBehaviour
{
    [SerializeField] private GameObject wallCubePrefab = null;
    [SerializeField] private GameData gameData = null; 
    private void Awake()
    {
        transform.position = gameData.StartPositionCenter;
        StartCoroutine(BuildWall());
    }
    private IEnumerator BuildWall()
    {
        var wallDimensions = gameData.GridDimensions + new Vector2Int(2, 2);
        Vector2 startPosition = new Vector2(
            (int)(transform.position.x - ((float)wallDimensions.x / 2.0f)),
            (int)(transform.position.y - ((float)wallDimensions.y / 2.0f))
            );

        for (int i = 0; i < wallDimensions.x; ++i)
        {
            var newCube = Instantiate(
                original: wallCubePrefab,
                position: new Vector3(startPosition.x + i, 0, startPosition.y), 
                rotation: Quaternion.identity,
                parent: transform);
            newCube.gameObject.name = $"BotWall_{i}";
            if (i % 3 == 0)
            {
                yield return null;
            }

        }

        for (int i = 0; i < wallDimensions.x; ++i)
        {
            var newCube = Instantiate(
                original: wallCubePrefab,
                position: new Vector3(startPosition.x + i, 0, 
                                     -startPosition.y),
                rotation: Quaternion.identity,
                parent: transform);
            newCube.gameObject.name = $"TopWall_{i}";
            if (i % 3 == 0)
            {
                yield return null;
            }
        }

        for (int i = 0; i < wallDimensions.y; ++i)
        {
            var newCube = Instantiate(
                original: wallCubePrefab,
                position: new Vector3(startPosition.x, 0, startPosition.y + i),
                rotation: Quaternion.identity,
                parent: transform);
            newCube.gameObject.name = $"LeftWall_{i}";
            if (i % 3 == 0)
            {
                yield return null;
            }
        }

        for (int i = 0; i < wallDimensions.y; ++i)
        {
            var newCube = Instantiate(
                original: wallCubePrefab,
                position: new Vector3(-startPosition.x, 0, startPosition.y + i),
                rotation: Quaternion.identity,
                parent: transform);
            newCube.gameObject.name = $"RightWall_{i}";
            if (i % 3 == 0)
            {
                yield return null;
            }
        }
        yield return null;
    }
}
