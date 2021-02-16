using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private Player _player;
    [SerializeField]
    private GameObject _explosion;
    private SpawnManager _spawnManager;
    [SerializeField]
    private AudioClip _explosionSound;
    private Camera _mainCamera;
    // Update is called once per frame
    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("The player referenced by laser is Null.");
        }
        _spawnManager = FindObjectOfType<SpawnManager>().GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager referenced by Laser is null.");
        }
        
        _mainCamera = FindObjectOfType<Camera>();
        if (_mainCamera == null)
        {
            Debug.LogError("The Main Camera referenced by Laser is Null.");
        }
    }
    void Update()
    {
        Shooting();
        OutBounds();
    }

    private void Shooting()
    {
        float _speed = 20f;
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
    }

    private void OutBounds()
    {
        if (transform.position.y >= 8f)
        {
            if (this.transform.parent != null)
            {
                Destroy(this.transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Enemy")
        {
            other.GetComponent<Enemy>().CollisionTrue();
            Destroy(other.GetComponent<Collider2D>());
            if (_player != null)
            {
                _player.IncreaseScore(10);
            }
            other.GetComponent<Animator>().SetTrigger("OnEnemyDeath");
            other.GetComponent<Enemy>().SetSpeedZero();
            AudioSource.PlayClipAtPoint(_explosionSound, _mainCamera.transform.position);
            Destroy(other.gameObject, 2.0f);
            Destroy(this.gameObject);
        }    
        else if (other.tag == "Asteroid")
        {
            Destroy(other.GetComponent<Collider2D>());
            GameObject _explosionCopy = Instantiate(_explosion, new Vector3(other.transform.position.x, other.transform.position.y), Quaternion.identity);
            Destroy(other.gameObject);
            _spawnManager.StartSpawn();
            AudioSource.PlayClipAtPoint(_explosionSound, _mainCamera.transform.position);
            Destroy(this.gameObject, 0.25f);
            Destroy(_explosionCopy.gameObject, 2.7f);
        }
    }

    /* I initially got this method to work by enabling 'is kinetic' on the enemy and
     * then using the following code to destroy both the laser and enemy upon collision. 
     * However, the better way would be to use 'isTrigger', which does not risk having 
     * an unwanted surface collisions. It gives us the wanted 'passthrough' collisions.
     * This also avoids the overuse of RigidBodies, which can be performance intensive.
    private void OnCollisionEnter(Collision col)
    {
        Debug.Log("Collision name: " + col.gameObject.name);
        if (col.gameObject.name == "Enemy(Clone)")
        {   
            Destroy(col.gameObject);
            Destroy(this.gameObject);
        }
    }
    */
}
