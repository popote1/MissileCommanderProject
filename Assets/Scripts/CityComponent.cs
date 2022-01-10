using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityComponent : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer;
    public Sprite CityAlive;
    public Sprite CityDestroy;

    public bool IsAlive
    {
        get { return _isAlive; }
        set
        {
            _isAlive = value;
            if (_isAlive) SpriteRenderer.sprite = CityAlive;
            else SpriteRenderer.sprite = CityDestroy;
        }
    
    }
    private bool _isAlive =true;
}
