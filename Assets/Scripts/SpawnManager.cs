using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{ 
    [SerializeField] private GameObject _enemyPrefab;
	[SerializeField] private GameObject _enemyContainer;
	[SerializeField] private GameObject _tripleShotPowerupPrefab;
	[SerializeField] private GameObject _powerupContainer;
	
	private bool _stopSpawning = false;
	
    // Start is called before the first frame update
    void Start()
    {
	    StartCoroutine(SpawnRoutine());
	    StartCoroutine(SpawnPowerUpRountine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnRoutine()
    {
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
		while(_stopSpawning == false)
		{
			Vector3 positionToSpawn = new Vector3(Random.Range(-8.0f, 8.0f), 7, 0);
			GameObject newEnemy = Instantiate(_tripleShotPowerupPrefab, positionToSpawn, Quaternion.identity);
			newEnemy.transform.parent = _powerupContainer.transform;
		    yield return new WaitForSeconds(5.0f);
		}
	}
    
	public void OnPlayerDeath()
	{
		_stopSpawning = true;
	}
}
