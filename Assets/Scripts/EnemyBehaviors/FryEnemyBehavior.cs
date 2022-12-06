using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FryEnemyBehavior : EnemyBehavior
{
    [SerializeField] private Sprite[] spawn_frames;
    
    public void Spawn() {
        base.stunned = true;
        StartCoroutine(start_animation());
    }

    private IEnumerator start_animation() {
        Debug.Log("Fry summoning");
        Sprite starting_sprite = base.sprite_renderer.sprite;
        for (int i = 0; i < spawn_frames.Length; i++) {
            Debug.Log("changin sprite");
            base.sprite_renderer.sprite = spawn_frames[i];
            yield return new WaitForSeconds(0.25f);
        }
        base.sprite_renderer.sprite = starting_sprite;
        yield return new WaitForSeconds(0.25f);
        animator.SetBool("done_spawning", true);
        stunned = false;
    }
}