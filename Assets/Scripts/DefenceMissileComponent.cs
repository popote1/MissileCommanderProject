using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceMissileComponent : MonoBehaviour
{

    public ExplosionComponent Explosion;
    
    private float _timeToTarget;
    private Vector3 _target;
    private Vector3 _starposition;
    private float _timer=0;
    private GameManager _gameManager;

    public void SetMissileData(Vector3 target, float timeToTarget,GameManager gameManager)
    {
        _starposition = transform.position;
        _timeToTarget = timeToTarget;
        _target = target;
        _gameManager = gameManager;
        _timer=0;
    }
    
    void Update()
    {
       _timer += Time.deltaTime;
       Vector3 pos = CustomBezierPoint(_timer/_timeToTarget);
       transform.forward = pos -transform.position;
       transform.position = pos;

       if (_timeToTarget <_timer) {
           Instantiate(Explosion,transform.position, Quaternion.identity).DoExplosion();
           Destroy(gameObject);
       }
    }

    private Vector3 CustomBezierPoint(float t)
    {
        Vector3 offsett = new Vector3(_starposition.x, _target.y, 0);
        
        Vector3 v1 = Vector3.Lerp(_starposition,offsett, t);
        Vector3 v2 = Vector3.Lerp(offsett, _target, t);
        return Vector3.Lerp(v1, v2, t);
    }

    private void OnDestroy() {
        _gameManager.RemoveActiveDefenceMissile(this);
    }
}
