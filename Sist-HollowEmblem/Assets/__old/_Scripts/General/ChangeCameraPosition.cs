using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class ChangeCameraPosition : MonoBehaviour
{

    [SerializeField] CinemachineVirtualCamera vcam;

    [SerializeField] Transform bossTarget;
    [SerializeField] Transform playerTarget;

    [SerializeField] GameObject[] invisibleWalls;

    [SerializeField] GameObject playerCamera;
    [SerializeField] GameObject bossCamera;
    [SerializeField] GameObject activator;

    [SerializeField] GameObject boss;
    [SerializeField] GameObject bossCanvas;

    [SerializeField] Slider healthSlider;
    [SerializeField] Slider secondHealthSlider;

    [SerializeField] HealthController healthController;

    [SerializeField] RectTransform sliderValuePosition;
    [SerializeField] RectTransform secondSliderValuePosistion;

    public ChangeAmbientMusic changeAmbientMusic;

    public static bool bossIsActive;

    public bool canMaximize;

    private void Awake()
    {
        changeAmbientMusic = FindObjectOfType<ChangeAmbientMusic>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            activator.SetActive(false);

            boss.SetActive(true);
            bossCanvas.SetActive(true);
            vcam.Follow = bossTarget;
            bossIsActive = true;
            //changeAmbientMusic.ChangeSong();
            print("changedmusic");

            foreach (GameObject wall in invisibleWalls)
            {
                wall.SetActive(true);
            }

            canMaximize = true;

            collision.TryGetComponent<PlayerRespawn>(out var pr);
            pr.SetRespawn(transform.position);

            //if (boss!= null)
            //{
            //    foreach (GameObject wall in invisibleWalls)
            //    {
            //        wall.SetActive(true);
            //    }

            //    canMaximize = true;

            //    collision.TryGetComponent<PlayerRespawn>(out var pr);
            //    pr.SetRespawn(transform.position);

            //    bossIsActive = true;
            //    changeAmbientMusic.ChangeSong();
                
            //}
        }
    }

    public void SetPlayerCamera()
    {
        foreach (GameObject wall in invisibleWalls)
        {
            wall.SetActive(false);
        }

        PutMaxHealth();

        bossIsActive = false;
        //changeAmbientMusic.ChangeSong();
        vcam.Follow = playerTarget;
        bossCanvas.SetActive(false);

        //playerCamera.SetActive(true);
        //bossCamera.SetActive(false);
    }

    public void PutMaxHealth()
    {
        if (canMaximize)
        {
            canMaximize = false;

            healthController.maxHealth++;

            sliderValuePosition.transform.localPosition = new Vector2(sliderValuePosition.transform.localPosition.x + 50, sliderValuePosition.transform.localPosition.y);
            sliderValuePosition.transform.localScale = new Vector2(sliderValuePosition.transform.localScale.x + 0.3f, sliderValuePosition.transform.localScale.y);

            secondSliderValuePosistion.transform.localPosition = new Vector2(secondSliderValuePosistion.transform.localPosition.x + 50, secondSliderValuePosistion.transform.localPosition.y);
            secondSliderValuePosistion.transform.localScale = new Vector2(secondSliderValuePosistion.transform.localScale.x + 0.3f, secondSliderValuePosistion.transform.localScale.y);

            healthSlider.maxValue = healthController.maxHealth;
            secondHealthSlider.maxValue = healthController.maxHealth;

            healthController.healthPoints = healthController.maxHealth;

            healthSlider.value = healthController.healthPoints;
            secondHealthSlider.value = healthController.healthPoints;
        }
        
    }
}
