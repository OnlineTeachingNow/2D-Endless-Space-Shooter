using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float _speed = 5f;
    //The enemy is always moving down, so this code was not useful.
    //private Vector3[] _direction = {new Vector3(-1, 0, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, -1, 0)};
    [SerializeField]
    private AudioClip _explosion;
    [SerializeField]
    private GameObject _enemyLaser;
    private Vector3 _setDirection;
    private Animator _anim;
    private Camera _mainCamera;
    private bool _collisiontrue;

    private void Start()
    {
        _mainCamera = FindObjectOfType<Camera>();
        if (_mainCamera == null)
        {
            Debug.LogError("The Main Camera reference by Enemy is null.");
        }
        _anim = gameObject.GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("The animator is Null.");
        }
    }
    private void OnEnable()
    {
        SetDirectionSpeed();
        StartCoroutine(Shooting());
    }

    // Update is called once per frame
    void Update()
    {
        MoveEnemy();
    }

    private void SetDirectionSpeed()
    {
        _setDirection = Vector3.down;
    }

    private void MoveEnemy()
    {
        transform.Translate(_setDirection * _speed* Time.deltaTime);
        //Move to top when reaches bottom of screen
        if (transform.position.y < -6.6f)
        {
            float randomX = Random.Range(-11.35f, 10.25f);
            transform.position = new Vector3(randomX, 6f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _collisiontrue = true;
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            _anim.SetTrigger("OnEnemyDeath");
            SetSpeedZero();
            AudioSource.PlayClipAtPoint(_explosion, _mainCamera.transform.position);
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.0f);
        }
    }

    public void SetSpeedZero()
    {
        _speed = 0;
    }

    private IEnumerator Shooting()
    {
        while (_collisiontrue != true)
        {
            Instantiate(_enemyLaser, this.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(1.0f, 4.0f));
        }
    }

    public void CollisionTrue()
    {
        _collisiontrue = true;
    }

    /* ALL THIS CODE JUST REVERSED POSITIONS WHEN THE ENEMY HIT THE BOUNDARIES. 
    if (this.transform.position.x >= 10.25f || this.transform.position.x <= -11.35f || this.transform.position.y >= 6f || this.transform.position.y <= -0.5f)
    { 
        switch (_setDirection.x)
    {
        case -1:
            _setDirection.x = 1;
            break;
        case 1:
            _setDirection.x = -1;
            break;
    }
    switch (_setDirection.y)
    {
        case -1:
            _setDirection.y = 1;
            break;
        case 1:
            _setDirection.y = -1;
            break;
    }
    */
}


