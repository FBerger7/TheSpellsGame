﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class SpellController : MouseTracker
{
    private const float SPELL_CHANGE_COOLDOWN = 0.5f;

    public KeyCode leftSpellKey;
    public KeyCode rightSpellKey;
    public KeyCode deffensiveSpellKey;
    public KeyCode mobileSpellKey;
    public KeyCode changeSpellKey;

    private OffensiveSpellsModel model;
    private List<Tuple<int, int>> _spellQueue;
    private Dictionary<int, BaseSpell> _offensiveSpells;
    private Dictionary<int, BaseSpell> _mobileSpells;
    private SummonPowerShield _summonPowerShield;
    private int _currentOffensiveSpellsPair = 0;
    public int _currentMobileSpell = 0;
    private float _remainingPairChangeCooldown = SPELL_CHANGE_COOLDOWN;
    

    // Start is called before the first frame update
    void Start()
    {
        SetCamera();
        _spellQueue = new List<Tuple<int, int>>();
        model = new OffensiveSpellsModel();

        _summonPowerShield = gameObject.GetComponent<SummonPowerShield>();
        AddOffensiveSpells();
        AddMobileSpells();
        // inicjalizacja listy spelli tylko do testów, potem tego nie będzie
        // ------------------------------------------------------------------
        _spellQueue.Add(new Tuple<int, int>(OffensiveSpellsModel.SLIME_BOMB, OffensiveSpellsModel.FIRE_BREATH));
        _spellQueue.Add(new Tuple<int, int>(OffensiveSpellsModel.FIRE_BREATH, OffensiveSpellsModel.BASIC_SPELL));
        _spellQueue.Add(new Tuple<int, int>(OffensiveSpellsModel.BASIC_SPELL, OffensiveSpellsModel.SUMMON_WALL));
        _spellQueue.Add(new Tuple<int, int>(OffensiveSpellsModel.SUMMON_WALL, OffensiveSpellsModel.BASIC_SPELL));
        // ------------------------------------------------------------------
    }

    // Update is called once per frame
    void Update()
    {
        TrackMouse();
        _remainingPairChangeCooldown -= Time.deltaTime;

        if (Input.GetKey(changeSpellKey))
        {
            if (_remainingPairChangeCooldown <= 0)
            {
                _remainingPairChangeCooldown = SPELL_CHANGE_COOLDOWN;
                _currentOffensiveSpellsPair = (_currentOffensiveSpellsPair + 1) % _spellQueue.Count;
            }
        }

        if (Input.GetKey(leftSpellKey))
        {
            _offensiveSpells[_spellQueue[_currentOffensiveSpellsPair].Item1].PerformAttack(pointToLook);
        }
        if (Input.GetKeyUp(leftSpellKey))
        {
            _offensiveSpells[_spellQueue[_currentOffensiveSpellsPair].Item1].EndAttack();
        }

        if (Input.GetKey(rightSpellKey))
        {
            _offensiveSpells[_spellQueue[_currentOffensiveSpellsPair].Item2].PerformAttack(pointToLook);
        }
        if (Input.GetKeyUp(leftSpellKey))
        {
            _offensiveSpells[_spellQueue[_currentOffensiveSpellsPair].Item2].EndAttack();
        }

        if (Input.GetKey(deffensiveSpellKey))
        {
            _summonPowerShield.PerformAttack(model);
        }
        if (Input.GetKey(mobileSpellKey))
        {
            _mobileSpells[_currentMobileSpell].PerformAttack(pointToLook);
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
    }

    private void AddMobileSpells()
    {
        _mobileSpells = new Dictionary<int, BaseSpell>();
        _mobileSpells.Add(MobileSpellsModel.JUMP, gameObject.GetComponent<Jump>());
        _mobileSpells.Add(MobileSpellsModel.RUN, gameObject.GetComponent<Run>());
        _mobileSpells.Add(MobileSpellsModel.TELEPORT, gameObject.GetComponent<Teleport>());
    }
}
