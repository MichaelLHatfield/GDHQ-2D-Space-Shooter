using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRadar : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if(other.CompareTag("Laser"))
        {
            var enemy = this.transform.parent.GetComponent<Enemy>();
            enemy.AvoidLaser();
        }
    }
}
