﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField] private float _speed = 3.5f;
	[SerializeField] private GameObject _laserPrefab;
	[SerializeField] private float _fireRate = 0.15f;
	private float _canFire = -1f;
	[SerializeField] private int _lives = 3;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
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
		transform.Translate(direction * _speed * Time.deltaTime);

		//check the vertical position and adjust as necessary to keep it between 0 and -3.8
		if (transform.position.y >= 0)
		{
			transform.position = new Vector3(transform.position.x, 0, transform.position.z);
		}
		else if (transform.position.y < -4)
		{
			transform.position = new Vector3(transform.position.x, -4, transform.position.z);
		}

		//check the horizontal position - if the player goes off screen then wrap to other side
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
		Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
	}

	public void Damage()
    {
		_lives = _lives -1;

		//check if dead
		if(_lives < 1)
        {
			Destroy(this.gameObject);
        }
    }
}
