using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    private const float _maxHealth = 10;
    public EnemyTurnCardGameState _enemy;
    public float _health;
    public Image healthBar;

    private void Awake()
    {
        _health = _enemy._health;
    }

    private void Update()
    {
        _health = _enemy._health;
        healthBar.fillAmount = _health / _maxHealth;
    }
}
