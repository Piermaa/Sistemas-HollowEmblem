using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class BossSpawnerPlayerDetector : PlayerDetector
{
    [Header("Instance Editable")]
    
    [SerializeField] private CinemachineVirtualCamera _vcam;
    [SerializeField] private Collider[] _invisibleWalls;
    [SerializeField] private BossEnemy _boss;

    [Space]
    [Header("Prefab Elements")]
    
    [SerializeField] private Transform _cameraLockPosition;
    [SerializeField] private Transform _cameraLockPositionPlayer;
    [SerializeField] private BossUI _bossUI;
    
    private void Awake()
    {
        _boss.gameObject.SetActive(false);
    }

    public override void OnPlayerDetect()
    {
        base.OnPlayerDetect();
        _boss.gameObject.SetActive(true);
        _bossUI.InitializeBossUI(_boss);
    }
    
    private void SetPlayerCamera()
    {
        foreach (Collider wall in _invisibleWalls)
        {
            wall.gameObject.SetActive(false);
        }

    //    changeAmbientMusic.ChangeSong();
        _vcam.Follow = _cameraLockPositionPlayer;
    }
}
