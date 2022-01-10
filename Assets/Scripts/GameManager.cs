using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public GameStats GameStat = GameStats.InMenu;
    public CityComponent[] MissileTarget;
    public int AttackMissollaunch;
    
    [Header("Attack Missiles Parameters")]
    public AttackMissileComponent PrefabAttackMissible;
    public Transform[] MissileSpawnPoints;
    public float AttackMissileSpeed = 5;
    public float AttackMissileMinDelay=1f;
    public float AttackMissileMaxDelay = 5f;
    
    [Header("Defence Missile Parameteres")]
    public DefenceMissileComponent PrefabDefenceMissileComponent;
    public LauncherComponent[] DefenceMissileSpawnPoints;
    public float TimeToTarge=2f;
    [Header("Score")]
    public int ScoreByMissileDestory;
    public int ScoreByCityDestroy;
    public int ScoreByMissileLeft;
    
    [Header("UI Elements")]
    [Header("UI EndGame")] 
    public GameObject PanelEndGame;
    public Text TxtEndGameText;
    public Text TxtEndGameScore;
    [Header("UI InGame")] 
    public GameObject PanelInGameUI;
    public Text TxtScore;
    public Text TxtLauncher1;
    public Text TxtLauncher2;
    public Text TxtLauncher3;
    [Header("UI Pause")]
    public GameObject PanelPause;
    [Header("UI Start")]
    public GameObject PanelMainMenu;

    public UnityEvent OnMissileDestroy;
    public UnityEvent OnCityDesctoy;
    
    
    private List<AttackMissileComponent> _activeAttackMissile = new List<AttackMissileComponent>();
    private List<DefenceMissileComponent> _activeDefanceMissile = new List<DefenceMissileComponent>(); 
    private float _missileTimer =0;
    private float _missileDelay;
    private Camera _camera;
    private int _attackMissileToLaunch;
    private int _score;
    private int _cityAlives;
    
    public enum GameStats {
        InGame,InPause,InMenu
    }

    public int Score {
        get { return _score;}
        set {
            _score = value;
            TxtScore.text = "Score : "+_score;
        }
    }

    public void RemoveActiveAttackMissile(AttackMissileComponent missile)
    {
        _activeAttackMissile.Remove(missile);
        Score += ScoreByMissileDestory;
        OnMissileDestroy.Invoke();
    }

    public void RemoveActiveDefenceMissile(DefenceMissileComponent missile)=> _activeDefanceMissile.Remove(missile);

    public void DestroyCity() {
        _cityAlives--;
        Score += ScoreByMissileDestory;
        OnCityDesctoy.Invoke();
    }

    
    void Start() {
        _camera = Camera.main;
        _missileDelay = Random.Range(AttackMissileMinDelay, AttackMissileMaxDelay);
    }

    
    void Update()
    {
        switch (GameStat)
        {
            case GameStats.InGame:
                MissileTimer();
                CheckEndGameConditions();
                if (Input.GetButtonDown("Fire1"))LaunchDefenceMissile();
                break;
            case GameStats.InPause: break;
            case GameStats.InMenu: break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void CheckEndGameConditions() {
        if (_activeAttackMissile.Count == 0 && _attackMissileToLaunch == 0) UIEndGame(true);
        if (_cityAlives <= 0) UIEndGame(false);
    }

    private void MissileTimer()
    {
        if (_attackMissileToLaunch > 0) {
            _missileTimer += Time.deltaTime;
            if (_missileTimer > _missileDelay) {
                LaunchAttackMissile();
                _missileTimer = 0;
                _missileDelay = Random.Range(AttackMissileMinDelay, AttackMissileMaxDelay);
            }
        }
    }

    private void LaunchAttackMissile() {
        Transform spawnPoint = MissileSpawnPoints[Random.Range(0, MissileSpawnPoints.Length)];
        Vector3 target = GetTargetCity();
        Vector3 direction = (target - spawnPoint.position).normalized;
        
        AttackMissileComponent missile = Instantiate(PrefabAttackMissible, spawnPoint.position, Quaternion.identity);
        missile.transform.forward = direction;
        missile.SetMissileData(direction, AttackMissileSpeed,this);
        _activeAttackMissile.Add(missile);
        _attackMissileToLaunch--;


    }

    private void LaunchDefenceMissile() {
        Vector3 viewPort = _camera.ScreenToViewportPoint(Input.mousePosition) + new Vector3(0, 0, 10);
        Vector3 target = _camera.ViewportToWorldPoint(viewPort);
        Debug.DrawLine(_camera.transform.position, target, Color.green);
        Vector3 launcher = GetDefenceLauncher(target);
        if (launcher == new Vector3(-1, -1, -1))
        {
            Debug.Log(" plus de missile");
            return;
        }
        Vector3 direction = (target - launcher).normalized;
        
        
        DefenceMissileComponent missile =
            Instantiate(PrefabDefenceMissileComponent, launcher, Quaternion.identity);
        missile.transform.forward = direction;
        missile.SetMissileData(target, TimeToTarge,this);
        _activeDefanceMissile.Add(missile);
    }

    private Vector3 GetDefenceLauncher(Vector3 target) {
        Vector3 launcher = new Vector3(-1,-1,-1);
        float distance = 1000;
        int index = 1;
        for (int i = 0; i < DefenceMissileSpawnPoints.Length; i++)
        {
            if (Vector3.Distance(target, DefenceMissileSpawnPoints[i].transform.position) < distance&&DefenceMissileSpawnPoints[i].MissileInStock>0) {
                distance = Vector3.Distance(target, DefenceMissileSpawnPoints[i].transform.position);
                launcher = DefenceMissileSpawnPoints[i].transform.position;
                index = i;
            }
        }
        if (index == 0)
        {
            DefenceMissileSpawnPoints[index].MissileInStock--;
            TxtLauncher1.text = DefenceMissileSpawnPoints[index].MissileInStock.ToString();
        }
        else if(index == 1) {
            DefenceMissileSpawnPoints[index].MissileInStock--;
            TxtLauncher2.text = DefenceMissileSpawnPoints[index].MissileInStock.ToString();
        }
        else if(index == 2) {
            DefenceMissileSpawnPoints[index].MissileInStock--;
            TxtLauncher3.text = DefenceMissileSpawnPoints[index].MissileInStock.ToString();
        }
        return launcher;
    }

    private Vector3 GetTargetCity()
    {
        List<CityComponent> aliveCity =new List<CityComponent>();
        foreach (CityComponent city in MissileTarget) {
            if (city.IsAlive)aliveCity.Add(city);
        }
        return aliveCity[Random.Range(0, aliveCity.Count)].transform.position;
    }
    
    // UI Part
    //------------------------------------------------------------------------------------------------------------//

    public void UIClickStart() {
        foreach (AttackMissileComponent missile in _activeAttackMissile) {
            Destroy(missile.gameObject);
        }
        foreach (DefenceMissileComponent missile in _activeDefanceMissile) {
            Destroy(missile.gameObject);
        }
        
        foreach (CityComponent city in MissileTarget) {
            city.IsAlive = true;
        }

        foreach (LauncherComponent launcher in DefenceMissileSpawnPoints) {
            launcher.MissileInStock = 5;
        }

        _cityAlives = MissileTarget.Length;
        
        TxtLauncher1.text = DefenceMissileSpawnPoints[0].MissileInStock.ToString();
        TxtLauncher2.text = DefenceMissileSpawnPoints[1].MissileInStock.ToString();
        TxtLauncher3.text = DefenceMissileSpawnPoints[2].MissileInStock.ToString();
        
        _attackMissileToLaunch = AttackMissollaunch;
        _score = 0;
        TxtScore.text = _score.ToString();
        
        PanelEndGame.SetActive(false);
        PanelMainMenu.SetActive(false);
        PanelPause.SetActive(false);
        PanelInGameUI.SetActive(true);
        Time.timeScale = 1;
        GameStat= GameStats.InGame;
    }

    public void UIPause() {
        Time.timeScale = 0;
        PanelPause.SetActive(true);
        PanelInGameUI.SetActive(false);
        GameStat = GameStats.InPause;
    }

    public void UIUnPause()
    {
        Time.timeScale = 1;
        PanelPause.SetActive(false);
        PanelInGameUI.SetActive(true);
        GameStat = GameStats.InGame;
    }

    public void UIEndGame(bool isWin)
    {
        PanelInGameUI.SetActive(false);
        PanelEndGame.SetActive(true);
        foreach (LauncherComponent launch in DefenceMissileSpawnPoints)
        {
            Score += launch.MissileInStock*ScoreByMissileLeft;
        }

        TxtEndGameScore.text = "SCORE : " + Score;
        if (isWin)
        {
            TxtEndGameText.text = "WIN";
            TxtEndGameText.color = Color.green;
        }
        else
        {
            TxtEndGameText.text = "GAME OVER";
            TxtEndGameText.color = Color.red;
        }

        GameStat = GameStats.InMenu;
    }

    public void UIQuite() {
        Application.Quit();
    }
    

}
