﻿using UnityEngine;

[System.Serializable]
public class CharacterStats : MonoBehaviour
{
   // public float myDamage;

    public float maxHealth;

    public float CurrentHealth { get; private set; }

    public bool isHostile;
    public bool poisonImmune;
    public float poisonCooldown;
    public CharacterInterface characterInterface;
    private PlayerAnimation _playerAnimation;
    private Animator _anim;

    [SerializeField]
    protected float _poisonTimer;

    private void Awake()
    {
        CurrentHealth = maxHealth;
        _playerAnimation = new PlayerAnimation();
        _anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        _poisonTimer -= Time.deltaTime;
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        Debug.Log(transform.name + " takes " + damage + " damage.");

        if (characterInterface)
            characterInterface.SetHealthPoints((int)CurrentHealth);

        if (CurrentHealth <= 0)
        {
            Die();
        }

    }

    public void ApplayPoison(float dmg)
    {
        if (_poisonTimer < 0)
        {
            TakeDamage(dmg);
            _poisonTimer = poisonCooldown;
        }
    }

    public virtual void Die()
    {
        //This method is meant to be overwrriten
        if(transform.name == "Player")
            _playerAnimation.DieAnimation(ref _anim);
        Debug.Log(transform.name + " died");

    }

}
