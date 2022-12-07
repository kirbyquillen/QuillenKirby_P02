using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTurnCardGameState : CardGameState
{
    [SerializeField] Text _playerTurnTextUI = null;
    [SerializeField] public Text _winState = null;
    [SerializeField] Text _loseState = null;
    [SerializeField] public Canvas _playerMenu = null;
    [SerializeField] GameObject _enemyArt;
    public EnemyTurnCardGameState _enemy;
    public int _health;
    public int _maxHealth = 10;
    public int _magic;
    float _effect;

    int _playerTurnCount = 0;

    public bool _defendActive = false;
    public bool _bideActive = true;
    public bool _dmgReduceActive = false;
    public bool _enemyDefendEnabled = true;

    [SerializeField] AudioSource lose;

    private void Start()
    {
        _winState.gameObject.SetActive(false);
        _loseState.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
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
            StateMachine.Input.PressedConfirm += OnPressedConfirm;
        }
        else
        {
            lose.Play();
            _loseState.gameObject.SetActive(true);
        }
    }

    public override void Exit()
    {
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
        Debug.Log("You attacked! What now?");
        _playerTurnTextUI.text = "You attacked!";
        Debug.Log("Enemy health: " + _enemy._health);
        StateMachine.ChangeState<EnemyTurnCardGameState>();
    }

    public void Defend()
    {
        _defendActive = true;
        Debug.Log("You are defending!");
        _playerTurnTextUI.text = "You defended yourself!";
        StateMachine.ChangeState<EnemyTurnCardGameState>();
    }

    public void Heal()
    {
        if (_magic >= 10)
        {
            _health += 5;
            _magic -= 10;
            Debug.Log("You have successfully healed! You feel invigorated...");
            _playerTurnTextUI.text = "You successfully healed! You feel invigorated...";
        }
        else if (_magic < 10)
        {
            Debug.Log("Uh oh! You didn't have enough mana, and your spell failed...");
            _playerTurnTextUI.text = "Uh oh! You didn't have enough mana, and your spell failed...";
        }
        StateMachine.ChangeState<EnemyTurnCardGameState>();
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
            else if (_bideActive == true && _health >= _health *.5)
            {
                _bideActive = false;
                _enemy._biding.gameObject.SetActive(false);
                Debug.Log("... Disable bide! Your enemy can no longer bide.");
                _playerTurnTextUI.text = "You disabled enemy bide!";
                if (_enemy._bideInProgress == true)
                    Debug.Log("Enemy bide failed!");
                _playerTurnTextUI.text = "You disabled enemy bide! Enemy bide failed!";
                _magic -= 5;
                _effect = 1;
            }
            else if (_dmgReduceActive == false && _health <= _health*.5)
            {
                _dmgReduceActive = true;
                Debug.Log("... Weaken enemy! Your enemy's attacks are now weaker.");
                _magic -= 5;
                _effect = 0;
            }
        }
        else if (_magic <= 5)
        {
            Debug.Log("Uh oh! You didn't have enough mana, and your spell failed...");
            _playerTurnTextUI.text = "Uh oh! You didn't have enough mana, and your spell failed...";
        }
        StateMachine.ChangeState<EnemyTurnCardGameState>();
    }
}
