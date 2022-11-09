using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTurnCardGameState : CardGameState
{
    [SerializeField] Text _playerTurnTextUI = null;
    [SerializeField] Text _winState = null;
    [SerializeField] Text _loseState = null;
    [SerializeField] Canvas _playerMenu = null;
    public EnemyTurnCardGameState _enemy;
    public int _health;
    public int _magic;
    float _effect;

    int _playerTurnCount = 0;

    public bool _defendActive = false;
    public bool _bideActive = true;
    public bool _dmgReduceActive = false;
    public bool _enemyDefendEnabled = true;

    private void Start()
    {
        _winState.gameObject.SetActive(false);
        _loseState.gameObject.SetActive(false);
        _effect = Random.Range(0, 1);
    }

    public override void Enter()
    {
        Debug.Log("Player health: " + _health);
        Debug.Log("Player Mana: " + _magic);
        _defendActive = false;
        if (_health > 0)
        {
            _playerTurnTextUI.gameObject.SetActive(true);
            _playerMenu.gameObject.SetActive(true);

            _playerTurnCount++;
            _playerTurnTextUI.text = "Player Turn: " + _playerTurnCount.ToString();
            StateMachine.Input.PressedConfirm += OnPressedConfirm;
        }
        else
            _loseState.gameObject.SetActive(true);
    }

    public override void Exit()
    {
        _playerTurnTextUI.gameObject.SetActive(false);
        _playerMenu.gameObject.SetActive(false);
        StateMachine.Input.PressedConfirm -= OnPressedConfirm;

    }

    void OnPressedConfirm()
    {
        StateMachine.ChangeState<EnemyTurnCardGameState>();
    }

    public void Attack()
    {
        _enemy._health -= 2;
        Debug.Log("You attacked!");
        Debug.Log("Enemy health: " + _enemy._health);
    }

    public void Defend()
    {
        _defendActive = true;
        Debug.Log("You are defending!");
    }

    public void Heal()
    {
        if (_magic >= 10)
        {
            _health += 5;
            _magic -= 10;
            Debug.Log("You have successfully healed! You feel invigorated...");
        }
        else if (_magic < 10)
        {
            Debug.Log("Uh oh! You didn't have enough mana, and your spell failed...");
        }
    }

    public void CastEffect()
    {

        if (_magic >= 5)
        {
            Debug.Log("You summon up your power, and cast...");
            if (_bideActive == true && _dmgReduceActive == true)
            {
                _dmgReduceActive = true;
                _magic -= 5;
                Debug.Log("... Nothing! You've already cast all your spells! Oops...");
            }
            else if (_bideActive == true && _dmgReduceActive == false && _effect == 0)
            {
                _bideActive = false;
                Debug.Log("... Disable bide! Your enemy can no longer bide.");
                _magic -= 5;
                _effect = 1;
            }
            else if (_bideActive == true && _dmgReduceActive == false && _effect == 1)
            {
                _dmgReduceActive = true;
                Debug.Log("... Weaken enemy! Your enemy's attacks are now weaker.");

                _effect = 0;
            }
        }
        else if (_magic <= 5)
        {
            Debug.Log("Uh oh! You didn't have enough mana, and your spell failed...");
        }
    }
}
