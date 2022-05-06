using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoomsDayEnemy : MonoBehaviour
{
 	[SerializeField] private GameObject _bombPrefab;
	[SerializeField] private GameObject _explosionPrefab;
	[SerializeField] private int _speed = 2;
	private bool _launchedWeapon = false;
	private Player _player;
	[SerializeField] private AudioClip _explosionSoundClip;
	private AudioSource _audioSource;

	// Start is called before the first frame update
	void Start()
	{
		_player = GameObject.Find("Player").GetComponent<Player>();
		if(_player == null)
		{
			Debug.LogError("Player is null");
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
	
	void Update()
	{
		transform.Translate(Vector3.down * _speed * Time.deltaTime);

		if (transform.position.y < 4f && _launchedWeapon == false)
		{
			Instantiate(_bombPrefab, transform.position - new Vector3(0,2,0), Quaternion.identity);
			_speed = -_speed;
			_launchedWeapon = true;
		}
		
		if(transform.position.y > 9f)
		{
			Destroy(this.gameObject);
		}
	}


	private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Laser")
		{
			Destroy(other.gameObject);

			if(_player != null)
			{
				_player.AddToScore(100);
			}

			_audioSource.Play();
			Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
			Destroy(other.gameObject);
			Destroy(GetComponent<Collider2D>());
			Destroy(this.gameObject, 2f);
		}
	}	
}
