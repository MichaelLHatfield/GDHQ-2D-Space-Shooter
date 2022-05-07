using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{ 
    [SerializeField] private GameObject _enemyPrefab;
	[SerializeField] private GameObject _enemyContainer;
	[SerializeField] private GameObject[] _powerups;
	[Header("Lower # means higher spawn rate")]
	[SerializeField] private int[] _powerupFrequencies;
	[SerializeField] private GameObject _powerupContainer;
	[SerializeField] private GameObject _doomsDayEnemy;
	[SerializeField] private int _spawnCount = 0;
	private bool _stopSpawning = false;
	private List<int> _powerUpFrequencyList = new List<int>();
	[SerializeField] private Text _waveText;
	[SerializeField] private GameObject _asteroid;
	private int _waveNumber = 1;
	private bool _playerIsDead = false;

    public void StartSpawning()
	{
        //spawning system to choose which powerup to spawn - only runs once
        //  build an array using the index of the powerUp and the frequency in which it spawns
        //  then choose a random number from 0 to length of new array - that index should be the power up to spawn
        for (int i = 0; i < _powerups.Length; i++)
		{
			for(int k = 0; k < _powerupFrequencies[i]; k++)
			{
				_powerUpFrequencyList.Add(i); 
			}
		}

		 StartCoroutine(SpawnEnemyRoutine());
		 StartCoroutine(SpawnPowerUpRountine());
	}

    IEnumerator SpawnEnemyRoutine()
    {
		yield return new WaitForSeconds(5.0f);

	    while (_stopSpawning == false)
	    {
		    int eRot = Random.Range(0,3);
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
		    
		    Vector3 positionToSpawn = new Vector3(Random.Range(-8f, 8f), 7f, 0f);
		    GameObject newEnemy = Instantiate(_enemyPrefab, positionToSpawn, Quaternion.Euler(new Vector3(0, 0, eRot)));
	        newEnemy.transform.parent = _enemyContainer.transform;
            
		    _spawnCount = _spawnCount + 1;
		    
		    if(_spawnCount > 10)
		    {
		    	_spawnCount = 0;
				_stopSpawning = true;
		    	positionToSpawn = new Vector3(Random.Range(-8f, 8f), 8f, 0f);
			    newEnemy = Instantiate(_doomsDayEnemy, positionToSpawn, Quaternion.identity);
			    newEnemy.transform.parent = _enemyContainer.transform;
		    }
		    
		    yield return new WaitForSeconds(5.0f);
        }

		//kicked out so game over or wave completed??
		if(_playerIsDead == false)
        {
			//wait a few seconds
			yield return new WaitForSeconds(3.0f);

			//remove any enemies from the game
			foreach (Transform child in _enemyContainer.transform)
			{
				GameObject.Destroy(child.gameObject);
			}
			yield return new WaitForSeconds(2.0f);

			//spawn the player an ammo and health pack just to be nice
			Vector3 ammoPositionToSpawn = new Vector3(0, 7, 0);
			GameObject freePowerup1 = Instantiate(_powerups[3], ammoPositionToSpawn, Quaternion.identity);
			freePowerup1.transform.parent = _powerupContainer.transform;
			//and health pack just to be nice
			Vector3 healthPositionToSpawn = new Vector3(0, 8, 0);
			GameObject freePowerup2 = Instantiate(_powerups[4], healthPositionToSpawn, Quaternion.identity);
			freePowerup2.transform.parent = _powerupContainer.transform;


			//start next wave
			_waveNumber++;
			_waveText.enabled = true;
			_waveText.text = "Wave " + _waveNumber;
			_spawnCount = 0;
			Vector3 positionToSpawn = new Vector3(0f, 3f, 0f);
			Instantiate(_asteroid, positionToSpawn, Quaternion.identity);
		}
	}

	IEnumerator SpawnPowerUpRountine()
	{
		yield return new WaitForSeconds(5.0f);

		while (_stopSpawning == false)
		{
			int whichIndex = Random.Range(0, _powerUpFrequencyList.Count);
			int whichPowerup = _powerUpFrequencyList[whichIndex];

			Vector3 positionToSpawn = new Vector3(Random.Range(-8.0f, 8.0f), 7, 0);
			GameObject newPowerup = Instantiate(_powerups[whichPowerup], positionToSpawn, Quaternion.identity);
			newPowerup.transform.parent = _powerupContainer.transform;		

			yield return new WaitForSeconds(Random.Range(3, 8));
		}
	}
	
    
	public void OnPlayerDeath()
	{
		_stopSpawning = true;
		_playerIsDead = true;
	}
}
