using UnityEngine;

public class SelfDestroy : MonoBehaviour
{

    [SerializeField] private ParticleSystem ps;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if(ps != null && !ps.IsAlive())
        {
            DestroySelf();
        }
    }

    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
