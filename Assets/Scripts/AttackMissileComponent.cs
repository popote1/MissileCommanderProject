using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMissileComponent : MonoBehaviour
{

    public ExplosionComponent Explosion;
    
    private Vector3 _direction;
    private float _speed;

    public void SetMissileData(Vector3 direction, float speed) {
        _direction = direction;
        _speed = speed;
    }
    
    private void Update()
    {
        transform.position += _direction * _speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Missile Explod");
        Instantiate(Explosion, transform.position,Quaternion.identity).DoExplosion();
        Destroy(gameObject);
    }
}
