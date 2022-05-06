using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoomsDayBomb : MonoBehaviour
{
	[SerializeField] private float _speed = 2.0f;
	[SerializeField] private GameObject _babyBombPrefabs;
	[SerializeField] private GameObject _explosionPrefab;
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

	// Update is called once per frame
	void Update()
	{
		transform.Translate(Vector3.down * _speed * Time.deltaTime);

		if (transform.position.y < -6f)
		{
			Destroy(this.gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		Player player = other.transform.GetComponent<Player>();

		if(other.tag == "Player" && player != null)
		{
			//detonate weapon
			Instantiate(_babyBombPrefabs, transform.position, Quaternion.Euler(new Vector3(0, 0, 60)));
			Instantiate(_babyBombPrefabs, transform.position, Quaternion.Euler(new Vector3(0, 0, 120)));
			//Instantiate(_babyBombPrefabs, transform.position, Quaternion.Euler(new Vector3(0, 0, 180)));
			Instantiate(_babyBombPrefabs, transform.position, Quaternion.Euler(new Vector3(0, 0, 240)));
			Instantiate(_babyBombPrefabs, transform.position, Quaternion.Euler(new Vector3(0, 0, 300)));
			Instantiate(_babyBombPrefabs, transform.position, Quaternion.Euler(new Vector3(0, 0, 360)));
			//_speed = 1;
			Destroy(GetComponent<Collider2D>());
			Destroy(gameObject);
		}
	}
}
