using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class UserInterfaceController : MonoBehaviour
{
    [SerializeField]
    private GameObject _HomeUI;

    [SerializeField]
    private AudioSource _bgm;

    [SerializeField]
    private StorySystem _StoryUI;

    [SerializeField]
    private GameObject _GameUI;

    [SerializeField]
    private TimerDisplay Timer;

    [SerializeField]
    private VideoPlayer _player;

    [SerializeField]
    private Image _bg;

    [SerializeField]
    private Image _tip;

    [SerializeField]
    private List<Sprite> _spriteList;

    [SerializeField]
    private Image _crossEfect;

    [SerializeField]
    private float _duration = 3;

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

    private Vector3 _initPos;
    private Quaternion _initRot;

    void Start()
    {
        if (Instance != this) Destroy(this);
        _initPos = Camera.main.transform.position;
        _initRot = Camera.main.transform.rotation;
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
        _bgm.Stop();
        SceneManager.UnloadSceneAsync(_currentGame);
        _player.Play();
    }

    public void PlayStory()
    {
        _HomeUI.SetActive(false);
        _StoryUI.gameObject.SetActive(true);
    }

    public void StartPlay()
    {
        _StoryUI.gameObject.SetActive(false);
        _GameUI.SetActive(true);
        Timer.StartCountdown(90f, PlayGameOverVideo);
        JumpStage1();
    }

    private string _currentGame;

    public void JumpStage1()
    {
        _tip.sprite = _spriteList[0];
        _currentGame = "Table";
        CrossScene(_currentGame);
    }

    public void JumpStage2()
    {
        WinEffect.show();
        _tip.sprite = _spriteList[1];
        CrossScene("Wire", _currentGame);
        _currentGame = "Wire";
    }

    public void JumpStage3()
    {
        WinEffect.show(Quaternion.Euler(90, 0, 0));
        Camera.main.transform.SetPositionAndRotation(_initPos, _initRot);
        _tip.sprite = _spriteList[2];
        CrossScene("TV", _currentGame);
        _currentGame = "TV";
    }
}