using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class UserInterfaceController : MonoBehaviour
{
    [SerializeField]
    public StorySystem _StoryUI;

    [SerializeField]
    public GameObject _GameUI;

    [SerializeField]
    private TimerDisplay Timer;

    [SerializeField]
    private VideoPlayer _player;

    [SerializeField]
    public Image _bg;
    
    [SerializeField]
    public Image _tip;

    [SerializeField]
    public List<Sprite> _spriteList;

    [SerializeField]
    public Image _crossEfect;

    [SerializeField]
    public float _duration = 3;

    private static UserInterfaceController s_Instance;

    public static UserInterfaceController Instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType(typeof(UserInterfaceController)) as UserInterfaceController;
                if (s_Instance == null)
                {
                    GameObject go = new GameObject("UserInterfaceController");
                    s_Instance = go.AddComponent<UserInterfaceController>();
                }
            }

            return s_Instance;
        }
    }

    void Start()
    {
        if (Instance != this) Destroy(this);
    }

    public void CrossScene(string To, string From = null)
    {
        var fadeOut = _crossEfect.DOFade(1, _duration / 2);
        var fadeIn = _crossEfect.DOFade(0, _duration / 2);
        Sequence mySequence = DOTween.Sequence();
        mySequence.AppendCallback(() =>
        {
            if (!string.IsNullOrEmpty(From))
                SceneManager.UnloadSceneAsync(From);
        });
        mySequence.Append(fadeOut);
        mySequence.AppendCallback(() => { SceneManager.LoadSceneAsync(To, LoadSceneMode.Additive); });
        mySequence.Append(fadeIn);
        mySequence.AppendCallback(() => { _bg.gameObject.SetActive(false); });
        
    }

    public void PlayGameOverVideo()
    {
        SceneManager.UnloadSceneAsync("Table");
        _player.Play();
    }

    public void StartPlay()
    {
        _StoryUI.gameObject.SetActive(false);
        _GameUI.SetActive(true);
        Timer.StartCountdown(5f, PlayGameOverVideo);
        JumpStage1();
    }

    public void JumpStage1()
    {
        _tip.sprite = _spriteList[0];
        CrossScene("Table");
    }
    
    public void JumpStage2()
    {
        _tip.sprite = _spriteList[1];
        CrossScene("Wire");
    }
    
    public void JumpStage3()
    {
        _tip.sprite = _spriteList[2];
        CrossScene("TV");
    }
}