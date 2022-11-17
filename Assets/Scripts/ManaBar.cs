using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    private const float _maxHealth = 20;
    public PlayerTurnCardGameState _player;
    public float _health;
    public Image healthBar;

    private void Awake()
    {
        _health = _player._magic;
    }

    private void Update()
    {
        _health = _player._magic;
        healthBar.fillAmount = _health / _maxHealth;
    }
}