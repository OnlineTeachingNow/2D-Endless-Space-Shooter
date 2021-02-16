using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemy;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private float _spawnRate = 5f;
    [SerializeField]
    private GameObject[] _powerUps;
    private bool _stopSpawningEnemy = false;
    private bool _stopSpawningPowerUp = false;
    private Vector3 _posToSpawn;
    private float _spawnDelay = 2.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void StartSpawn()
    {
        StartCoroutine(SpawnEnemy());
        StartCoroutine(SpawnPowerUp());
    }

    // Update is called once per frame
    void Update()
    {
        /*This was my first attempt. I decided to say that if the player disappears (which it does when it dies, then to stop the coroutine. 
        /*if (_player == null)
        {
            StopCoroutine(Spawn());
        }
        
        /* It makes more sense to use a Coroutine for this function. This is the first attempt. 
                //use Time.time to determine when enemies are spawned. 
        //variable to update spawnrate
         * if (_nextSpawn < Time.time)
        {
            _nextSpawn = Time.time + _spawnRate;
          Instantiate(enemy, new Vector3 (Random.Range(-10.3f, 12.75f), Random.Range(-0.5f, 6f), 0), Quaternion.identity);
        }
        */
    }

    private IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(_spawnDelay);
        while (_stopSpawningEnemy == false)
        {
            _posToSpawn = new Vector3(Random.Range(-9.3f, 9.1f), 6f, 0f);
            GameObject newEnemy = Instantiate(_enemy, _posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_spawnRate);
        }
    }

    private IEnumerator SpawnPowerUp()
    {
        yield return new WaitForSeconds(_spawnDelay);
        while (_stopSpawningPowerUp == false)
        {
            _posToSpawn = new Vector3(Random.Range(-9.3f, 9.1f), 6f, 0f);
            GameObject _powerUp = _powerUps[Random.Range(0, _powerUps.Length)];
            GameObject newPowerUp = Instantiate(_powerUp, _posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(5f, 10f));
        }
    }
    public void OnPlayerDeath()
    {
        _stopSpawningEnemy = true;
        _stopSpawningPowerUp = true;
    }
}
