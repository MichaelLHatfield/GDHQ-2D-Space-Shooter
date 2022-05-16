using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitBox : MonoBehaviour
{
    [SerializeField] private GameObject _shieldsEffect;
    private Player _player;
    private bool _isShieldActive = true;
    private static readonly int OnEnemyDeath = Animator.StringToHash("OnEnemyDeath");

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if(_player == null)
        {
            Debug.LogError("Player is null");
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isShieldActive == true && (other.CompareTag("Laser") || other.CompareTag("Player")))
        {
            _isShieldActive = false;
            _shieldsEffect.SetActive(false);
            
            if (other.CompareTag("Laser"))
            {
                Destroy(other.gameObject);
            }
        }
        else
        {
            if (other.CompareTag("Player"))
            {
                Player player = other.transform.GetComponent<Player>();

                if (player != null)
                {
                    player.Damage();
                }
                
                var enemy = this.transform.parent.GetComponent<Enemy>();
                enemy.EnemyDeath();
            }

            if (other.CompareTag("Laser"))
            {
                Destroy(other.gameObject);

                if (_player != null)
                {
                    _player.AddToScore(10);
                }
                var enemy = this.transform.parent.GetComponent<Enemy>();
                enemy.EnemyDeath();
            }
        }
    }
}

