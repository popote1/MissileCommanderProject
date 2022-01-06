using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceMissileComponent : MonoBehaviour
{

    public ExplosionComponent Explosion;
    
    private float _timeToTarget;
    private Vector3 _target;
    private Vector3 _starposition;
    private float _timer;

    public void SetMissileData(Vector3 target, float timeToTarget)
    {
        _timeToTarget = timeToTarget;
        _target = target;
    }
    
    void Update()
    {
        _timer += Time.deltaTime;
        Vector3 pos = Vector3.Lerp(_starposition, _target , _timer/_timeToTarget);
        transform.position = pos;

        if (_timeToTarget > _timer) {
            Instantiate(Explosion,transform.position, Quaternion.identity).DoExplosion();
            Destroy(gameObject);
        }
    }
    
    
}
