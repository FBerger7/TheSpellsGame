﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class SpellController : MouseTracker
{
    public KeyCode leftSpellKey;
    public KeyCode rightSpellKey;
    public KeyCode deffensiveSpellKey;
    public KeyCode healSpellKey;
    public KeyCode mobileSpellKey;
    public KeyCode changeOffensiveSpellKey;
    public KeyCode changeMobileSpellKey;
    public CharacterInterface characterInterface;

    private OffensiveSpellsModel _model;
    private PlayerAnimation _playerAnimation;
    private Animator _anim;
    private List<Tuple<int, int>> _spellQueue;
    private Dictionary<int, BaseSpell> _offensiveSpells;
    private Dictionary<int, BaseSpell> _mobileSpells;
    private SummonPowerShield _summonPowerShield;
    private UseHeal _useHeal;
    private int _currentOffensiveSpellsPair = 0;
    private int _currentMobileSpell = 0;
    private float shield_lifespan;
    private float current_shield_lifespan;
    private bool is_shield = false;

    // Start is called before the first frame update
    void Start()
    {
        SetCamera();
        _spellQueue = new List<Tuple<int, int>>();
        _model = new OffensiveSpellsModel();
        _playerAnimation = new PlayerAnimation();
        _anim = gameObject.GetComponentInParent<Animator>();

        _useHeal = gameObject.GetComponent<UseHeal>();
        _summonPowerShield = gameObject.GetComponent<SummonPowerShield>();
        shield_lifespan = _summonPowerShield.shieldLifeSpan;
        current_shield_lifespan = _summonPowerShield.shieldLifeSpan;
        AddOffensiveSpells();
        AddMobileSpells();
        // inicjalizacja listy spelli tylko do testów, potem tego nie będzie
        // ------------------------------------------------------------------
        _spellQueue.Add(new Tuple<int, int>(OffensiveSpellsModel.PILLARRISE, OffensiveSpellsModel.FIRE_BREATH));
        _spellQueue.Add(new Tuple<int, int>(OffensiveSpellsModel.SLIME_BOMB, OffensiveSpellsModel.BASIC_SPELL));
        // ------------------------------------------------------------------
    }

    // Update is called once per frame
    void Update()
    {
        TrackMouse();

        //Debug.Log(pointToLook);

        HandleSpellChange();

        HandleOffensiveSpells();

        HandleDeffensiveSpell();

        HandleHealSpell();

        HandleMobileSpell();

        if(is_shield)
        {
            current_shield_lifespan -= Time.deltaTime;
            if(current_shield_lifespan <= 0)
            {
                current_shield_lifespan = shield_lifespan;
                is_shield = false;
                _playerAnimation.IdleAnimation(ref _anim);
            }
        }

        if (_anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f && _anim.GetCurrentAnimatorStateInfo(0).IsName("BasicAttack"))
            _playerAnimation.IdleAnimation(ref _anim);

        if (_anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f && _anim.GetCurrentAnimatorStateInfo(0).IsName("GoToParyAttack"))
            _playerAnimation.ParyAttack(ref _anim);
    }

    private void HandleSpellChange()
    {
        if (Input.GetKeyDown(changeOffensiveSpellKey))
        {
            _currentOffensiveSpellsPair = (_currentOffensiveSpellsPair + 1) % _spellQueue.Count;
            characterInterface.SetMainSpellFirst(_spellQueue[_currentOffensiveSpellsPair].Item1);
            characterInterface.SetMainSpellSecond(_spellQueue[_currentOffensiveSpellsPair].Item2);
        }

        if (Input.GetKeyDown(changeMobileSpellKey))
        {
            _currentMobileSpell = (_currentMobileSpell + 1) % _mobileSpells.Count;
            characterInterface.SetMobileSpell(_currentMobileSpell);
        }
    }

    private void HandleOffensiveSpells()
    {
        if (Input.GetKey(leftSpellKey))
        {
            _offensiveSpells[_spellQueue[_currentOffensiveSpellsPair].Item1].PerformAttack(pointToLook, false);
            _playerAnimation.AttackAnimation(ref _anim);
            Debug.Log("player attacked");
        }
        if (Input.GetKeyUp(leftSpellKey))
        {
            _offensiveSpells[_spellQueue[_currentOffensiveSpellsPair].Item1].EndAttack();
        }

        if (Input.GetKey(rightSpellKey))
        {
            _offensiveSpells[_spellQueue[_currentOffensiveSpellsPair].Item2].PerformAttack(pointToLook, false);
        }
        if (Input.GetKeyUp(leftSpellKey))
        {
            _offensiveSpells[_spellQueue[_currentOffensiveSpellsPair].Item2].EndAttack();
        }
    }

    private void HandleDeffensiveSpell()
    {
        if (Input.GetKey(deffensiveSpellKey))
        {
            is_shield = true;
            _summonPowerShield.PerformAttack(_model);
            _playerAnimation.ParyAttackTransition(ref _anim);
        }
    }

    private void HandleHealSpell()
    {
        if (Input.GetKey(healSpellKey))
        {
            _useHeal.PerformAttack(pointToLook,false);
            _playerAnimation.AttackAnimation(ref _anim);
        }
    }

    private void HandleMobileSpell()
    {
        if (Input.GetKey(mobileSpellKey))
        {
            _mobileSpells[_currentMobileSpell].PerformAttack(pointToLook, false);
        }

        if (Input.GetKeyUp(mobileSpellKey))
        {
            _mobileSpells[_currentMobileSpell].EndAttack();
        }
    }

    private void AddOffensiveSpells()
    {

        _offensiveSpells = new Dictionary<int, BaseSpell>();
        _offensiveSpells.Add(OffensiveSpellsModel.SLIME_BOMB, gameObject.GetComponent<SlimeBomb>());
        _offensiveSpells.Add(OffensiveSpellsModel.BASIC_SPELL, gameObject.GetComponent<BasicAttack>());
        _offensiveSpells.Add(OffensiveSpellsModel.SUMMON_WALL, gameObject.GetComponent<SummonWall>());
        _offensiveSpells.Add(OffensiveSpellsModel.FIRE_BREATH, gameObject.GetComponent<FireBreath>());
        _offensiveSpells.Add(OffensiveSpellsModel.RAILGUN, gameObject.GetComponent<Railgun>());
        _offensiveSpells.Add(OffensiveSpellsModel.PILLARRISE, gameObject.GetComponent<PillarRise>());
    }

    private void AddMobileSpells()
    {
        _mobileSpells = new Dictionary<int, BaseSpell>();
        _mobileSpells.Add(MobileSpellsModel.JUMP, gameObject.GetComponent<Jump>());
        _mobileSpells.Add(MobileSpellsModel.RUN, gameObject.GetComponent<Run>());
        _mobileSpells.Add(MobileSpellsModel.TELEPORT, gameObject.GetComponent<Teleport>());
    }
}
