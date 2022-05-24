using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackPencil : Enemy
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        StartCoroutine(MoveBehaviour());
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    private IEnumerator MoveBehaviour()
    {
        yield return new WaitForSeconds(1f);

        if (this.transform.eulerAngles.y == 270)
        {
            this.transform.rotation = Quaternion.Euler(0, 90, 0);
        } else if (this.transform.eulerAngles.y == 90)
        {
            this.transform.rotation = Quaternion.Euler(0, -90, 0);
        }

        StartCoroutine(MoveBehaviour());
    }
}
