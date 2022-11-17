using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    private const float _maxHealth = 10;
    public PlayerTurnCardGameState _player;
    public float _health;
    public Image healthBar;

    private void Awake()
    {
        _health = _player._health;
    }

    private void Update()
    {
        _health = _player._health;
        healthBar.fillAmount = _health / _maxHealth;
    }
}