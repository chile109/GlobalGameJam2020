using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 槌子
 * */
public class Hammer : MonoBehaviour
{
    public GameObject table;
    public AudioClip[] audios;
    public TQEBar tQEBar;
    public Animator gamearAnimator;

    protected Vector3 hammerOriginalPosition;
    void Start()
    {
        hammerOriginalPosition = transform.localPosition;
        transform.localPosition = new Vector3(hammerOriginalPosition.y, hammerOriginalPosition.y, hammerOriginalPosition.z - 1000f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !gameObject.GetComponent<Animator>().GetBool("do_knock"))
        {
            gameObject.GetComponent<Animator>().SetBool("do_knock", true);
            tQEBar.isPause = true;
            Invoke("nextDirection", 0.15f);
            doKnock(tQEBar.getResult());
        }
        else if(!gameObject.GetComponent<Animator>().GetBool("do_knock"))
        {
            tQEBar.isPause = false;
        }
    }

    void doKnock(bool isHit)
    {
        if (isHit)
        {
            var legs = table.GetComponent<TableLegs>().legs[(gamearAnimator.GetInteger("direction") + 1) % 4];

            gameObject.GetComponent<AudioSource>().PlayOneShot(audios[0]);

            if (legs.Count > 0)
            {
                gameObject.transform.position = new Vector3(
                    legs[0].transform.position.x,
                    legs[0].transform.position.y,
                    legs[0].transform.position.z
                );
                legs[0].GetComponent<TableLeg>().toDestroy(transform.parent.rotation);
                legs.RemoveAt(0);
                if (table.GetComponent<TableLegs>().isWin())
                {
                    Debug.Log("贏了");
                }
            }
        }
        else
        {
            gameObject.GetComponent<AudioSource>().PlayOneShot(audios[1]);
            //var p = Camera.main.ScreenToWorldPoint(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, hammerOriginalPosition.z));
            gameObject.transform.localPosition = new Vector3(hammerOriginalPosition.x, hammerOriginalPosition.y, hammerOriginalPosition.z);
        }
    }

    void nextDirection()
    {
        gamearAnimator.SetInteger("direction", (gamearAnimator.GetInteger("direction") + 1) % 4);
        gameObject.transform.localPosition = new Vector3(hammerOriginalPosition.y, hammerOriginalPosition.y, hammerOriginalPosition.z - 1000f);
    }
}
