using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemyBehavior : MonoBehaviour
{
    [Header("Object Refs")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer sprite_renderer;
    [SerializeField] private Rigidbody2D rigidbody_2d;
    [SerializeField] private Collider2D collider_2d;
    [SerializeField] private WeaponBehavior weapon_behavior;
    [SerializeField] private Transform target;
    [SerializeField] private GameObject stun_effect;
    [SerializeField] private GameObject attack_indicator;
    [SerializeField] private WeaponRigController weapon_rig;

    [SerializeField] private Material red_mat;
    [SerializeField] private Material default_mat;

    [Header("Enemy Settings")]
    [SerializeField] private float speed;
    [SerializeField] private int health;
    [SerializeField] private float distance_to_attack;
    [SerializeField] private int reward_amount;

    [SerializeField] private bool attacking = false;
    private bool walking = false;
    private bool stunned = false;
    private bool exit = false;
    private bool knockback_active = false;
    private bool summon_available = true;
    protected GameObject spawner = null;

    [Header("Minion Settings")]
    [SerializeField] private int summon_cooldown;
    [SerializeField] private Sprite summmoning_sprite;
    [SerializeField] private GameObject weapon_rig_inst;
    [SerializeField] private GameObject fry;
    [SerializeField] private GameObject fork;
    [SerializeField] private List<Vector3> enemy_spawns = new List<Vector3>();
    [SerializeField] private Transform room;

    void Start()
    {
        FinanceController.Instance.SetCurrency(0);
    }

    void Update()
    {
        Walk_Towards();
    }

    public virtual void SetTarget(Transform target_transfr) {
        target = target_transfr;
    }

    public virtual void SetAnchor() {
        weapon_behavior.SetAnchor(transform);
    }

    public virtual void SetWeapon(WeaponBehavior weapon) {
        weapon_behavior = weapon;
    }

    public virtual void SetSpawner(GameObject gate)
    {
        spawner = gate;
    }

    public virtual Transform GetTarget() {
        return target;
    } 

    public virtual void SetWeaponRig(WeaponRigController rig) {
        weapon_rig = rig;
    }

    public virtual void Walk_Towards() {
        float distance_between = Vector3.Distance(transform.position, target.position);
        if (distance_between > distance_to_attack && !attacking && !stunned) {
            if (!walking) {
                animator.SetBool("is_walking", true);
                walking = true;
            }
            // transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * speed);
            Vector3 movement = (target.position - transform.position);
            movement.Normalize();
            rigidbody_2d.velocity =  movement * speed;
            walking = true;
        } else {
            if (!attacking && !stunned) {
                if (walking) {
                    animator.SetBool("is_walking", false);
                    rigidbody_2d.velocity = Vector2.zero;
                    walking = false;
                }
                StartCoroutine(Attack());
            }
            
        }
    }

    public virtual IEnumerator Attack() {
        // attacking = true;
        // attack_indicator.SetActive(true);
        // yield return new WaitForSeconds(0.25f);
        // if (!stunned) {
        //     weapon_behavior.Swing();
        // }
        // attack_indicator.SetActive(false);
        // yield return new WaitForSeconds(1f);
        // attacking = false;

        if (!stunned) {
           if (summon_available) {
                summon_available = false;
                attacking = true;
                StartCoroutine(SpawnEnemies());
                StartCoroutine(SummonCooldown());
            } else {
                // do do di do doo
            } 
        }
        yield return null;
    }

    public virtual IEnumerator Hitstun() {
        stunned = true;
        yield return new WaitForSeconds(0.5f);
        stunned = false;
        exit = false;
    }

    public virtual void Death() {
        spawner.GetComponent<BossRoomController>().bossDead();
        StopAllCoroutines();
        // Destroy(weapon_rig.gameObject);
        Destroy(gameObject);
        UIController.Instance.ShowWinScreen();
    }

    public virtual IEnumerator Knockback(Collider2D collider) {
        // hehe no knockback
        yield return null;
    }

    public virtual void OnTriggerEnter2D(Collider2D collider) {
        // if:
        // not collided with player
        // not the weapon this object is holding
        // not collided with another enemy
        // if (collider.gameObject.name[2] != 'W') {
        //     return;
        //     // Physics2D.IgnoreCollision(collider_2d, collider.GetComponent<Collider>(), true);
        // }
        if (collider.gameObject.name != "P_Player" && collider.gameObject.name[2] != 'E') {
            //Debug.Log(collider.gameObject.name + " collided with " + gameObject.name);
            WeaponBehavior collider_weapon_behavior = collider.transform.parent.GetComponent<WeaponBehavior>();
            if (collider_weapon_behavior != null) { // if is a weapon
                if (!collider_weapon_behavior.IsEnemyWeapon()) { // if is not an enemy weapon
                    health -= collider_weapon_behavior.GetDamage();
                    UIController.Instance.BossTakeDamage(collider_weapon_behavior.GetDamage());
                    StartCoroutine(damage_effect());
                    if (health > 0) {
                        StartCoroutine(Knockback(collider));
                        // sprite_renderer.color /= 1.1f;
                        StartCoroutine(Hitstun()); 
                        
                    } else {
                        Death();
                    }
                    
                }
            } else { // else is not a weapon, probably contact damage
                health--;
                // sprite_renderer.color /= 1.1f;
                // StartCoroutine(Hitstun()); 
            }
            if (health <= 0) {
                Death();

            }
        }
        
    }

    public IEnumerator damage_effect() {
        sprite_renderer.material = red_mat;
        yield return new WaitForSeconds(0.25f);
        sprite_renderer.material = default_mat;
    }

    public virtual void OnTriggerExit2D(Collider2D collider) {
        if (!exit) {
            sprite_renderer.color *= 1.1f;
            exit = true;
        }
    }

    public virtual void Parried() {
        StartCoroutine(Parry_Stun());
    }

    public virtual IEnumerator Parry_Stun() {
        stunned = true;
        stun_effect.SetActive(true);
        stun_effect.transform.rotation = new Quaternion(0, 180, 0, 0);
        yield return new WaitForSeconds(0.25f);
        stun_effect.transform.rotation = new Quaternion(0, 0, 0, 0);
        yield return new WaitForSeconds(0.25f);
        stun_effect.transform.rotation = new Quaternion(0, 180, 0, 0);
        yield return new WaitForSeconds(0.25f);
        stun_effect.transform.rotation = new Quaternion(0, 0, 0, 0);
        yield return new WaitForSeconds(0.25f);
        stun_effect.SetActive(false);
        stunned = false;
    }

    private IEnumerator SpawnEnemies() {
        animator.SetBool("is_summoning", true);
        // Sprite starting_sprite = sprite_renderer.sprite;
        // sprite_renderer.sprite = summmoning_sprite;
        for (int i = 0; i < enemy_spawns.Count; i++) {
            SpawnEnemy(enemy_spawns[i]);
        }
        yield return new WaitForSeconds(1f);
        // sprite_renderer.sprite = starting_sprite;
        animator.SetBool("is_summoning", false);
        attacking = false;
    }

    private IEnumerator SummonCooldown() {
        float temp_distance = distance_to_attack;
        distance_to_attack = 3f;
        yield return new WaitForSeconds(summon_cooldown);
        summon_available = true;
        distance_to_attack = temp_distance;
    }

    private void SpawnEnemy(Vector3 pos) {

        GameObject hero = GameController.Instance.GetHero();

        Vector3 enemy_local_pos = room.TransformPoint(pos);

        GameObject enemy_obj = Instantiate(fry, enemy_local_pos, new Quaternion(0, 0, 0, 0));
        FryEnemyBehavior enemy_behavior = enemy_obj.GetComponent<FryEnemyBehavior>();
        enemy_behavior.Spawn();
        enemy_behavior.SetTarget(hero.transform);
        GameObject weapon_rig_obj = Instantiate(weapon_rig_inst);
        WeaponRigController controller = weapon_rig_obj.GetComponent<WeaponRigController>();
        controller.Set_Attr(fork, enemy_obj.transform, enemy_behavior, true);
        enemy_behavior.SetWeaponRig(controller);
        
    }
}
