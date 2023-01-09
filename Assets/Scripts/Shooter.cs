using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Shooter : MonoBehaviour
{
    [SerializeField]
    private GameObject          _BulletPrefab;

    private Camera              _MainCam;
    private IObjectPool<Bullet> _BulletPool;

    private void Awake()
    {
        _BulletPool = new ObjectPool<Bullet>(CreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet, maxSize:20);
    }

    void Start()
    {
        _MainCam = Camera.main;
    } 

    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            RaycastHit hitResult;
            if(Physics.Raycast(_MainCam.ScreenPointToRay(Input.mousePosition), out hitResult))
            {
                var direction = new Vector3(hitResult.point.x, transform.position.y, hitResult.point.z) - transform.position;
                // 총알을 새로 생성해서 사용
                //var bullet = Instantiate(_BulletPrefab, transform.position + direction.normalized, Quaternion.identity).GetComponent<Bullet>();
                var bullet = _BulletPool.Get(); 

                bullet.transform.position = transform.position + direction.normalized;
                bullet.Shoot(direction.normalized);
            }
        }
    }


    private Bullet CreateBullet(){
        Bullet bullet = Instantiate(_BulletPrefab).GetComponent<Bullet>();
        bullet.SetManagedPool(_BulletPool);
        return bullet;
    }

    private void OnGetBullet(Bullet bullet){
        bullet.gameObject.SetActive(true);
    }
    private void OnReleaseBullet(Bullet bullet){
        bullet.gameObject.SetActive(false);
    }

    private void OnDestroyBullet(Bullet bullet){
        Destroy(bullet.gameObject);
    }
}
