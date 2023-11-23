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
    [SerializeField] private BossEnemy _boss;
    [SerializeField] private Collider2D[] _invisibleWalls;

    [Space]
    [Header("Prefab Elements")]
    
    [SerializeField] private Transform _cameraLockPosition;
    [SerializeField] private BossUI _bossUI;
    
    private Transform _cameraLockPositionPlayer;

    private void Awake()
    {
        _boss.gameObject.SetActive(false);
    }

    public override void OnPlayerDetect()
    {
        base.OnPlayerDetect();
        _boss.gameObject.SetActive(true);
        _bossUI.InitializeBossUI(_boss);
        _cameraLockPositionPlayer = _playerTransform.GetComponent<Player>().CameraTarget;
        
        ActionsManager.SubscribeToAction(_boss.gameObject.name + ActionConstants.DEATH, SetPlayerCamera);
        
        foreach (Collider2D wall in _invisibleWalls)
        {
            wall.gameObject.SetActive(false);
        }
        
        _vcam.Follow = _cameraLockPosition;
        //    changeAmbientMusic.ChangeSong();
    }
    
    private void SetPlayerCamera()
    {
        foreach (Collider2D wall in _invisibleWalls)
        {
            wall.gameObject.SetActive(false);
        }

    //    changeAmbientMusic.ChangeSong();
        _vcam.Follow = _cameraLockPositionPlayer;

        ActionsManager.UnsubscribeToAction(_boss.gameObject.name + ActionConstants.DEATH, SetPlayerCamera);

    }
}
