using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraFeedBackComponant : MonoBehaviour
{
    public Volume Post;
    [Header("Camera Shake Parameters")] 
    public float ShakeTime;
    public float ShakeStrenght;
    public int ShakeVibrato;

    [Header(" CityDestroy Effect")] 
    public float CityDetroyTime;
    public AnimationCurve LensDistortionCurve;
    public AnimationCurve GrainCurve;
    public AnimationCurve ChromaticCurve;
    public AnimationCurve WhiteBalanceCurve;
    

    private Vector3 _originalPostion;
    private int _indexShake;
    private bool _isCityEffect;
    private float _cityTimer;
    void Start()
    {
        _originalPostion = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isCityEffect)CityDestroyEffect();
    }

    [ContextMenu("Test Shake")]
    public void Missileshake() {
        transform.DOPause();
        transform.position = _originalPostion;
        transform.DOShakePosition(ShakeTime, ShakeStrenght,ShakeVibrato);
    }
    
    public void CityDestroyEffect()
    {
        _cityTimer += Time.deltaTime;
        float t = _cityTimer / CityDetroyTime;
        ((LensDistortion) Post.profile.components[0]).intensity.value = LensDistortionCurve.Evaluate(t);
        ((FilmGrain) Post.profile.components[1]).intensity.value = GrainCurve.Evaluate(t);
        ((ChromaticAberration) Post.profile.components[3]).intensity.value = ChromaticCurve.Evaluate(t);
        ((WhiteBalance) Post.profile.components[5]).temperature.value = WhiteBalanceCurve.Evaluate(t);
        
        
       
       
       
        if (_cityTimer > CityDetroyTime) {
            _isCityEffect = false;
        }
    }

    [ContextMenu("Test City")]
    public void CityDetroy()
    {
        _cityTimer = 0;
        _isCityEffect = true;
    }
}
