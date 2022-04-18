using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
	[SerializeField] private float _speed = 3.0f;
	[SerializeField] private int _powerupID;

	// Start is called before the first frame update
	void Start()
	{
        
	}

	// Update is called once per frame
	void Update()
	{
		transform.Translate(Vector3.down * _speed * Time.deltaTime);

		if(transform.position.y < -7.0f)
		{
			Destroy(this.gameObject);       
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Player")
		{
			Player player = other.transform.GetComponent<Player>();

			if(player != null)
			{
				if(_powerupID == 0)
				{
					player.TripleShotActive();
				}
				else if(_powerupID == 1)
				{
					player.SpeedPowerupActive();
				}
				else if(_powerupID == 2)
				{
					//shield
				}
			}

			Destroy(gameObject);
		}
	}
}
