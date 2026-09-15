using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField, NotNull] private RectTransform m_root;
    [SerializeField, NotNull] private RectTransform m_title;
    [SerializeField, NotNull] private Button m_enterBtn;
    [SerializeField, NotNull] private Button m_quitButton;

    [Header("Position")]
    [SerializeField, NotNull] private float m_offScreenY;
    [SerializeField, NotNull] private float m_onScreenY;


    private void Start()
    {
        m_enterBtn.onClick.AddListener(EnterGame);
        m_quitButton.onClick.AddListener(QuitGame);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitGame();
        }

        LeanTween.scale(m_title, new Vector3(1.5f, 1.5f, 1.5f), 0.5f)
            .setEaseInOutSine()
            .setOnComplete(() =>
            {
                LeanTween.scale(m_title, new Vector3(1.5f, 1.5f, 1.5f), 0.5f).setEaseInOutSine();
            });
    }

    private void EnterGame()
    {
        LeanTween.moveY(m_root, m_offScreenY, 0.5f).setEaseInOutSine();
    }

    private void ExitGame()
    {
        LeanTween.moveY(m_root, m_onScreenY, 0.5f).setEaseInOutSine();
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
