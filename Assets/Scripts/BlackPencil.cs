using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackPencil : Enemy
{
    public int throwingDamage;
    public int throwingSpeed;
    public GameObject bullet;

    public AudioSource throwingSound;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        StartCoroutine(Behaviour());
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    private IEnumerator Behaviour()
    {
        yield return new WaitForSeconds(1f);

        if (this.transform.eulerAngles.y == 270)
        {
            this.transform.rotation = Quaternion.Euler(0, 90, 0);

            Shoot(true);
        } else if (this.transform.eulerAngles.y == 90)
        {
            this.transform.rotation = Quaternion.Euler(0, -90, 0);

            Shoot(false);
        }

        

        StartCoroutine(Behaviour());
    }

    private void Shoot(bool shootRight)
    {
        throwingSound.Play();
        BlackPencilBullet pencil = Instantiate(bullet, this.transform.position + new Vector3(0,0.5f,0), Quaternion.Euler(0,this.transform.eulerAngles.y,0)).GetComponent<BlackPencilBullet>();
        pencil.damage = this.throwingDamage;
        pencil.speed = this.throwingSpeed;
        pencil.shootRight = shootRight;
    }
}
