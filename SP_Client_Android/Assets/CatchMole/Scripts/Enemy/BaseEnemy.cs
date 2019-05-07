using UnityEngine;

public class BaseEnemy : MonoBehaviour {

    public BoxCollider2D boxColliderParent;
    Animator animator;
    float moveSpeed;
    float maxPosY;
    float minPosY;
    float deadTimer;
    bool flagMoveUP;
    bool startMove;
    EnumEnemyType enemyType; // Type of enemy

    public int scoreForKill; // number of points for kill
    public EnumEnemyState enemyState; // state of the enemy


    // Use this for initialization
    void Start() {
        animator = gameObject.GetComponent<Animator>();
        enemyState = EnumEnemyState.INST;
        minPosY = gameObject.transform.position.y;
        maxPosY = minPosY + 5;
    }

    // Update is called once per frame
    void Update() {
        MoveEnemy();
        DeadTimer();
    }

    public void HitMe(bool hit) { // The method is responsible for handling touch with the enemy, checks the enemy type and performs the following method KillEnemy()
        if (hit) {
            switch (enemyType) {
                case EnumEnemyType.BASE_ENEMY:
                    KillEnemy();
                    break;
                case EnumEnemyType.DEATH_ENEMY:
                    KillEnemy();
                    HealPoint.healPointScript.ChangeHealPoint(-1);
                    break;
                case EnumEnemyType.HEAL_ENEMY:
                    KillEnemy();
                    HealPoint.healPointScript.ChangeHealPoint(1);
                    break;
            }
        }
    }

    void MoveEnemy() { // The method is responsible for moving up and down this enemy
        if (startMove) {
            enemyState = EnumEnemyState.MOVE;
            if (gameObject.transform.position.y < maxPosY && !flagMoveUP) {
                gameObject.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + Time.deltaTime * moveSpeed);
                if (gameObject.transform.position.y >= maxPosY) { flagMoveUP = true; }
            } else if (gameObject.transform.position.y > minPosY && flagMoveUP) {
                gameObject.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - Time.deltaTime * moveSpeed);
                if (gameObject.transform.position.y <= minPosY) {
                    flagMoveUP = false;
                    if (enemyType == EnumEnemyType.BASE_ENEMY) {
                        HealPoint.healPointScript.ChangeHealPoint(-1);
                    }
                    ReInit();
                }
            }
        }
    }

    public void Init(RuntimeAnimatorController controller, EnumEnemyType enemyType, float moveSpeed, int scoreForKill) { // The method initializes the enemy and accepts the specified parameters
        animator.runtimeAnimatorController = controller;
        animator.Play("Move");
        this.scoreForKill = scoreForKill;
        this.enemyType = enemyType;
        this.moveSpeed = moveSpeed;
        startMove = true;
        boxColliderParent.enabled = true;
        flagMoveUP = false;
    }

    public void ReInit() { // The method resets all parameters to zero
        startMove = false;
        boxColliderParent.enabled = false;
        animator.runtimeAnimatorController = null;
        flagMoveUP = false;
        gameObject.transform.position = new Vector2(gameObject.transform.position.x, minPosY);
        enemyState = EnumEnemyState.INST;
    }

    public void KillEnemy() { // the method handles the death of the enemy
        boxColliderParent.enabled = false;
        startMove = false;
        animator.Play("Die");
        deadTimer = 0.5f;
        enemyState = EnumEnemyState.DEAD;
        //Score.scoreScript.AddScore(scoreForKill);
        AudioManager.audioManager.PlayAudio(EnumAudioName.KILL_ENEMY_MUSIC, true);
    }

    void DeadTimer() { // The death timer of the enemy, after which he goes into the waiting phase
        if (enemyState == EnumEnemyState.DEAD) {
            if (deadTimer > 0) {
                deadTimer -= Time.deltaTime;
            }
            else {
                ReInit();
            }
        }
    }
}
