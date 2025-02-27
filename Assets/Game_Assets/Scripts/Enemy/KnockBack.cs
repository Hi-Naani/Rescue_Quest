using System.Collections;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    private Flash flash;
    private Rigidbody2D rb;
    private bool gettingKnocked;

    public bool GettingKnocked
    {
        get
        {
            return gettingKnocked;
        }
        private set
        {
            gettingKnocked = value;
        }
    }

    private void Start()
    {
        flash = GetComponent<Flash>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        gettingKnocked = false;
    }

    public void GotKnocked(Transform damageSource, float knockBackThrust)
    {
        GettingKnocked = true;
        Vector2 direction = (this.transform.position - damageSource.transform.position).normalized;
        Vector2 forceAmount = (direction * knockBackThrust * rb.mass);
        rb.AddForce(forceAmount, ForceMode2D.Impulse);
        StartCoroutine(KnockRoutine());
        StartCoroutine(flash.Flashing());
    }

    IEnumerator KnockRoutine()
    {
        yield return new WaitForSeconds(0.2f);
        rb.velocity = Vector2.zero;
        GettingKnocked = false;
        StopCoroutine(KnockRoutine());
    }
}
