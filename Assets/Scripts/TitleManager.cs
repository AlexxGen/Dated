using System.Collections;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField, NotNull] private RectTransform m_root;
    [SerializeField, NotNull] private RectTransform m_title;
    [SerializeField, NotNull] private Button m_enterBtn;
    [SerializeField, NotNull] private Button m_quitBtn;

    [Header("Position")]
    [SerializeField, NotNull] private float m_offScreenY;
    [SerializeField, NotNull] private float m_onScreenY;
    [SerializeField, NotNull] private bool m_onScreen = true;

    private Coroutine tweenTitleRoutine;


    private void Start()
    {
        m_enterBtn.onClick.AddListener(EnterGame);
        m_quitBtn.onClick.AddListener(QuitGame);

        if (tweenTitleRoutine == null)
        {
            tweenTitleRoutine = StartCoroutine(tweenTitle());
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitGame();
        }
    }

    private void EnterGame()
    {
        LeanTween.moveY(m_root, m_offScreenY, 0.5f).setEaseInOutSine();
        m_onScreen = false;
    }

    private void ExitGame()
    {
        LeanTween.moveY(m_root, m_onScreenY, 0.5f).setEaseInOutSine();
        m_onScreen = true;
        if (tweenTitleRoutine == null)
        {
            tweenTitleRoutine = StartCoroutine(tweenTitle());
        }

    }

    private void QuitGame()
    {
        Debug.Log("quit clicked");
        StopCoroutine(tweenTitleRoutine);
        Application.Quit();
    }

    IEnumerator tweenTitle()
    {
        while (m_onScreen)
        {
            LeanTween.scale(m_title, Vector3.one * 1.05f, 0.5f)
            .setEaseInOutSine()
            .setOnComplete(() =>
            {
                LeanTween.scale(m_title, Vector3.one, 0.5f).setEaseInOutSine();
            });

            yield return new WaitForSeconds(1.0f);
        }
        tweenTitleRoutine = null;
    }
}
