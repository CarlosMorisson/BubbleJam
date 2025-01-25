using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public static BossController Instance;
    [Header("Boss Attributes")]
    [Tooltip("Life Of Boss")]
    public int bossLife;

    [SerializeField]
    private Sprite[] spritesDoBoss; //Caso queira mudar quanto menos vida tiver

    private SpriteRenderer actualSprite;

    public static Action<int> OnTakeDamage;
    void Start()
    {
        Instance = this;
        OnTakeDamage += ReceiveDamage;
        GameController.OnGameStateChanged += LoadBossLife;
        actualSprite = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadBossLife(GameController.GameState newState)
    {
        if(newState == GameController.GameState.Store)
            bossLife = PlayerPrefs.GetInt("BossLife");
        else
            PlayerPrefs.SetInt("BossLife", bossLife);
        //actualSprite.sprite = CheckSprite();
    }
    public void ReceiveDamage(int damage)
    {
        bossLife -= damage;
        //actualSprite.sprite = CheckSprite();
        //Colocar a DISGRAAAAAAAAAAAAAAAAAAAAAAÇA AQUI

    }
    private Sprite CheckSprite()
    {
        Sprite sprite;
        sprite = spritesDoBoss[0];
     
        //Verificar Dano
        return sprite;
    }
}
