using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMissileComponent : MonoBehaviour
{

    public ExplosionComponent Explosion;
    
    private Vector3 _direction;
    private float _speed;
    private GameManager _gameManager;

    public void SetMissileData(Vector3 direction, float speed,GameManager gameManager) {
        _direction = direction;
        _speed = speed;
        _gameManager = gameManager;
    }
    
    private void Update()
    {
        transform.position += _direction * _speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Missile Explod");
        if (other.transform.CompareTag("City")) {
            if ( other.GetComponent<CityComponent>().IsAlive) _gameManager.DestroyCity();
            other.GetComponent<CityComponent>().IsAlive = false;
            
        }
        Instantiate(Explosion, transform.position,Quaternion.identity).DoExplosion();
        Destroy(gameObject);
    }
    private void OnDestroy() {
        _gameManager.RemoveActiveAttackMissile(this);
    }
}
