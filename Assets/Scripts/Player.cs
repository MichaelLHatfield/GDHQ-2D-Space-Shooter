using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField] private float _speed = 3.5f;
	private float _superSpeed = 7.0f;
	private bool _superSpeedActive = false;
	[SerializeField] private GameObject _laserPrefab;
	[SerializeField] private GameObject _tripleshotPrefab;
	[SerializeField] private float _fireRate = 0.15f;
	private float _canFire = -1f;
	[SerializeField] private int _lives = 3;
	private SpawnManager _spawnManager;
	[SerializeField] private bool _isTripleShotActive = false;
	

    // Start is called before the first frame update
    void Start()
    {
	    transform.position = new Vector3(0, -3.5f, 0);
	    _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
	    
	    if(_spawnManager == null)
	    {
	    	Debug.LogError("Spawn Manager is NULL!");
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

		Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
		if(_superSpeedActive == true)
        {
			transform.Translate(direction * _superSpeed * Time.deltaTime);
		}
		else
		{
			transform.Translate(direction * _speed * Time.deltaTime);
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
    }
    
	public void Damage()
    {
		_lives = _lives -1;

		if(_lives < 1)
		{
			_spawnManager.OnPlayerDeath();
			Destroy(this.gameObject);
        }
    }
    
	public void TripleShotActive()
	{
		_isTripleShotActive = true;
		StartCoroutine(TripleShotPowerDownRoutine());
	}
	
	IEnumerator TripleShotPowerDownRoutine()
	{
		yield return new WaitForSeconds(5.0f);
		_isTripleShotActive = false;
	}

	public void SpeedPowerupActive()
    {
		_superSpeedActive = true;
		StartCoroutine(SpeedPowerupPowerDownRoutine());
    }

	IEnumerator SpeedPowerupPowerDownRoutine()
    {
		yield return new WaitForSeconds(5.0f);
		_superSpeedActive = false;
    }
}
