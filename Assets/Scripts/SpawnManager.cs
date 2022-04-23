using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{ 
    [SerializeField] private GameObject _enemyPrefab;
	[SerializeField] private GameObject _enemyContainer;
	[SerializeField] private GameObject[] _powerups;
	[SerializeField] private GameObject _powerupContainer;

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
            Vector3 positionToSpawn = new Vector3(Random.Range(-8f, 8f), 7f, 0f);
	        GameObject newEnemy = Instantiate(_enemyPrefab, positionToSpawn, Quaternion.identity);
	        newEnemy.transform.parent = _enemyContainer.transform;
            
		    yield return new WaitForSeconds(5.0f);
        }
    }
    
	IEnumerator SpawnPowerUpRountine()
	{
		yield return new WaitForSeconds(5.0f);

		while (_stopSpawning == false)
		{
			Vector3 positionToSpawn = new Vector3(Random.Range(-8.0f, 8.0f), 7, 0);
			int whichPowerup = Random.Range(0, 3);
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
