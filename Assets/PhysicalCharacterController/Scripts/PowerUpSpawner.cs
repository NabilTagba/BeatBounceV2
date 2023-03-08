using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    /*public enum PowerUps
    {
        Nothing,
        ThrowSpeedIncrease,

    }
    PowerUps currentPU;*/

    public float PUSpawnTime;
    bool SpawningPU = false;
    public GameObject currentPU;
    public GameObject PUToSpawn;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void StartRound()
    {
        SpawningPU = false;
        Destroy(currentPU.gameObject);
    }

    public void SpawnPowerUp()
    {
        SpawningPU = false;

    }

    // Update is called once per frame
    void Update()
    {
        if(currentPU == null && !SpawningPU)
        {
            SpawningPU = true;
            Invoke("SpawnPowerUp", PUSpawnTime);
        }
    }
}
