using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    /* Stat */
    [HideInInspector] public float _health;
    [HideInInspector] public int _attackPower;
    [HideInInspector] public float _attackDistance;
    [HideInInspector] public float _attackCoolTime;
    [HideInInspector] public float _skillAttckDistance;
    [HideInInspector] public float _skillAttckCoolTime;

    /* Values*/
    [HideInInspector] public int id;
    [HideInInspector] public string job;
    [HideInInspector] public int level = 1;
    [HideInInspector] public float exp = 0;
    [HideInInspector] public int _hpUpgradeLevel = 0;
    [HideInInspector] public int _attackUpgradeLevel = 0;
    [HideInInspector] public bool isAlive = true;
    [HideInInspector] public Animator animator;
    [HideInInspector] public float moveSpeed = 1f;
    [HideInInspector] public int attackSequence = 0;
    [HideInInspector] public CurrentCharacterState currentState = CurrentCharacterState.Idle;
    [HideInInspector] public CharacterInfoPanel _characterInfoPanel;
    [HideInInspector] public Monster targetMonster = null;
    private float currentAttackCooldownTime = 0f; 
    private bool isAttackOnCooldown = false;
    private float currentSkillAttackCooldownTime = 0f; 
    private bool isSkillAttackOnCooldown = false;
    private GameObject targetMonsterObj = null;
    private Coroutine responeCoroutine;

    /// ///////////////////////////////////////////////////////////////////////
    /// virtual

    public virtual void Init(){}
    public virtual void Reset(){
        targetMonsterObj = null;
        targetMonster = null;
        _characterInfoPanel.RestoreHPAll();
        _characterInfoPanel.ChangeLVText(level);

        // set animation
        ChangeState(CurrentCharacterState.Idle);

        animator.Play("idle", 0, 0f);
        isAlive = true;
    }
    public virtual void EachSkillAttack(){}
    public virtual void Upgrade(){}
    public virtual float GetUpgradeHP(){ return 0f; }
    public virtual float GetUpgradeAttack(){ return 0f;}

    /// ///////////////////////////////////////////////////////////////////////
    /// main

    public void Update(){
        // if alive and stage start
        if(isAlive && GameManager.Instance.isStageStart)
        {
            // double check
            if(targetMonsterObj == null){
                targetMonster = null;
            }
            
            // If no target monster, or if it's dead
            if(targetMonster == null || targetMonster.isAlive == false){
                // find target monster
                TrackMonster();
            }else{
                // check attack distance inside
                if(Vector3.Distance(transform.position, targetMonsterObj.transform.position) < _attackDistance){
                    Attack();
                    // check can skill attack 
                    if(Vector3.Distance(transform.position, targetMonsterObj.transform.position) < _skillAttckDistance){
                        SkillAttack();
                    }
                }else{
                    // outside => move
                    Move();
                }
            }

            UpdateCooldownTimer();
        }

        if(isAlive && GameManager.Instance.isStageStart == false){
            // set animation
            ChangeState(CurrentCharacterState.Idle);
        }
    }

    /// ///////////////////////////////////////////////////////////////////////
    /// function
    
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
    
    public void TakeDamage(float damageAmount){
        _characterInfoPanel.DamgeHP(damageAmount);
        _health -= damageAmount;
        if(_health < 0){
            _health = 0;
            Die();
        }
    }

    public void RestoreHP(float restoreHP){
        // check not over hp restore
        restoreHP = (_health + restoreHP <= GetUpgradeHP()) ? restoreHP : GetUpgradeHP() - _health;
        _health += restoreHP;
        _characterInfoPanel.RestoreHP(restoreHP);
        //Debug.Log("priest heal " + job + " " + restoreHP);
    }

    public void UpgradeHPLevel(int num)
    {
        _hpUpgradeLevel = num;
        // save data
        SaveLoadManager.Instance.SetCharacterInfoHpUpgradeLevel(id, _hpUpgradeLevel);
    }

    public void UpgradeAttackLevel(int num)
    {
        _attackUpgradeLevel = num;
        // save data
        SaveLoadManager.Instance.SetCharacterInfoAttackUpgradeLevel(id, _attackUpgradeLevel);
    }

    public void GetEXP(float value){
        exp += value;
        _characterInfoPanel.RestoreEXP(value);
        CheckLevelUp();
    }

    public void ChangeState(CurrentCharacterState newState)
    {
        if(currentState == newState) return;
        if(animator == null) return;

        currentState = newState;

        switch (currentState)
        {
            case CurrentCharacterState.Move:
                // move animation
                animator.SetBool("isAttack", false);
                animator.SetBool("isWalk", true);
                break;
            case CurrentCharacterState.Attack:
                // attack animation
                animator.SetBool("isAttack", true);
                animator.SetBool("isWalk", false);
                break;
            case CurrentCharacterState.Die:
                // dead animation
                animator.SetBool("isAttack", false);
                animator.SetBool("isWalk", false);
                animator.SetBool("isDie", true);
                break;
            default:
                // idle animation
                animator.SetBool("isWalk", false);
                animator.SetBool("isAttack", false);
                animator.SetBool("isDie", false);
                break;
        }
    }
    
    /// ///////////////////////////////////////////////////////////////////////
    /// about attack
    
    private void Attack(){
        // set animation
        ChangeState(CurrentCharacterState.Attack);

        // check the cool time is over
        if(isAttackOnCooldown == false){
            if(targetMonster == null) return;

            // Set Attack Direction Image
            Vector2 direction = targetMonsterObj.transform.position - transform.position;
            direction.Normalize();
            ChangeImageDirection(direction);

            // damge monster
            targetMonster.TakeDamage(this, _attackPower);

            // play sfx
            GameSoundManager.Instance.SfxPlay(SFX.BasicAttack);

            // check monster is dead
            if(targetMonster.isAlive == false){
                targetMonster = null;
            }

            isAttackOnCooldown = true;
        }
    }

    private void SkillAttack(){
        // set animation
        ChangeState(CurrentCharacterState.Attack);

        if(isSkillAttackOnCooldown == false){
            if(targetMonster == null) return;

            EachSkillAttack();

            // check monster is dead
            if(targetMonster.isAlive == false){
                targetMonster = null;
            }

            isSkillAttackOnCooldown = true;
        }
    }

    private void CheckLevelUp(){
        if(exp >= 100){
            level++;
            exp = 0;
            _characterInfoPanel.DamgeEXP(100);
            _characterInfoPanel.ChangeLVText(level);
            // save data
            SaveLoadManager.Instance.SetCharacterInfoLevel(id, level);
            // play sfx
            GameSoundManager.Instance.SfxPlay(SFX.LevelUp);
        }
    }

    private void UpdateCooldownTimer()
    {
        if (isAttackOnCooldown)
        {
            currentAttackCooldownTime += Time.deltaTime;
            if (currentAttackCooldownTime >= _attackCoolTime)
            {
                isAttackOnCooldown = false;
                currentAttackCooldownTime = 0f;
            }
        }

        if (isSkillAttackOnCooldown)
        {
            currentSkillAttackCooldownTime += Time.deltaTime;
            if (currentSkillAttackCooldownTime >= _skillAttckCoolTime)
            {
                isSkillAttackOnCooldown = false;
                currentSkillAttackCooldownTime = 0f;
            }
        }
    }

    /// ///////////////////////////////////////////////////////////////////////
    /// track and move

    private void TrackMonster()
    {
        // set animation
        ChangeState(CurrentCharacterState.Idle);

        // Tracking the nearest monster
        if(GameManager.Instance.monstersController.spawnedMonsters.Count > 0){
            Monster closestMonster = null;
            float closestDistance = float.MaxValue;

            foreach (var monster in GameManager.Instance.monstersController.spawnedMonsters)
            {
                if(monster.isAlive){
                    float distanceToMonster = Vector3.Distance(transform.position, monster.transform.position);
                    if (distanceToMonster < closestDistance)
                    {
                        // setting target monster
                        closestMonster = monster;
                        closestDistance = distanceToMonster;
                    }
                }
            }

            if (closestMonster != null)
            {
                targetMonster = closestMonster;
                targetMonsterObj = closestMonster.gameObject;
            }
        }else{
            // if no monster, traget monster reset
            targetMonster = null;
        }
    }

    private void Move(){
        // set animation 
        ChangeState(CurrentCharacterState.Move);

        // Calculate direction : character -> monster
        Vector2 direction = targetMonsterObj.transform.position - transform.position;
        direction.Normalize();

        // Flip image according to the direction
        ChangeImageDirection(direction);

        // move to monster
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    private void ChangeImageDirection(Vector2 direction){
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); 
            // set no filp ui image
            _characterInfoPanel.gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1); 
            // set no filp ui image
            _characterInfoPanel.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    /// ///////////////////////////////////////////////////////////////////////
    /// die

    private void Die(){
        isAlive = false;

        // set animation
        ChangeState(CurrentCharacterState.Die);

        _characterInfoPanel.ZeroHP();
        GameManager.Instance.charactersController.CheckAllCharacterDie();
        StartRespone();

        // play sfx
        GameSoundManager.Instance.SfxPlay(SFX.Die);
    }

    private void StartRespone(){
        if(responeCoroutine != null) StopCoroutine(responeCoroutine);
        responeCoroutine = StartCoroutine(waitResponeCoroutine());
    }

    private IEnumerator waitResponeCoroutine()
    {
        yield return new WaitForSeconds(GameManager.Instance.characterResponeTime);
        if(GameManager.Instance.isStageStart) Reset();
    }
}
