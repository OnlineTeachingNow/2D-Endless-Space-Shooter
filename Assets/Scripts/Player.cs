using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject _laser;
    [SerializeField]
    private GameObject _tripleShot;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _nextFire = 0.0f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    [SerializeField]
    private float _speed = 5f;
    private float _speedMultiplier = 2f;
    [SerializeField]
    private bool _shootTriple = false;
    [SerializeField]
    private bool _shieldActive = false;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    public int _score;
    [SerializeField]
    private List<GameObject> _damagedEngines;
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _laserShot;
    [SerializeField]
    private AudioClip _explosionSound;
    [SerializeField]
    private GameObject _explosion;
    private UIManager _uiManager;
    private Sprite _livesImage;
    private Camera _mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        //Sets the original player position
        transform.position = new Vector3(0, -5f, 0);
        _spawnManager = GameObject.FindGameObjectWithTag("Spawn Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is Null.");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is Null.");
        }

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("The AudioSource on the Player is Null.");
        }

        _mainCamera = FindObjectOfType<Camera>();
        if (_mainCamera == null)
        {
            Debug.LogError("The Main Camera reference by player is Null.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
        PlayerWrap();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire)
            Shoot();
    }
    private void PlayerMove() 
    {
        //Gets a reference to the Input Keys in the Input Manager
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        //sets the direction of player movement
        Vector3 direction = new Vector3(horizontalInput, verticalInput);
        //Sets the speed of player movement

        //Moves the player
        transform.Translate(direction * _speed * Time.deltaTime);
    }

    private void PlayerWrap()
    {
        //Gets the current x and y positions
        float positionX = transform.position.x;
        float positionY = transform.position.y;

        //Wraps the X position
        if (transform.position.x >= 10.25)
        {
            transform.position = new Vector3(-11.35f, positionY, 0);
        }
        else if (transform.position.x <= -11.35)
        {
            transform.position = new Vector3(10.25f, positionY, 0);
        }

        //wraps the Y position
        if (transform.position.y >= 6)
        {
            transform.position = new Vector3(positionX, -6.6f, 0);
        }
        else if (transform.position.y <= -6.6)
        {
            transform.position = new Vector3(positionX, 6f, 0);
        }
    }

    private void Shoot()
    {
        _nextFire = Time.time + _fireRate;
        if (_shootTriple == true)
        {
            Instantiate(_tripleShot, this.transform.position, Quaternion.identity);
        }
            else
        {
            Instantiate(_laser, new Vector3(this.transform.position.x, this.transform.position.y + 1.05f, 0), Quaternion.identity);
        }
        _audioSource.clip = _laserShot;
        _audioSource.Play();
    }

    public void Damage()
    {
        if (_shieldActive)
        {
            _shieldActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }
        else
        {
            _lives--;
            _uiManager.UpdateLives(_lives);
        }

        switch (_lives)
        {
            case 2:
                int _i = Random.Range(0, _damagedEngines.Count);
                _damagedEngines[_i].SetActive(true);
                _damagedEngines.RemoveAt(_i);
                break;
            case 1:
                _damagedEngines[0].SetActive(true);
                break;
            case 0:
                _spawnManager.OnPlayerDeath();
                GameObject _explosionCopy = Instantiate(_explosion, this.transform.position, Quaternion.identity);
                AudioSource.PlayClipAtPoint(_explosionSound, _mainCamera.transform.position);
                Destroy(this.gameObject, 0.25f);
                break;
        }
    }

    public void TripleShot()
    {
        _shootTriple = true;
        StartCoroutine(TriplePowerDown());   
    }

    private IEnumerator TriplePowerDown()
    {
        yield return new WaitForSeconds(5f);
        _shootTriple = false;
    }

    public void SpeedUp()
    {
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedPowerDown());
    }

    private IEnumerator SpeedPowerDown()
    {
        yield return new WaitForSeconds(5f);
        _speed /= _speedMultiplier;
    }

    public void ActivateShield()
    {
        _shieldActive = true;
        _shieldVisualizer.SetActive(true);
    }

    public void IncreaseScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
