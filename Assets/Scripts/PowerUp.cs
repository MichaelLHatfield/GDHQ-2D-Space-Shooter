using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
	[SerializeField] private float _speed = 3.0f;
	[SerializeField] private int _powerupID;
	[SerializeField] private AudioClip _powerupSoundClip;


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

			AudioSource.PlayClipAtPoint(_powerupSoundClip, transform.position);

			if(player != null)
			{
				switch(_powerupID)
				{
					case 0:
						player.TripleShotActive();
						break;
					case 1:
						player.SpeedPowerupActive();
						break;
					case 2:
						player.ActivateShields();
						break;
					case 3:
						player.AmmoPickedUp();
						break;
					case 4:
						player.HealthPackPickup();
						break;
					case 5:
						player.PhotonBlastActive();
						break;
					default:
						Debug.Log("error in switch statemenet");
						break;
				}
			}

			Destroy(gameObject);
		}
	}
}
