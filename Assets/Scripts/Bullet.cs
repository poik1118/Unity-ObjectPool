using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float               _Speed = 3f;

    private Vector3             _Direction;
    private IObjectPool<Bullet> _ManagedPool;

    
    void Update()
    {
        transform.Translate(_Direction * Time.deltaTime * _Speed);
    }


    public void Shoot(Vector3 dir)
    {
        _Direction = dir;

        // 5초가 지난 후 총알을 파괴
        //Destroy(gameObject, 5f);

        Invoke("DestroyBullet", 5.0f);
    }


    public void SetManagedPool(IObjectPool<Bullet> pool){
        _ManagedPool = pool;
    }
    public void DestroyBullet(){
        _ManagedPool.Release(this);
    }
}
