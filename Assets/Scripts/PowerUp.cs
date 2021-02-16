using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;
    private Player _player;
    [SerializeField] //0 = triple shot, 1 = speed, 2 = shields
    private int powerUpID;
    [SerializeField]
    private AudioClip _powerUpSound;
    private Camera _mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = FindObjectOfType<Camera>();
        if (_mainCamera == null)
        {
            Debug.LogError("The main camera referenced by Power up is null.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        MovePowerUp();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(_powerUpSound, _mainCamera.transform.position);
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                switch (powerUpID)
                {
                    case 0: 
                        player.TripleShot();
                        break;
                    case 1: 
                        player.SpeedUp();
                        break;
                    case 2:
                        player.ActivateShield();
                        break;
                    default:
                        Debug.Log("Not a valid powerup");
                        break;
                }
            }
            Destroy(this.gameObject);
        }
    }

    private void MovePowerUp()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }
}
