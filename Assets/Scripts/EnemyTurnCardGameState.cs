using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class EnemyTurnCardGameState : CardGameState
{
    [SerializeField] Text _enemyTurnTextUI = null;
    public static event Action EnemyTurnBegan;
    public static event Action EnemyTurnEnded;
    public PlayerTurnCardGameState _player;
    [SerializeField] Canvas _enemyMenu = null;
    [SerializeField] GameObject _playerArt;
    [SerializeField] GameObject _bideCover;
    public Text _biding;
    public Text _bideFailed;

    [SerializeField] float _pauseDuration = 1.5f;

    public int _health;
    public int _magic;

    public bool _bideInProgress;
    public bool _cantBide = false;
    public bool _defendActive = true;

    [SerializeField] AudioSource giggle;
    [SerializeField] AudioSource damage;
    [SerializeField] AudioSource bideWeakened;
    [SerializeField] AudioSource bideFailed;
    [SerializeField] AudioSource win;

    private void Start()
    {
        _biding.gameObject.SetActive(false);
        _bideFailed.gameObject.SetActive(false);
        _bideCover.gameObject.SetActive(false);
    }

    public override void Enter()
    {
        Debug.Log("Enemy health: " + _health);

        if(_health >= 0)
        {
            EnemyTurnBegan?.Invoke();
            StartCoroutine(EnemyThinkingRoutine(_pauseDuration));
        }
        else if (_health <= 0)
        {
            win.Play();
            _player._playerMenu.gameObject.SetActive(false);
            _player._winState.gameObject.SetActive(true);
        }
    }

    IEnumerator EnemyThinkingRoutine(float pauseDuration)
    {
        _enemyTurnTextUI.gameObject.SetActive(false);
        yield return new WaitForSeconds(pauseDuration);
        _enemyTurnTextUI.gameObject.SetActive(true);
        Attack();

        EnemyTurnEnded?.Invoke();
        StateMachine.ChangeState<PlayerTurnCardGameState>();
    }

    public void Attack()
    {
        if (_health > _health * .25 && _bideInProgress == true && _player._bideActive == true && _player._defendActive == false)
        {
            _bideInProgress = false;
            _player._health -= 4;
            _biding.gameObject.SetActive(false);
            _bideCover.gameObject.SetActive(false);
            Debug.Log("Enemy unleashes bide!");
            _enemyTurnTextUI.text = "How do you like that!";
        }
        if (_bideInProgress == true && _player._bideActive == true && _player._defendActive == true)
        {
            bideWeakened.Play();
            _bideInProgress = false;
            _player._health -= 2;
            _biding.gameObject.SetActive(false);
            _bideCover.gameObject.SetActive(false);
            Debug.Log("Enemy unleashes bide! Your high defense reduced the damage...");
            _enemyTurnTextUI.text = "How do you like... hey, no fair! [Your high defense reduced the damage.]";
        }
        else if (_health > _health*.25 && _player._bideActive == true && _bideInProgress == false)
        {
            _bideInProgress = true;
            _biding.gameObject.SetActive(true);
            _bideCover.gameObject.SetActive(true);
            Debug.Log("Enemy is biding...");
            _enemyTurnTextUI.text = "[Your enemy is biding.]";
        }
        else if (_health > _health * .25 && _player._dmgReduceActive == true && _player._bideActive == true && _bideInProgress == false)
        {
            _biding.gameObject.SetActive(true);
            _bideCover.gameObject.SetActive(true);
            _bideInProgress = true;
            Debug.Log("Enemy is biding...");
            _enemyTurnTextUI.text = "[Your enemy is biding.]";
        }
        else if (_health > _health * .25 && _player._dmgReduceActive == false && _player._bideActive == true && _bideInProgress == false)
        {
            _biding.gameObject.SetActive(true);
            _bideCover.gameObject.SetActive(true);
            _bideInProgress = true;
            Debug.Log("Enemy is biding...");
            _enemyTurnTextUI.text = "[Your enemy is biding.]";
        }
        else if (_health > _health * .25 && _player._bideActive == false && _bideInProgress == true && _cantBide == false)
        {
            bideFailed.Play();
            _bideInProgress = false;
            _biding.gameObject.SetActive(false);
            Debug.Log("Enemy is biding...");
            _enemyTurnTextUI.text = "Hey... Why can't I... " +
                "What did you do?!";
            _bideCover.gameObject.SetActive(false);
        }
        else if (_player._dmgReduceActive == false && _player._bideActive == false && _bideInProgress == false && _player._defendActive == false)
        {
            _player._health -= 2;
            Debug.Log("Enemy attacks!");
            _enemyTurnTextUI.text = "Take that!";
            giggle.Play();
        }
        else if (_player._dmgReduceActive == false && _player._bideActive == false && _bideInProgress == false && _player._defendActive == true)
        {
            _player._health -= 2;
            Debug.Log("Enemy attacks! Your high defense reduced the damage...");
            _enemyTurnTextUI.text = "Put your guard down and fight me!!";
        }
        else if (_player._dmgReduceActive == true && _player._bideActive == false && _bideInProgress == false && _player._defendActive == false)
        {
            _player._health -= 1;
            Debug.Log("Enemy attacks! But it seems weak...");
        }
        else if (_player._dmgReduceActive == true && _player._bideActive == false && _bideInProgress == false && _player._defendActive == true)
        {
            Debug.Log("Enemy attacks! But it seems weak, and your defense was too high to be affected...");
        }
        else if (_health < _health * .25 && _defendActive == false && _player._enemyDefendEnabled == true)
        {
            _defendActive = true;
            Debug.Log("Enemy is defending...");
            _enemyTurnTextUI.text = "Man... [Your enemy is defending.]";
        }

    }
}
