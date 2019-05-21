﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class GloriosaController : EnemyController
{
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 200f;

        _agent = GetComponent<NavMeshAgent>();
        lookRadius = _agent.stoppingDistance = 50f;

        _anim = GetComponent<Animator>();
        _target = PlayerManager.instance.player.transform; //RangeAttribute of Gloriosa

        _attack = gameObject.GetComponentInChildren<Railgun>();
        _enemyAnimation = new GloriosaAnimation();
    }

    // Update is called once per frame
    void Update()
    {
        RunUpdate();
    }

    public override void Die()
    {
        _enemyAnimation.DieAnimation(ref _anim);
        Debug.Log(transform.name + " died");
        // Destroy(gameObject);
    }
}

