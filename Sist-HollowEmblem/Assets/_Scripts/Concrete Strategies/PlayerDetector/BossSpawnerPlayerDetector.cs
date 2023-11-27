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
    [SerializeField] private AudioClip _bossAudioClip;

    [Space]
    [Header("Prefab Elements")]
    
    [SerializeField] private Transform _cameraLockPosition;
    [SerializeField] private BossUI _bossUI;
    
    private Transform _cameraLockPositionPlayer;
    private Camera _cam;
    private AudioSource _cameraAudioSource;
    private ChangeAmbientMusic _camMusicChanger;

    private void Awake()
    {
        _boss.gameObject.SetActive(false);

        _cam = Camera.main;
        _cameraAudioSource = _cam.GetComponent<AudioSource>();
        _camMusicChanger = _cam.GetComponent<ChangeAmbientMusic>();
    }

    public override void OnPlayerDetect()
    {
        base.OnPlayerDetect();
        _boss.gameObject.SetActive(true);
        _bossUI.InitializeBossUI(_boss);
        _cameraLockPositionPlayer = _playerTransform.GetComponent<Player>().CameraTarget;
        _cameraAudioSource.clip = _bossAudioClip;
        _cameraAudioSource.Play();


        ActionsManager.SubscribeToAction(_boss.gameObject.name + ActionConstants.DEATH, SetPlayerCamera);
        
        foreach (Collider2D wall in _invisibleWalls)
        {
            wall.gameObject.SetActive(true);
        }
        
        _vcam.Follow = _cameraLockPosition;
    }
    
    private void SetPlayerCamera()
    {
        foreach (Collider2D wall in _invisibleWalls)
        {
            wall.gameObject.SetActive(false);
        }

        _camMusicChanger.SetAmbienceMusic();
        _vcam.Follow = _cameraLockPositionPlayer;
        _camMusicChanger.SetAmbienceMusic();
        _cameraAudioSource.Play();

        ActionsManager.UnsubscribeToAction(_boss.gameObject.name + ActionConstants.DEATH, SetPlayerCamera);

    }
}
