using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{ 
    [SerializeField] private GameObject _enemyPrefab;
	[SerializeField] private GameObject _enemyContainer;
	[SerializeField] private GameObject[] _powerups;
	[SerializeField] private GameObject _powerupContainer;
	[SerializeField] private GameObject _doomsDayEnemy;
	[SerializeField] private int _spawnCount = 0;

	private bool _stopSpawning = false;
	
    public void StartSpawning()
    {
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
			Vector3 positionToSpawn = new Vector3(Random.Range(-8.0f, 8.0f), 7, 0);
			int whichPowerup = Random.Range(0, _powerups.Length);
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
