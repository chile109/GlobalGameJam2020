using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StorySystem : MonoBehaviour
{
    [SerializeField]
    public Image _content;

    [SerializeField]
    public List<Sprite> _spriteList;

    [SerializeField]
    public Image _dad;

    [SerializeField]
    public Image _mom;

    private int _index = 1;

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            NextDialog();
        }
    }

    public void NextDialog()
    {
        _index += 1;
        _content.sprite = _spriteList[_index - 1];
        _content.SetNativeSize();
        
        switch (_index)
        {
            case 1:
                _mom.gameObject.SetActive(true);
                break;
            case 2:
                _mom.gameObject.SetActive(false);
                _dad.gameObject.SetActive(true);
                break;
            case 4:
                this.enabled = false;
                _dad.gameObject.SetActive(false);
                
                var ZoomIn1 = _content.rectTransform.DOScale(2, 0.1f);
                var ZoomIn2 = _content.rectTransform.DOScale(4, 0.1f);
                var ZoomIn3 = _content.rectTransform.DOScale(9, 0.05f);
                Sequence mySequence = DOTween.Sequence();
                mySequence.Append(ZoomIn1);
                mySequence.AppendInterval(0.5f);
                mySequence.Append(ZoomIn2);
                mySequence.AppendInterval(0.5f);
                mySequence.Append(ZoomIn3);
                mySequence.AppendInterval(0.5f);
                mySequence.AppendCallback(() =>
                {
                    UserInterfaceController.Instance.StartPlay();
                    this.gameObject.SetActive(false);
                    
                });
                break;
        }
    }
}