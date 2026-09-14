using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;

public class BlockDrag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public static event Action<DayColumn, BlockData> onBlockDropped;

    [SerializeField] private GameObject placeholderPrefab;
    [SerializeField] private float returnTweenTime = 0.25f;
    [SerializeField] private LeanTweenType returnEaseType = LeanTweenType.easeOutCubic;

    private Transform previousParent;
    private int previousSiblingIndex;
    private CanvasGroup canvasGroup;
    private Canvas canvas;

    private GameObject currentPlaceholder;
    private RectTransform rectTransform;

    private DayColumn lastHoveredColumn;

    [SerializeField] private TextMeshProUGUI postIt;

    private int returnTweenId = -1;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (returnTweenId != -1)
        {
            LeanTween.cancel(gameObject);
            returnTweenId = -1;
        }

        LeanTween.cancel(gameObject);

        CleanupPlaceholder();

        postIt.text = GetComponent<BlockData>().Dialogue;

        previousParent = transform.parent;
        previousSiblingIndex = transform.GetSiblingIndex();

        if (placeholderPrefab != null )
        {
            currentPlaceholder = Instantiate(placeholderPrefab, previousParent);
            currentPlaceholder.transform.position = gameObject.transform.position;
            currentPlaceholder.transform.SetSiblingIndex(transform.GetSiblingIndex());

            RectTransform placeholderRect = currentPlaceholder.GetComponent<RectTransform>();
            if (placeholderRect != null )
            {
                placeholderRect.sizeDelta = rectTransform.sizeDelta;
            }
        }

        transform.SetParent(canvas.transform, true);

        canvasGroup.blocksRaycasts = false;

        if (previousParent != null)
        {
            DayColumn initialColumn = previousParent.GetComponent<DayColumn>();
            if (initialColumn != null)
            {
                lastHoveredColumn = initialColumn;
                initialColumn.AnimateLayout(gameObject.transform);
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

        if (currentPlaceholder != null)
        {
            DayColumn targetColumn = GetTargetColumn(eventData);
            if (targetColumn != null)
            {
                lastHoveredColumn = targetColumn;
            }

            DayColumn activeColumn = targetColumn != null ? targetColumn : lastHoveredColumn;

            if (activeColumn != null)
            {
                HandleAutoScroll(activeColumn, eventData.position);

                if (activeColumn.CanFitBlock(rectTransform.rect.height, currentPlaceholder.transform))
                {
                    bool parentChanged = currentPlaceholder.transform.parent != activeColumn.transform;

                    if (parentChanged)
                    {
                        DayColumn oldColumn = currentPlaceholder.transform.parent.GetComponent<DayColumn>();
                        currentPlaceholder.transform.SetParent(activeColumn.transform, false);

                        if (oldColumn != null)
                        {
                            oldColumn.ResetAutoScrollExpansion();
                            oldColumn.AnimateLayout(gameObject.transform);
                        }
                    }

                    int newIndex = activeColumn.DetermineInsertIndex(eventData.position, currentPlaceholder.transform);

                    if (parentChanged || currentPlaceholder.transform.GetSiblingIndex() != newIndex)
                    {
                        currentPlaceholder.transform.SetSiblingIndex(newIndex);
                        activeColumn.AnimateLayout(gameObject.transform);
                    }
                }
            }
        }
    }

    private DayColumn GetTargetColumn(PointerEventData eventData)
    {
        if (eventData.pointerEnter != null)
        {
            DayColumn column = eventData.pointerEnter.GetComponentInParent<DayColumn>();
            if (column != null)
                return column;
        }

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            DayColumn column = result.gameObject.GetComponentInParent<DayColumn>();
            if (column != null)
                return column;
        }

        return null;
    }

    private void HandleAutoScroll(DayColumn column, Vector2 mousePosition)
    {
        ScrollRect scrollRect = column.GetComponentInParent<ScrollRect>();
        if (scrollRect == null || !scrollRect.enabled || scrollRect.content == null)
            return;

        RectTransform viewport = scrollRect.viewport != null ? scrollRect.viewport : scrollRect.GetComponent<RectTransform>();

        Vector3[] viewportCorners = new Vector3[4];
        viewport.GetWorldCorners(viewportCorners);

        float bottomEdge = viewportCorners[0].y;
        float topEdge = viewportCorners[1].y;

        float threshold = column.scrollZoneThreshold;
        float speed = column.scrollSpeed;

        if (mousePosition.y < bottomEdge + threshold)
        {
            float dragBlockHeight = rectTransform.rect.height;
            column.ExpandForAutoScroll(dragBlockHeight + 150f);

            float intensity = Mathf.Clamp01((bottomEdge + threshold - mousePosition.y) / threshold);
            float scrollDelta = speed * intensity * Time.deltaTime;

            Vector2 contentPos = scrollRect.content.anchoredPosition;
            contentPos.y += scrollDelta;

            float contentHeight = scrollRect.content.rect.height;
            float viewportHeight = viewport.rect.height;
            float maxScrollY = Mathf.Max(0f, contentHeight - viewportHeight);

            contentPos.y = Mathf.Clamp(contentPos.y, 0f, maxScrollY);
            scrollRect.content.anchoredPosition = contentPos;
        }

        else if (mousePosition.y > topEdge - threshold)
        {
            float intensity = Mathf.Clamp01((mousePosition.y - (topEdge - threshold)) / threshold);
            float scrollDelta = speed * intensity * Time.deltaTime;

            Vector2 contentPos = scrollRect.content.anchoredPosition;
            contentPos.y -= scrollDelta;

            contentPos.y = Mathf.Max(0f, contentPos.y);
            scrollRect.content.anchoredPosition = contentPos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        if (currentPlaceholder != null)
        {
            DayColumn targetColumn = currentPlaceholder.transform.parent.GetComponent<DayColumn>();
            if (targetColumn != null)
            {
                targetColumn.AnimateLayout(gameObject.transform);
                onBlockDropped?.Invoke(targetColumn, gameObject.GetComponent<BlockData>());
            }

            Vector3 targetWorldPos = currentPlaceholder.transform.position;
            GameObject placeholderToDestroy = currentPlaceholder;

            returnTweenId = LeanTween.move(gameObject, targetWorldPos, returnTweenTime)
                .setEase(returnEaseType)
                .setOnComplete(() =>
                {
                    returnTweenId = -1;

                    if (placeholderToDestroy != null)
                    {
                        transform.SetParent(placeholderToDestroy.transform.parent, false);
                        transform.SetSiblingIndex(placeholderToDestroy.transform.GetSiblingIndex());

                        Destroy(placeholderToDestroy);

                        if (currentPlaceholder == placeholderToDestroy)
                        {
                            currentPlaceholder = null;
                        }

                        if (targetColumn != null)
                        {
                            targetColumn.ResetAutoScrollExpansion();

                            targetColumn.RefreshLayout();
                        }
                    }
                }).id;

        }
        else
        {
            transform.SetParent(previousParent, false);
            transform.SetSiblingIndex(previousSiblingIndex);
        }

        if (lastHoveredColumn != null)
        {
            lastHoveredColumn.ResetAutoScrollExpansion();
            lastHoveredColumn = null;
        }
    }

    private void CleanupPlaceholder()
    {
        if (currentPlaceholder != null)
        {
            DayColumn col = currentPlaceholder.transform.parent != null ? currentPlaceholder.transform.parent.GetComponent<DayColumn>() : null;
            transform.SetParent(currentPlaceholder.transform.parent, false);
            transform.SetSiblingIndex(currentPlaceholder.transform.GetSiblingIndex());

            Destroy(currentPlaceholder);
            currentPlaceholder = null;

            if (col != null)
            {
                col.RefreshLayout();
            }
        }
    }

    private void Start()
    {
        postIt = GameObject.FindWithTag("Post-It").GetComponent<TextMeshProUGUI>();
    }
}
