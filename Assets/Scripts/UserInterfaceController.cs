using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class UserInterfaceController : MonoBehaviour
{
    [SerializeField]
    private TimerDisplay Timer;
    
    [SerializeField]
    private VideoPlayer _player;

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

        Timer.StartCountdown(5f, PlayGameOverVideo);
    }

    public void CrossScene()
    {
        var fadeOut = _crossEfect.DOFade(1, _duration / 2);
        var fadeIn = _crossEfect.DOFade(0, _duration / 2);
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(fadeOut);
        mySequence.Append(fadeIn);
    }

    public void PlayGameOverVideo()
    {
        _player.Play();
    }
}