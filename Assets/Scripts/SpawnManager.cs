using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void StartSpawning()
	{
        //spawning system to choose which powerup to spawn - only runs once
        //  build an array using the index of the powerUp and the frequency in which it spawns
        //  then choose a random number from 0 to length of new array - that index should be the power up to spawn
        for (int i = 0; i < _powerups.Length; i++)
		{
			for(int k = 0; k < _powerupFrequencies[i]; k++)
			{
				_powerUpFrequencyList.Add(i);  //should look like 0,0,0,0,1,1,1,etc.
			}
		}

		//for( int i = 0; i < _powerUpFrequencyList.Count; i++)
        //{
		//	Debug.Log("List: " + i + "=" + _powerUpFrequencyList[i]);
		//}

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
		    	positionToSpawn = new Vector3(Random.Range(-8f, 8f), 8f, 0f);
			    newEnemy = Instantiate(_doomsDayEnemy, positionToSpawn, Quaternion.identity);
			    newEnemy.transform.parent = _enemyContainer.transform;
		    }
		    
		    yield return new WaitForSeconds(5.0f);
		    
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
	}
}
