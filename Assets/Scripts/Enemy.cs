using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4.0f;
	[SerializeField] private GameObject _laserPrefabs;
	private Player _player;
    private Animator _anim;
    [SerializeField] private AudioClip _explosionSoundClip;
    private AudioSource _audioSource;

    private float _fireRate = 3.0f;
    private float _canFire = -1.0f;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if(_player == null)
        {
            Debug.LogError("Player is null");
        }

        _anim = GetComponent<Animator>();
        if(_anim == null)
        {
            Debug.LogError("Animator is NULL");
        }

        _audioSource = GetComponent<AudioSource>();
        if(_audioSource == null)
        {
            Debug.LogError("Audio source for enemy is null");
        }
        else
        {
            _audioSource.clip = _explosionSoundClip;
        }

    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if(Time.time > _canFire)
        {
            _fireRate = Random.Range(3.0f, 7.0f);
	        _canFire = Time.time + _fireRate;
	        
	        GameObject enemyLaser = Instantiate(_laserPrefabs, transform.position, transform.rotation);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignType();
            }
        }

    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6f)
        {
            float randomX = Random.Range(-9.0f, 9.0f);
            transform.position = new Vector3(randomX, 9.0f, 0.0f);
        }
    }

	private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if(player != null)
            {
                player.Damage();
            }

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 1;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(gameObject, 2.8f);
        }

        if(other.tag == "Laser")
        {
            Destroy(other.gameObject);

            if(_player != null)
            {
                _player.AddToScore(10);
            }

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 1;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }
    }
}
