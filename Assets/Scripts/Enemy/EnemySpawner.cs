using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawners;

    [SerializeField] private GameObject zombie;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SpawnZombie();
        }
    }

    private void SpawnZombie()
    {
        int randomInt = Random.RandomRange(1, spawners.Length);
        //Debug.Log(randomInt);     // проверка точек спавна
        Transform randomSpawner = spawners[randomInt];
        Instantiate(zombie, randomSpawner.position, randomSpawner.rotation);

    }
}
