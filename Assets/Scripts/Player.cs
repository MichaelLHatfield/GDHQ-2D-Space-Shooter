﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField] private float _speed = 5f;
	private float _superSpeed = 10f;
	private bool _superSpeedActive = false;
	[SerializeField] private GameObject _laserPrefab;
	[SerializeField] private GameObject _tripleshotPrefab;
	[SerializeField] private GameObject _shieldsEffect;
	private SpriteRenderer _shieldAlpha;
	[SerializeField] private GameObject _rightDamage;
	[SerializeField] private GameObject _leftDamage;
	[SerializeField] private GameObject _explosionPrefab;
	[SerializeField] private AudioClip _laserSoundClip;
	private AudioSource _audioSource;
	
	[SerializeField] private float _fireRate = 0.15f;
	private float _canFire = -1f;
	[SerializeField] private int _lives = 3;
	[SerializeField] private int _shieldStrength;
 	private SpawnManager _spawnManager;
	private bool _isTripleShotActive = false;
	private bool _isShieldActive = false;
	[SerializeField] private int _score;
	[SerializeField] private UI_Manager _uiManager;

    // Start is called before the first frame update
    void Start()
    {
	    transform.position = new Vector3(0, -3, 0);
	    _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
		_uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
	    _audioSource = GetComponent<AudioSource>();
		
	    _shieldAlpha = _shieldsEffect.GetComponent<SpriteRenderer>();
	    
	    if(_spawnManager == null)
	    {
	    	Debug.LogError("Spawn Manager is NULL!");
	    }

		if(_uiManager == null)
        {
			Debug.LogError("UI Manager is NULL!");
        }

		if(_audioSource == null)
        {
			Debug.LogError("Audiosource on the player is NULL.");
        }
        else
		{
			_audioSource.clip = _laserSoundClip;
        }
    }

	// Update is called once per frame
	void Update()
	{
		CalculateMovement();

		if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
		{
			FireLaser();
		}
	}

	void CalculateMovement()
	{
		float horizontalInput = Input.GetAxis("Horizontal");
		float verticalInput = Input.GetAxis("Vertical");

		int boost = 1;
		if(Input.GetKey(KeyCode.LeftShift))
		{
			boost = 2;
		}
		
		Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
		if(_superSpeedActive == true)
        {
			transform.Translate(direction * _superSpeed * boost* Time.deltaTime);
		}
		else
		{
			transform.Translate(direction * _speed * boost * Time.deltaTime);
		}

		if (transform.position.y >= 0)
		{
			transform.position = new Vector3(transform.position.x, 0, transform.position.z);
		}
		else if (transform.position.y < -4)
		{
			transform.position = new Vector3(transform.position.x, -4, transform.position.z);
		}

		if (transform.position.x > 11.3f)
		{
			transform.position = new Vector3(-11.3f, transform.position.y, transform.position.z);
		}
		else if (transform.position.x < -11.3f)
		{
			transform.position = new Vector3(11.3f, transform.position.y, transform.position.z);
		}
	}

	void FireLaser()
    {
	    _canFire = Time.time + _fireRate;
	    
	    if(_isTripleShotActive == false)
	    {
	    	Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
	    }
	    else
	    {
	    	Instantiate(_tripleshotPrefab, transform.position, Quaternion.identity);
	    }

		_audioSource.Play();
	    
    }
    
	public void Damage()
    {
		if(_isShieldActive == true)
        {
			_shieldsEffect.SetActive(false);
			_isShieldActive = false;
        }
        else
        {
			_lives = _lives - 1;

			_uiManager.UpdateLivesDisplay(_lives);

			if (_lives < 1)
			{
				_spawnManager.OnPlayerDeath();
				_uiManager.GameOverSequence();
				Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
				Destroy(this.gameObject, 0.5f);
			}
        }
		
	    if(_lives == 2)
	    {
	    	_rightDamage.SetActive(true);
	    }
	    
	    if(_lives == 1)
	    {
	    	_leftDamage.SetActive(true);
	    }
	
    }
    
	public void TripleShotActive()
	{
		_isTripleShotActive = true;
		StartCoroutine(TripleShotCoolDownRoutine());
	}
	
	IEnumerator TripleShotCoolDownRoutine()
	{
		yield return new WaitForSeconds(5.0f);
		_isTripleShotActive = false;
	}

	public void SpeedPowerupActive()
    {
		_superSpeedActive = true;
		StartCoroutine(SpeedPowerupCoolDownRoutine());
    }

	IEnumerator SpeedPowerupCoolDownRoutine()
    {
		yield return new WaitForSeconds(5.0f);
		_superSpeedActive = false;
    }

	public void ActivateShields()
    {
		_isShieldActive = true;
		_shieldsEffect.SetActive(true);
    }

	public void AddToScore(int points)
    {
		_score = _score + points;
		_uiManager.UpdateScoreText(_score);
    }
}
