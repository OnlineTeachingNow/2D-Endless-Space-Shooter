using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_laser : MonoBehaviour
{
    private Player _player;
    [SerializeField]
    private AudioClip _explosion;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        Shooting();
    }

    private void Shooting()
    {
        float _speed = 20f;
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }

    private void OutOfBounds()
    {
        if (transform.position.y < -7)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _player = FindObjectOfType<Player>().GetComponent<Player>();
            if (_player != null)
            {
                _player.Damage();
                AudioSource.PlayClipAtPoint(_explosion, FindObjectOfType<Camera>().GetComponent<Camera>().transform.position);
            }
        }
    }
}
