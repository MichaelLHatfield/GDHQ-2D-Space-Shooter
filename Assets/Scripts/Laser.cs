using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float _speed = 8.0f;
	[SerializeField] private bool _isEnemyLaser = false;
    private bool _isParentNotNull;


    // Update is called once per frame
    private void Start()
    {
        _isParentNotNull = transform.parent != null;
    }

    void Update()
    {
         if(_isEnemyLaser == false)
         {
             MoveUpward();
         }
         else
         {
             MoveDown();
         }
    }

    void MoveUpward()
    {
        transform.Translate(Vector3.up * (_speed * Time.deltaTime));

        if (transform.position.y > 8)
        {
            if (_isParentNotNull)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    void MoveDown()
    {
        transform.Translate(Vector3.down * (_speed * Time.deltaTime));

        if (transform.position.y < -8)
        {
            if (_isParentNotNull)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    public void AssignType()
    {
        _isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && _isEnemyLaser == true)
        {
            Player player = other.GetComponent<Player>();

            if(player != null)
            {
                player.Damage();
            }	    
	        
	        Destroy(this.gameObject);

        }
        
    }
}


