using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapSlapper : Enemy    
{
    public float turnSpeed;
    public float speed;
    public float animationSpeed;
    public GameObject[] animations;

    private int index = 0;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        StartCoroutine(Behaviour());
        StartCoroutine(Animation());
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        Move();
    }

    private IEnumerator Animation()
    {
        yield return new WaitForSeconds(animationSpeed);

        foreach (GameObject go in animations)
        {
            go.SetActive(false);
        }

        animations[index].SetActive(true);

        index++;

        if (index > animations.Length - 1)
        {
            index = 0;
        }

        StartCoroutine(Animation());
    }

    private void Move()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime);
    }

    private IEnumerator Behaviour()
    {
        yield return new WaitForSeconds(turnSpeed);

        if (this.transform.eulerAngles.y == 270)
        {
            this.transform.rotation = Quaternion.Euler(0, 90, 0);

            yield return new WaitForSeconds(turnSpeed / 2);
        }
        else if (this.transform.eulerAngles.y == 90)
        {
            this.transform.rotation = Quaternion.Euler(0, -90, 0);

            yield return new WaitForSeconds(turnSpeed / 2);
        }

        StartCoroutine(Behaviour());
    }
}
