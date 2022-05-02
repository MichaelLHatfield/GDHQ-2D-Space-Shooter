using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField] private float _speed = 5f;
	private float _superSpeed = 10f;
	private bool _superSpeedActive = false;
	[SerializeField] private int _thrust = 30;
	[SerializeField] private int _maxThrust = 30;
	private bool _okayToAddToThrust = true;
	private bool _okayToSubtractThrust = true;
	[SerializeField] private GameObject _laserPrefab;
	private bool _isTripleShotActive = false;
	[SerializeField] private GameObject _tripleshotPrefab;
	private bool _isPhotonBlastActive = false;
	[SerializeField] private GameObject _photonBlastPrefab;
	[SerializeField] private GameObject _shieldsEffect;
	private Material _shieldAlpha;
	[SerializeField] private GameObject _rightDamage;
	[SerializeField] private GameObject _leftDamage;
	[SerializeField] private GameObject _explosionPrefab;
	[SerializeField] private AudioClip _laserSoundClip;
	private AudioSource _audioSource;
	
	[SerializeField] private float _fireRate = 0.15f;
	private float _canFire = -1f;
	[SerializeField] private int _lives = 3;
	[SerializeField] private int _shieldStrength = 3;
	[SerializeField] private int _ammoCount = 15;
 	private SpawnManager _spawnManager;
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
	    _shieldAlpha = _shieldsEffect.GetComponent<Renderer>().material;
	    
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

		if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _ammoCount > 0)
		{
			FireLaser();
		}
		
		if(_okayToAddToThrust == true && _thrust < _maxThrust)
		{
			_thrust = _thrust + 1;
			_uiManager.UpdateThrustFuel(_thrust);
			_okayToAddToThrust = false;
			StartCoroutine(IncreaseThrusterFuel());
		}
	}

	void CalculateMovement()
	{
		float horizontalInput = Input.GetAxis("Horizontal");
		float verticalInput = Input.GetAxis("Vertical");

		int boost = 1;
		
		if(Input.GetKey(KeyCode.LeftShift) && _thrust > 0)
		{
			boost = 2;
			if(_okayToSubtractThrust == true && _thrust > 0)
			{
				_thrust = _thrust -1;
				if(_thrust < 0)
				{
					_thrust = 0;
				}
				_uiManager.UpdateThrustFuel(_thrust);
				_okayToSubtractThrust = false;
				StartCoroutine(DecreaseThrusterFuel());
			}
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
	    _ammoCount = _ammoCount - 1;
	    
	    
	    if(_isTripleShotActive == true)
	    {
	    	Instantiate(_tripleshotPrefab, transform.position, Quaternion.identity);
	    }
	    else if(_isPhotonBlastActive == true)
	    {
		    Instantiate(_photonBlastPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);

	    }
	    else
	    {
	    	Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
	    }

	    _uiManager.UpdateAmmoCount(_ammoCount);
	    
		_audioSource.Play();
	    
    }
    
	public void Damage()
	{
		float alphaVal = 1.0f;
		_shieldStrength = _shieldStrength - 1;

		if(_isShieldActive == true)
		{
			//damge the shield instead of the player
			//change alpha of shield effct to match state
			if(_shieldStrength == 2)
			{
				alphaVal = 0.66f;
			}
			else if(_shieldStrength == 1)
			{
				alphaVal = 0.33f;
			}
			else if(_shieldStrength < 1)
			{
				_shieldsEffect.SetActive(false);
				_isShieldActive = false;
			}			
			
			Color oldColor = _shieldAlpha.color;
			Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, alphaVal);
			_shieldAlpha.SetColor("_Color", newColor);
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
	
	public void PhotonBlastActive()
	{
		_isPhotonBlastActive = true;
		StartCoroutine(PhotonBlastCoolDownRoutine());
	}
	
	IEnumerator PhotonBlastCoolDownRoutine()
	{
		yield return new WaitForSeconds(5.0f);
		_isPhotonBlastActive = false;
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
		//reset alpha to 1 before using shields again
		Color oldColor = _shieldAlpha.color;
		Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, 1.0f);
		_shieldAlpha.SetColor("_Color", newColor);
		
		_isShieldActive = true;
		_shieldsEffect.SetActive(true);
		_shieldStrength = 3;
	}
    
	public void AmmoPickedUp()
	{
		_ammoCount = 15;
		_uiManager.UpdateAmmoCount(_ammoCount);
	}
	
	public void HealthPackPickup()
	{
		if(_lives < 3)
		{
			_lives = _lives + 1;
		}
		
		//adjust damage visuals as needed
		_rightDamage.SetActive(false);
		_leftDamage.SetActive(false);
		
		if(_lives == 2)
		{
			_rightDamage.SetActive(true);
		}
	    
		if(_lives == 1)
		{
			_leftDamage.SetActive(true);
		}
		
		_uiManager.UpdateLivesDisplay(_lives);

	}

	public void AddToScore(int points)
    {
		_score = _score + points;
		_uiManager.UpdateScoreText(_score);
    }
    
	IEnumerator DecreaseThrusterFuel()
	{
		yield return new WaitForSeconds(0.25f);
		_okayToSubtractThrust = true;
	}
	
	IEnumerator IncreaseThrusterFuel()
	{
		yield return new WaitForSeconds(2.0f);
		_okayToAddToThrust = true;
	}
}
