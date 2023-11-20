using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutWall : PlayerDetector, IEnemyDeath
{
    [SerializeField] private AudioSource _damageSound;
    private bool _playerEntered=false;
    public GameObject hideParent;
    private bool _mustFade;
    public AudioSource fadeSound;
    public List<SpriteRenderer> sprites=new List<SpriteRenderer>();
    // Start is called before the first frame update
    private void Awake()
    {
        fadeSound = GetComponent<AudioSource>();
    }
    void Start()
    { 
        var spritesArray = (hideParent.GetComponentsInChildren<SpriteRenderer>());
        foreach (SpriteRenderer sr in spritesArray)
        {
            sprites.Add(sr);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_mustFade)
        {
            StartCoroutine(Awaiting());
        }
    }

    public override void OnPlayerDetect()
    {
        base.OnPlayerDetect();
        Fade();
    }

    public void Fade()
    {
        if (!_playerEntered)
        {
            fadeSound.Play();
            _mustFade = true;
            _playerEntered = true;
        }
    }
    
    private IEnumerator WaitForSound()
    {
        yield return new WaitUntil(()=>!_damageSound.isPlaying);
        gameObject.SetActive(false);
    }

    private IEnumerator Awaiting()
    {
        // Colocar el audio del ascensor
        
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            foreach (SpriteRenderer sr in sprites)
            {
                sr.color = new Color(255, 255, 255, i);
            }

            yield return null;
        }
        _mustFade = false;
    }

    public void InitializeEnemyDeath(Enemy enemy)
    {
        
    }

    public void Death()
    {
        StartCoroutine(WaitForSound());
    }
}
