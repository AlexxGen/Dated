using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DayColumn : MonoBehaviour
{
    [SerializeField] private bool isScrollable = false;
    [SerializeField] private float shiftTweenTime = 0.15f;
    [SerializeField] private LeanTweenType shiftEaseType = LeanTweenType.easeOutQuad;

    public float scrollZoneThreshold = 180f;
    public float scrollSpeed = 1800f;

    private LayoutElement layoutElement;
    private float baseMinHeight = 0f;

    private RectTransform rectTransform;
    private VerticalLayoutGroup layoutGroup;
    private ScrollRect scrollRect;
    private ContentSizeFitter contentSizeFitter;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        layoutGroup = GetComponent<VerticalLayoutGroup>();

        scrollRect = GetComponent<ScrollRect>(); 
        contentSizeFitter = GetComponent<ContentSizeFitter>();

        layoutElement = GetComponent<LayoutElement>();
        if (layoutElement != null)
        {
            baseMinHeight = layoutElement.minHeight;
        }

        ConfigureScrolling();
    }

    public void ConfigureScrolling()
    {
        if (scrollRect != null)
        {
            scrollRect.enabled = isScrollable;
        }

        if (contentSizeFitter != null)
        {
            contentSizeFitter.verticalFit = isScrollable ? ContentSizeFitter.FitMode.PreferredSize : ContentSizeFitter.FitMode.Unconstrained;
        }
    }

    public void SetScrollable(bool state)
    {
        isScrollable = state;
        ConfigureScrolling();
    }

    public void ExpandForAutoScroll(float extraHeight)
    {
        if (!isScrollable || layoutElement == null)
            return;

        RectTransform viewport = scrollRect != null && scrollRect.viewport != null
            ? scrollRect.viewport : transform.parent as RectTransform;

        float targetHeight = (viewport != null ? viewport.rect.height : baseMinHeight) + extraHeight;

        if (layoutElement.minHeight < targetHeight)
        {
            layoutElement.minHeight = targetHeight;
            RefreshLayout();
        }
    }

    public void ResetAutoScrollExpansion()
    {
        if (layoutElement != null)
        {
            layoutElement.minHeight = baseMinHeight;
            RefreshLayout();
        }
    }

    public void AnimateLayout(Transform ignoreTransform = null)
    {
        if (rectTransform == null) return;

        Dictionary<RectTransform, Vector3> startWorldPositions = new Dictionary<RectTransform, Vector3>();

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child == ignoreTransform)
                continue;

            RectTransform childRect = child.GetComponent<RectTransform>();
            if (childRect != null && child.gameObject.activeSelf)
            {
                startWorldPositions[childRect] = childRect.position;
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);

        foreach (var kvp in startWorldPositions)
        {
            RectTransform childRect = kvp.Key;
            if (childRect == null)
                continue;

            Vector3 startWorld = kvp.Value;
            Vector3 targetWorld = childRect.position;

            if (Vector3.Distance(startWorld, targetWorld) > 1f)
            {
                Vector3 offset = startWorld - targetWorld;

                LeanTween.cancel(childRect.gameObject);

                childRect.localPosition += offset;

                LeanTween.moveLocal(childRect.gameObject, childRect.localPosition - offset, shiftTweenTime)
                    .setEase(shiftEaseType);
            }
        }
    }

    public void RefreshLayout()
    {
        if (rectTransform != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        }
    }

    public bool CanFitBlock(float blockHeight, Transform ignoreTransform = null)
    {
        if (isScrollable)
            return true;

        float currentTopPadding = layoutGroup.padding.top;
        float currentBottomPadding = layoutGroup.padding.bottom;
        float currentSpacing = layoutGroup.spacing;

        float availableHeight = rectTransform.rect.height - (currentTopPadding + currentBottomPadding);

        int totalItemCount = 1;
        float totalHeight = blockHeight;

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            if (child == ignoreTransform)
                continue;

            RectTransform childRect = child.GetComponent<RectTransform>();
            if (childRect != null)
            {
                totalHeight += childRect.rect.height;
                totalItemCount++;
            }
        }

        if (totalItemCount > 1)
        {
            totalHeight += (totalItemCount - 1) * currentSpacing;
        }

        return totalHeight <= availableHeight;
    }

    public int DetermineInsertIndex(Vector2 mouseScreenPosition, Transform ignoreTransform = null)
    {
        List<RectTransform> validChildren = new List<RectTransform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child == ignoreTransform || !child.gameObject.activeSelf) continue;

            RectTransform childRect = child.GetComponent<RectTransform>();
            if (childRect != null)
            {
                validChildren.Add(childRect);
            }
        }

        if (validChildren.Count == 0) return 0;

        Canvas rootCanvas = GetComponentInParent<Canvas>()?.rootCanvas;
        Camera uiCamera = (rootCanvas != null && rootCanvas.renderMode == RenderMode.ScreenSpaceOverlay) ? null: rootCanvas?.worldCamera;

        Vector3 firstWorldCenter = validChildren[0].TransformPoint(validChildren[0].rect.center);
        Vector2 firstScreenCenter = RectTransformUtility.WorldToScreenPoint(uiCamera, firstWorldCenter);
        if (mouseScreenPosition.y > firstScreenCenter.y)
        {
            return validChildren[0].GetSiblingIndex();
        }

        for (int i = 0; i < validChildren.Count - 1; i++)
        {
            Vector3 currentWorld = validChildren[i].TransformPoint(validChildren[i].rect.center);
            Vector3 nextWorld = validChildren[i + 1].TransformPoint(validChildren[i + 1].rect.center);

            Vector2 currentScreen = RectTransformUtility.WorldToScreenPoint(uiCamera, currentWorld);
            Vector2 nextScreen = RectTransformUtility.WorldToScreenPoint(uiCamera, nextWorld);

            float midY = (currentScreen.y + nextScreen.y) / 2f;

            if (mouseScreenPosition.y > midY)
            {
                int targetIndex = validChildren[i + 1].GetSiblingIndex();

                if (ignoreTransform != null && ignoreTransform.parent == transform && ignoreTransform.GetSiblingIndex() < targetIndex)
                {
                    targetIndex--;
                }
                return targetIndex;
            }
        }

        int lastIndex = validChildren[validChildren.Count - 1].GetSiblingIndex();
        if (ignoreTransform != null && ignoreTransform.parent == transform && ignoreTransform.GetSiblingIndex() < lastIndex)
        {
            return lastIndex;
        }
        return lastIndex + 1;
    }

    public void ClearColumn()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);

            if (child.gameObject.activeSelf)
            {
                Destroy(child.gameObject);
            }
        }

        RefreshLayout();
    }

}
