using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public enum SpawnState { SPAWNING, WAITING, COUNTING};

    // VARIABLES
    [SerializeField] private Wave[] waves;

    [SerializeField] private float timeBetweenWaves = 3f;
    [SerializeField] private float waveCountdown = 0;

    private SpawnState state = SpawnState.COUNTING;

    private int currentWave;

    // REFERENCES
    [SerializeField] private Transform[] spawners;


    private void Start()
    {
        waveCountdown = timeBetweenWaves;
        currentWave = 0;
    }

    private void Update()
    {
        if (waveCountdown <= 0)
        {
            // spawn enemies
            if (state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave(waves[currentWave]));
            }

        }
        else
            waveCountdown -= Time.deltaTime;



        //TESTING
        //if (Input.GetKeyDown(KeyCode.P))
        //    SpawnZombie();


    }

    private IEnumerator SpawnWave(Wave wave)
    {
        state = SpawnState.SPAWNING;

        for(int i = 0; i < wave.enemiesAmount; i++)
        {
            SpawnZombie(wave.enemy);
            yield return new WaitForSeconds(wave.delay);
        }
        
        state = SpawnState.WAITING;

        yield break;
    }

    [System.Obsolete] // игнорить устаревший метод
    private void SpawnZombie(GameObject enemy)
    {
        int randomInt = Random.RandomRange(1, spawners.Length);
        //Debug.Log(randomInt);     // проверка точек спавна
        Transform randomSpawner = spawners[randomInt];
        Instantiate(enemy, randomSpawner.position, randomSpawner.rotation);

    }
}