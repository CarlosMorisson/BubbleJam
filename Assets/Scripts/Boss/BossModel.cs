using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using NEO.Utils;


public class BossModel : MonoBehaviour
{
    public static BossModel Instance;
    [Header("Boss Attributes")]
    [Tooltip("Life Of Boss")]
    public float bossLife;

    public float maxBossLife;

    [SerializeField]
    private float yAxisToGo;
    [SerializeField]
    private float timeToGo;

    [SerializeField]
    private Sprite[] spritesDoBoss; //Caso queira mudar quanto menos vida tiver

    [SerializeField]
    private Material blinkMaterial; //Material para dar o efeito de Piscar quando recebe dano
    private Material previousMaterial;
    private SpriteRenderer actualSprite;
    public static Action<int> OnTakeDamage;
    void Start()
    {
        Instance = this;
        OnTakeDamage += ReceiveDamage;
        GameController.OnGameStateChanged += LoadBossLife;
        actualSprite = GetComponentInChildren<SpriteRenderer>();
        previousMaterial = actualSprite.material;
    }
    private void OnEnable()
    {
        gameObject.transform.DOLocalMoveY(yAxisToGo, timeToGo);
        bossLife = PlayerPrefs.GetInt("BossLife");
    }

    public void LoadBossLife(GameController.GameState newState)
    {
        if(newState == GameController.GameState.Store)
            bossLife = PlayerPrefs.GetInt("BossLife");
        else
        {
            PlayerPrefs.SetInt("BossLife", (int)bossLife);
            gameObject.SetActive(false);
        }
            
        //actualSprite.sprite = CheckSprite();
    }
    public void ReceiveDamage(int damage)
    {
        bossLife -= damage;
        actualSprite.sprite = CheckSprite();
        StartCoroutine(EffectDamage());
        GameView.Instance.SetHealth(bossLife, maxBossLife);
        //Colocar a DISGRAAAAAAAAAAAAAAAAAAAAAAÇA AQUI

    }
    public IEnumerator EffectDamage()
    {
        actualSprite.material = blinkMaterial;
        yield return new WaitForSeconds(0.2f);
        actualSprite.material = previousMaterial;

    }
    private Sprite CheckSprite()
    {
        float healthPercentage = bossLife / maxBossLife;
        healthPercentage *= 100;

        if (healthPercentage > 80f)
        {
            return spritesDoBoss[0]; // 80%
        }
        else if (healthPercentage > 60f)
        {
            return spritesDoBoss[1]; //  60% e 80%
        }
        else if (healthPercentage > 40f)
        {
            return spritesDoBoss[2]; //  40% e 60%
        }
        else if (healthPercentage > 20f)
        {
            return spritesDoBoss[3]; // 20% e 40%
        }
        else
        {
            return spritesDoBoss[4]; // Sprite para vida abaixo de 20%
        }
        //Verificar Dano
    }
}
