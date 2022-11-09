using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class EnemyTurnCardGameState : CardGameState
{
    public static event Action EnemyTurnBegan;
    public static event Action EnemyTurnEnded;
    public PlayerTurnCardGameState _player;
    public Text _biding;
    public Text _bideFailed;

    [SerializeField] float _pauseDuration = 1.5f;

    public int _health;
    public int _magic;

    bool _bideInProgress;
    public bool _defendActive = true;

    private void Start()
    {
        _biding.gameObject.SetActive(false);
        _bideFailed.gameObject.SetActive(false);
    }

    public override void Enter()
    {
        Debug.Log("Enemy health: " + _health);
        EnemyTurnBegan?.Invoke();

        StartCoroutine(EnemyThinkingRoutine(_pauseDuration));
    }

    IEnumerator EnemyThinkingRoutine(float pauseDuration)
    {
        yield return new WaitForSeconds(pauseDuration);
        Attack();

        EnemyTurnEnded?.Invoke();
        StateMachine.ChangeState<PlayerTurnCardGameState>();
    }

    public void Attack()
    {
        if (_bideInProgress == true && _player._bideActive == true && _player._defendActive == false)
        {
            _player._health -= 4;
            _bideInProgress = false;
            _biding.gameObject.SetActive(false);
            Debug.Log("Enemy unleashes bide!");
        }
        if (_bideInProgress == true && _player._bideActive == true && _player._defendActive == true)
        {
            _player._health -= 2;
            _bideInProgress = false;
            _biding.gameObject.SetActive(false);
            Debug.Log("Enemy unleashes bide! Your high defense reduced the damage...");
        }
        else if (_health > _health*.25 && _player._bideActive == true)
        {
            _biding.gameObject.SetActive(true);
            _bideInProgress = true;
            Debug.Log("Enemy is biding...");
        }
        else if (_health > _health * .25 && _player._dmgReduceActive == true && _player._bideActive == true && _bideInProgress == false)
        {
            _biding.gameObject.SetActive(true);
            _bideInProgress = true;
            Debug.Log("Enemy is biding...");
        }
        else if (_health > _health * .25 && _player._dmgReduceActive == false && _player._bideActive == true && _bideInProgress == false)
        {
            _biding.gameObject.SetActive(true);
            _bideInProgress = true;
            Debug.Log("Enemy is biding...");
        }
        else if (_health > _health * .25 && _player._dmgReduceActive == false && _player._bideActive == false && _bideInProgress == false && _player._defendActive == false)
        {
            _player._health -= 2;
            Debug.Log("Enemy attacks!");
        }
        else if (_health > _health * .25 && _player._dmgReduceActive == false && _player._bideActive == false && _bideInProgress == false && _player._defendActive == true)
        {
            _player._health -= 2;
            Debug.Log("Enemy attacks! Your high defense reduced the damage...");
        }
        else if (_health > _health * .25 && _player._dmgReduceActive == true && _player._bideActive == false && _bideInProgress == false && _player._defendActive == false)
        {
            _player._health -= 1;
            Debug.Log("Enemy attacks! But it seems weak...");
        }
        else if (_health > _health * .25 && _player._dmgReduceActive == true && _player._bideActive == false && _bideInProgress == false && _player._defendActive == true)
        {
            Debug.Log("Enemy attacks! But it seems weak, and your defense was too high to be affected...");
        }
        else if (_health < _health * .25 && _defendActive == false && _player._enemyDefendEnabled == true)
        {
            _defendActive = true;
            Debug.Log("Enemy is defending...");
        }
        else if (_health < _health * .25 && _defendActive == false && _player._enemyDefendEnabled == false && _player._dmgReduceActive == false && _player._defendActive == false)
        {
            _player._health -= 2;
            Debug.Log("Enemy attacks!");
        }
        else if (_health < _health * .25 && _defendActive == false && _player._enemyDefendEnabled == false && _player._dmgReduceActive == false && _player._defendActive == true)
        {
            _player._health -= 2;
            Debug.Log("Enemy attacks! Your high defense reduced the damage...");
        }
        else if (_health < _health * .25 && _defendActive == false && _player._enemyDefendEnabled == false && _player._dmgReduceActive == true && _player._defendActive == false)
        {
            _player._health -= 1;
            Debug.Log("Enemy attacks! But it seems weak...");
        }
        else if (_health < _health * .25 && _defendActive == false && _player._enemyDefendEnabled == false && _player._dmgReduceActive == true && _player._defendActive == true)
        {
            _player._health -= 1;
            Debug.Log("Enemy attacks! But it seems weak, and your defense was too high to be affected...");
        }

    }
}
