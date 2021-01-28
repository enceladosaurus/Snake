using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] private GameData gameData = null;
    [SerializeField] private PickUpParticle pickUpParticle = null;
    public void Respawn() 
    {
        gameObject.SetActive(false);
        //trigger particle effect 
        var particle = Instantiate(
            original: pickUpParticle,
            position: transform.position,
            rotation: Quaternion.identity
            );
        //teleport to random grid location
        transform.position = gameData.RandomGridCoordinatesWorld();
        gameObject.SetActive(true);
    }
}
