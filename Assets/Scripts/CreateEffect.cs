using System.Collections;
using UnityEngine;

public class CreateEffect : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sortingOrder = 2;
        float spriteWorldSize = sr.sprite.bounds.size.x;
        float scale = GameManager.Instance.TileSpriteSize / spriteWorldSize;
        transform.localScale = new Vector3(scale, scale, 1f);
    }

    public IEnumerator PlayAndWait(Vector3 position)
    {
        transform.position = position;
        animator.Play("create", 0, 0f);
        yield return null;

        float length = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(length);

        PoolingManager.Instance.CreateEffectPool.Release(this);
    }
}
