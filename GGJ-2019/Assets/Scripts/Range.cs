﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed;
    
    public GameObject muzzlePrefab;

    private GameObject target;
    private Transform pivot;
    private Transform chevron;
    public float reloadRate = 3f;
    private float reload = 0;
    public bool canShoot = true;
    private AudioSource shoot;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player");
        chevron = gameObject.transform.Find("Pivot/Chevron");
        pivot = gameObject.transform.Find("Pivot");
        shoot = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (reload > 0)
            reload -= Time.deltaTime;
        var dist = Vector3.Distance(target.transform.position, gameObject.transform.position);
        if ((dist <= 13) && (reload <= 0) && canShoot)
        {
            shoot.pitch = Random.Range(0.975f, 1.25f);
            shoot.Play();
            Vector2 myPos = new Vector2(transform.position.x, transform.position.y);
            Vector2 t = new Vector2(target.transform.localPosition.x, target.transform.localPosition.y);

            Vector2 direction = t - myPos;
            direction.Normalize();
            GameObject projectile = (GameObject)Instantiate(projectilePrefab, new Vector3(myPos.x, myPos.y, -2), Quaternion.identity);
            projectile.GetComponent<Rigidbody>().velocity = direction * projectileSpeed;

            projectile.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(t.y - myPos.y, t.x - myPos.x) * Mathf.Rad2Deg);

            pivot.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(t.y - myPos.y, t.x - myPos.x) * Mathf.Rad2Deg);
            var muzzle = (GameObject)Instantiate(muzzlePrefab, chevron.position, pivot.rotation);

            reload = reloadRate;
        }
    }
}
