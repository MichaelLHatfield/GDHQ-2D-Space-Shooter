using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRadar : MonoBehaviour
{
    private bool _iDidThisAlready = false;

    private void Start()
    {
        StartCoroutine(ResetDodgeAbility());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //enemy only gets 1 dodge at a time

        if (!_iDidThisAlready)
        {
            if (other.CompareTag("Laser"))
            {
                var enemy = this.transform.parent.GetComponent<Enemy>();
                enemy.AvoidLaser();
                _iDidThisAlready = true;
            }
        }
    }

    IEnumerator ResetDodgeAbility()
    {
        while (true)
        {
            yield return new WaitForSeconds(5.0f);
            _iDidThisAlready = false;
        }
    }
}
