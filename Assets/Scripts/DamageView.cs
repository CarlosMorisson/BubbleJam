using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageView : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    TextMeshProUGUI m_Text;
    private string text;
    private int cont;

    public static DamageView Instance;

    private void Awake()
    {
        Instance = this;
        
    }
    private void OnEnable()
    {
        DamageController.OnTakeDamage += UpdateText;
        GameController.OnGameStateChanged += UpdateText;
    }
    private void OnDisable()
    {
        DamageController.OnTakeDamage -= UpdateText;
        GameController.OnGameStateChanged -= UpdateText;
    }
    private void Start()
    {
        cont = 0;
    }

    public void UpdateText()
    {
        cont=BubbleController.Instance.GetActiveBubbleCount();
        m_Text.text = cont.ToString();
    }
    public void UpdateText(GameController.GameState state)
    {
        if (state == GameController.GameState.Game)
            UpdateText();
    }




}
