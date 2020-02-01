using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    public AnimationCurve moveTrackX;
    public AnimationCurve moveTrackY;
    public AnimationCurve moveTrackZ;

    public GameObject hammer;
    public GameObject table;
    public GameObject badBar;
    public GameObject goodRangeBar;

    public AudioClip[] audios;

    protected float totleTimeX = 0;
    protected float totleTimeY = 0;
    protected float totleTimeZ = 0;
    protected Vector3 cursorOriginalPosition;
    protected Vector3 hammerOriginalPosition;
    protected float runtime;
    void Start()
    {
        totleTimeX = moveTrackX[moveTrackX.length - 1].time;
        totleTimeY = moveTrackY[moveTrackY.length - 1].time;
        totleTimeZ = moveTrackZ[moveTrackZ.length - 1].time;
        cursorOriginalPosition = transform.localPosition;
        hammerOriginalPosition = hammer.transform.position;
        newRange();
        hammer.transform.position = new Vector3(hammerOriginalPosition.y, hammerOriginalPosition.y, hammerOriginalPosition.z - 1000f);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !hammer.GetComponent<Animator>().GetBool("do_knock"))
        {
            hammer.GetComponent<Animator>().SetBool("do_knock", true);
            Invoke("nextDirection", 0.15f);
            if (
                goodRangeBar.transform.localPosition.x - goodRangeBar.GetComponent<RectTransform>().rect.width / 2 <= gameObject.transform.localPosition.x 
                && 
                gameObject.transform.localPosition.x <= goodRangeBar.transform.localPosition.x + goodRangeBar.GetComponent<RectTransform>().rect.width / 2
             )
            {
                var legs = table.GetComponent<TableLegs>().legs[(table.GetComponent<Animator>().GetInteger("direction") + 1) % 4];

                hammer.GetComponent<AudioSource>().PlayOneShot(audios[0]);

                if (legs.Count > 0) {
                    hammer.transform.position = new Vector3(
                        legs[0].transform.position.x,
                        legs[0].transform.position.y,
                        legs[0].transform.position.z
                    );
                    legs[0].GetComponent<TableLeg>().toDestroy();
                    legs.RemoveAt(0);
                    newRange();
                    if (table.GetComponent<TableLegs>().isWin())
                    {
                        Debug.Log("贏了");
                    }
                }
            }
            else
            {
                hammer.GetComponent<AudioSource>().PlayOneShot(audios[1]);
                //var p = Camera.main.ScreenToWorldPoint(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, hammerOriginalPosition.z));
                hammer.transform.position = new Vector3(hammerOriginalPosition.x, hammerOriginalPosition.y, hammerOriginalPosition.z);
            }
        }
        else if (!hammer.GetComponent<Animator>().GetBool("do_knock"))
        {
            runtime += Time.deltaTime;
            gameObject.transform.localPosition = new Vector3(moveTrackX.Evaluate(runtime % totleTimeX) + cursorOriginalPosition.x, moveTrackY.Evaluate(runtime % totleTimeY) + cursorOriginalPosition.y, moveTrackZ.Evaluate(runtime % totleTimeZ) + cursorOriginalPosition.z);
        }

    }
    void nextDirection()
    {
        table.GetComponent<Animator>().SetInteger("direction", (table.GetComponent<Animator>().GetInteger("direction") + 1) % 4);
        hammer.transform.position = new Vector3(hammerOriginalPosition.y, hammerOriginalPosition.y, hammerOriginalPosition.z - 1000f);
    }

    void newRange()
    {
        var goodBarWidth = badBar.GetComponent<RectTransform>().rect.width / Random.Range(5f, 10f);
        goodRangeBar.transform.localPosition = new Vector2(Random.Range(0, badBar.GetComponent<RectTransform>().rect.width / 2 - goodBarWidth / 2) * (Random.Range(0, 2) == 0 ? 1 : -1), goodRangeBar.transform.localPosition.y);
        var rect = goodRangeBar.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(goodBarWidth, rect.sizeDelta.y);
    }
}
