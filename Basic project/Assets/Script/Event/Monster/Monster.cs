using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    /* Stat */
    [HideInInspector] public float _health; 
    [HideInInspector] public float _attackPower;
    [HideInInspector] public float _attackNumber;
    [HideInInspector] public float _attackDistance;

    /* Values*/
    [HideInInspector] public Animator animator;
    [HideInInspector] public int index;
    [HideInInspector] public int level = 1;
    [HideInInspector] public float _exp;
    [HideInInspector] public int _coin;
    [HideInInspector] public bool isAlive = true;
    [HideInInspector] public float moveRandomRange = 5f; 
    [HideInInspector] public float moveSpeed = 1f;
    [HideInInspector] public CurrentMonsterState currentState = CurrentMonsterState.Idle; 
    [HideInInspector] public MonsterInfoPanel _monsterInfoPanel;
    private float attackCoolTime;
    private bool isBlackOut = false;
    private bool isAttackOnCooldown = false;
    private float currentAttackCooldownTime = 0f; 
    private GameObject targetCharacterObj = null;
    private Character targetCharacter = null;
    private Vector2 targetRandomPosition; // Target location to move
    private Coroutine blackOutCoroutine; 

    /////////////////////////////////////////////////////////////////////////////////
    /// virtual

    public virtual void Init(){}
    public virtual void Reset(){
        targetCharacter = null;
        targetCharacterObj = null;
        _monsterInfoPanel.RestoreHPAll();

        // set animation
        ChangeState(CurrentMonsterState.Idle);
        
        animator.Play("Idle", 0, 0f);
        isAlive = true;
    }
    public virtual void Upgrade(){}

    /////////////////////////////////////////////////////////////////////////////////
    /// main

    public void Update(){
        // if monster alive and Stage Start
        if(isAlive && isBlackOut == false && GameManager.Instance.isStageStart)
        {
            // if no track character
            if(targetCharacter == null) {
                // find track character
                TrackCharacter();
            }

            if(targetCharacter != null){
                // If the target character is outside the attack range
                if(Vector3.Distance(transform.position, targetCharacterObj.transform.position) > _attackDistance){
                    // move the tracking
                    TrackMove();
                }else{
                    // inside -> attack
                    Attack();
                }

            }else{
                // if Character not found,
                // around patrol
                Move();
            }

            UpdateCooldownTimer();
        }

        if(isAlive && GameManager.Instance.isStageStart == false){
            // set animation
            ChangeState(CurrentMonsterState.Idle);
        }
    }

    /////////////////////////////////////////////////////////////////////////////////
    /// function

    public void SettingIndex(int index){
        this.index = index;
    }

    // Set target locations at random
    public void SetRandomTargetPosition()
    {
        // Create a random location within the moveRange around the current location
        targetRandomPosition = (Vector2)transform.position + Random.insideUnitCircle * moveRandomRange;
    }

    // black out
    public void BlackOut(float time){
        isBlackOut = true;

        // set animation
        ChangeState(CurrentMonsterState.Idle);

        if(blackOutCoroutine != null) StopCoroutine(blackOutCoroutine);
        blackOutCoroutine = StartCoroutine(waitBlackOutCoroutine(time));
    }

    public void TakeDamage(Character attackCharacter, float damageAmount){
        
        // If attacked, if there is no target character, set target character on the attacker
        if(targetCharacter == null) {
            targetCharacter = attackCharacter;
            targetCharacterObj = attackCharacter.gameObject;
        }

        _monsterInfoPanel.DamgeHP(damageAmount);
        _health -= damageAmount;
        if(_health < 0){
            _health = 0;
            Die();

            // send exp data -> character (last attack character)
            attackCharacter.GetEXP(_exp);
        }
    }

    // change the state
    public void ChangeState(CurrentMonsterState newState)
    {
        if(currentState == newState) return;
        if(animator == null) return;

        currentState = newState;

        switch (currentState)
        {
            case CurrentMonsterState.Run:
                // move animation
                animator.SetBool("isAttack", false);
                animator.SetBool("isRun", true);
                break;
            case CurrentMonsterState.Attack:
                // attack animation
                animator.SetBool("isAttack", true);
                animator.SetBool("isRun", false);
                break;
            case CurrentMonsterState.Die:
                // dead animation
                animator.SetBool("isAttack", false);
                animator.SetBool("isRun", false);
                animator.SetBool("isDie", true);
                break;
            default:
                // idle animation
                animator.SetBool("isRun", false);
                animator.SetBool("isAttack", false);
                animator.SetBool("isDie", false);
                break;
        }
    }

    /////////////////////////////////////////////////////////////////////////////////
    /// track and move

    private void TrackMove(){
        // set animation
        ChangeState(CurrentMonsterState.Run);

        // calculation direction : monster -> character 
        Vector2 direction = targetCharacterObj.transform.position - transform.position;
        direction.Normalize();

        // Flip the image according to the direction
        ChangeImageDirection(direction);

        // move to character
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    private void Move(){
        // set animation
        ChangeState(CurrentMonsterState.Run);

        // If there is no target position, set a new target position
        if(targetRandomPosition == null) SetRandomTargetPosition();
        // if the target location is reached, Set a new target location
        if ((Vector2)transform.position == targetRandomPosition)
        {
            SetRandomTargetPosition();
        }

        Vector2 direction = targetRandomPosition - (Vector2)transform.position;
        direction.Normalize();
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    private void TrackCharacter()
    {
        if(GameManager.Instance.charactersController.characters.Count > 0){
            foreach (var character in GameManager.Instance.charactersController.characters)
            {
                if(character.isAlive){
                    if (Vector3.Distance(transform.position, character.transform.position) < _attackDistance)
                    {
                        targetCharacter = character;
                        targetCharacterObj = character.gameObject;
                        return;
                    }
                }
            }
        }

        targetCharacter = null;
        targetCharacterObj = null;
    }

    /// ///////////////////////////////////////////////////////////////////////
    /// about attack

    private void UpdateCooldownTimer()
    {
        float attackCoolTime = 1 / _attackNumber;

        if (isAttackOnCooldown)
        {
            currentAttackCooldownTime += Time.deltaTime;
            if (currentAttackCooldownTime >= attackCoolTime)
            {
                isAttackOnCooldown = false;
                currentAttackCooldownTime = 0f;
            }
        }
    }

    private void Attack(){
        ChangeState(CurrentMonsterState.Attack);

        // change attack taget direction image
        Vector2 direction = (Vector2)targetCharacterObj.transform.position - (Vector2)transform.position;
        direction.Normalize();
        ChangeImageDirection(direction);

        if(isAttackOnCooldown == false){
            isAttackOnCooldown = true;
            targetCharacter.TakeDamage(_attackPower);
            if(targetCharacter.isAlive == false){
                targetCharacter = null;
            }
        }
    }

    // Flip the image according to the direction
    private void ChangeImageDirection(Vector2 direction){
        if (direction.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); 
            _monsterInfoPanel.gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (direction.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            _monsterInfoPanel.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    // wait black out over => no attack / no animation
    private IEnumerator waitBlackOutCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        isBlackOut = false;
    }

    /// ///////////////////////////////////////////////////////////////////////
    /// die

    private void Die(){
        // set animation
        ChangeState(CurrentMonsterState.Die);

        _monsterInfoPanel.ZeroHP();
        isAlive = false;
        GameManager.Instance.playerController.AddCoin(_coin);
        GameManager.Instance.monstersController.CountDieMonster();
    }
}
