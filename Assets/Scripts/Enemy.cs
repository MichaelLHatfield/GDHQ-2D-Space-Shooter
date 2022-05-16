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
	[SerializeField] private bool _enemyFireEnabled = false;
	
    private float _fireRate = 3.0f;
    private float _canFire = -1.0f;
    private static readonly int OnEnemyDeath = Animator.StringToHash("OnEnemyDeath");

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

        if (_enemyFireEnabled != true) return;

        if (!(Time.time > _canFire)) return;
        
        _fireRate = Random.Range(3.0f, 7.0f);
        _canFire = Time.time + _fireRate;
	        
        GameObject enemyLaser = Instantiate(_laserPrefabs, transform.position, transform.rotation);
        Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

        foreach (var t in lasers)
        {
            t.AssignType();
        }
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * (_speed * Time.deltaTime));

        if (transform.position.y < -6f)
        {
            float randomX = Random.Range(-9.0f, 9.0f);
            transform.position = new Vector3(randomX, 9.0f, 0.0f);
        }
    }

    public void EnemyDeath()
    {
        _speed = 0f;
        _anim.SetTrigger(OnEnemyDeath);
        _audioSource.Play();
        Destroy(GetComponent<Collider2D>());
        Destroy(gameObject, 2.8f);
    }

    public void AvoidLaser()
    {
        //random swerve to try to avoid laser
        var eRot = Random.Range(0,2);
        switch (eRot)
        {
            case 0:
                eRot = -30;
                break;
            case 1:
                eRot = 30;
                break;
            default:
                eRot = 0;
                break;
        }

        transform.Rotate(0f, 0f, eRot);
    }
}
