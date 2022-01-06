using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class ExplosionComponent : MonoBehaviour
{
    public float ExplosionSpeed;
    public float ExplosionMaxSize;
    public AnimationCurve ExplosionEase;
    public UnityEvent OnExplosion;
    

    [ContextMenu("TextExplosion")]
    public void DoExplosion()
    {
        transform.DOScale(Vector3.one * ExplosionMaxSize, ExplosionSpeed).SetEase(ExplosionEase);
        OnExplosion.Invoke();
        Destroy(gameObject , ExplosionSpeed);
    }
    
}
