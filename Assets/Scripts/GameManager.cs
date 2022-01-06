using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public GameStats GameStat;
    public Transform[] MissileTarget;
    [Header("Attack Missiles Parameters")]
    public AttackMissileComponent PrefabAttackMissible;
    public Transform[] MissileSpawnPoints;
    public float AttackMissileSpeed = 5;
    public float AttackMissileMinDelay=1f;
    public float AttackMissileMaxDelay = 5f;
    [Header("Defence Missile Parameteres")]
    
    public DefenceMissileComponent PrefabDefenceMissileComponent;
    public Transform[] DefenceMissileSpawnPoints;
    public float TimeToTarge=2f;
    
    
    private List<AttackMissileComponent> _activeAttackMissile = new List<AttackMissileComponent>();
    private float _missileTimer =0;
    private float _missileDelay;
    
    
    
    
    public enum GameStats {
        InGame,InPause,InMenu
    }
    
  
    void Start() {
        _missileDelay = Random.Range(AttackMissileMinDelay, AttackMissileMaxDelay);
    }

    
    void Update()
    {
        switch (GameStat)
        {
            case GameStats.InGame:
                MissileTimer();
                break;
            case GameStats.InPause:
                break;
            case GameStats.InMenu:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void MissileTimer() {
        _missileTimer += Time.deltaTime;
        if (_missileTimer > _missileDelay) {
            LaunchAttackMissile();
            _missileTimer = 0;
            _missileDelay = Random.Range(AttackMissileMinDelay, AttackMissileMaxDelay);
        }
    }

    private void LaunchAttackMissile() {
        Transform spawnPoint = MissileSpawnPoints[Random.Range(0, MissileSpawnPoints.Length)];
        Transform target = MissileTarget[Random.Range(0, MissileTarget.Length)];
        Vector3 direction = (target.position - spawnPoint.position).normalized;


        AttackMissileComponent missile = Instantiate(PrefabAttackMissible, spawnPoint.position, Quaternion.identity);
        missile.transform.forward = direction;
        missile.SetMissileData(direction, AttackMissileSpeed);
        _activeAttackMissile.Add(missile);
    }

    private void LaunchDefenceMissile()
    {
        Transform spawnPoint = MissileSpawnPoints[Random.Range(0, MissileSpawnPoints.Length)];
        Transform target = MissileTarget[Random.Range(0, MissileTarget.Length)];
        Vector3 direction = (target.position - spawnPoint.position).normalized;


        AttackMissileComponent missile = Instantiate(PrefabAttackMissible, spawnPoint.position, Quaternion.identity);
        missile.transform.forward = direction;
        missile.SetMissileData(direction, AttackMissileSpeed);
        _activeAttackMissile.Add(missile);
    }
}
