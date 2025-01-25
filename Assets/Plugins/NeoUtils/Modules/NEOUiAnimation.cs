// Author: Gabriel Barberiz - https://gabrielbarberiz.notion.site/
// Created: 2024/09/20

using UnityEngine;
using DG.Tweening; //Required for Animations

namespace NEO.UiAnimations
{
    public static class NEOUiAnimation
    {
        #region EntranceAnimations
        ///<summary> 
        /// Creates a bounce-in animation for a RectTransform using DOTween, applying a fade-in effect with CanvasGroup and a scale transformation.
        ///</summary>
        ///<param name = "duration" > The duration of the animation.</param>
        public static void NEOBounceIn(this RectTransform rectTransform,float duration)
        {
            if (rectTransform == null) return;  // Ensure the object exists before starting the animation

            CanvasGroup canvasGroup = CheckCanvasGroup(rectTransform);
            Vector2 scale = rectTransform.localScale;
            Sequence sequence = DOTween.Sequence();

            sequence
            // Fade in the CanvasGroup alpha over half the duration
            .Join(DOVirtual.Float(0, 1, duration * 0.5f, (value) => canvasGroup.alpha = value))
            // Scale the RectTransform using an OutElastic easing
            .Join(rectTransform.DOScale(scale, duration).SetEase(Ease.OutElastic))
            // OnStart: Shrink the scale of the RectTransform by 20% at the beginning of the animation
            .OnStart(() => 
            { 
                rectTransform.localScale *= 0.8f; 
            })
            // OnUpdate: Continuously check if the RectTransform or CanvasGroup is destroyed during the animation
            .OnUpdate(() =>
            {
                if (rectTransform == null || canvasGroup == null)
                {
                    sequence.Kill();
                }
            })
            //OnComplete: Ensure CanvasGroup stays enabled after animation finishes
            .OnComplete(() =>
            {
                canvasGroup.enabled = true;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            });
        }
        ///<summary> 
        /// Creates a bounce-in reverse animation for a RectTransform using DOTween, starting from a scale of 1.2x and shrinking to the original size.
        ///</summary>
        ///<param name = "duration" > The duration of the animation.</param>
        public static void NEOBounceInReverse(this RectTransform rectTransform, float duration)
        {
            if (rectTransform == null) return;  // Ensure the object exists before starting the animation

            CanvasGroup canvasGroup = CheckCanvasGroup(rectTransform);
            Vector2 scale = rectTransform.localScale;
            Sequence sequence = DOTween.Sequence();

            sequence
            // Fade in the CanvasGroup alpha over half the duration
            .Join(DOVirtual.Float(0, 1, duration * 0.5f, (value) => canvasGroup.alpha = value))
            // Scale the RectTransform using an OutElastic easing, starting from 1.2x the original size
            .Join(rectTransform.DOScale(scale, duration).SetEase(Ease.OutElastic))
            // OnStart: Start with the RectTransform scale at 1.2x
            .OnStart(() =>
            {
                rectTransform.localScale *= 1.2f;
            })
            // OnUpdate: Continuously check if the RectTransform or CanvasGroup is destroyed during the animation
            .OnUpdate(() =>
            {
                if (rectTransform == null || canvasGroup == null)
                {
                    sequence.Kill();
                }
            })
            // OnComplete: Ensure CanvasGroup stays enabled after animation finishes
            .OnComplete(() =>
            {
                canvasGroup.enabled = true;
            });
        }
        ///<summary> 
        /// Creates a fade-in animation for a RectTransform using DOTween, applying a gradual increase in opacity with CanvasGroup.
        ///</summary>
        ///<param name = "duration" > The duration of the animation.</param>
        public static void NEOFadeIn(this RectTransform rectTransform, float duration)
        {
            if (rectTransform == null) return;  // Ensure the object exists before starting the animation

            CanvasGroup canvasGroup = CheckCanvasGroup(rectTransform);
            Sequence sequence = DOTween.Sequence();

            sequence
            // Fade in the CanvasGroup alpha over the duration
            .Join(DOVirtual.Float(0, 1, duration, (value) => canvasGroup.alpha = value))
            // OnUpdate: Continuously check if the RectTransform or CanvasGroup is destroyed during the animation
            .OnUpdate(() =>
            {
                if (rectTransform == null || canvasGroup == null)
                {
                    sequence.Kill();
                }
            })
            //OnComplete: Ensure CanvasGroup stays enabled after animation finishes
            .OnComplete(() =>
            {
                canvasGroup.enabled = true;
            });
        }
        ///<summary> 
        /// Creates a slide-down animation for a RectTransform using DOTween, starting from a position slightly above the initial position.
        ///</summary>
        ///<param name = "duration" > The duration of the animation.</param>
        public static void NEOSlideDownIn(this RectTransform rectTransform, float duration)
        {
            float rectHeight = rectTransform.rect.height * rectTransform.localScale.y;
            SlideIn(rectTransform, duration, new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + rectHeight * 0.5f));
        }
        ///<summary> 
        /// Creates a slide-down animation for a RectTransform using DOTween, starting from a position slightly below the initial position.
        ///</summary>
        ///<param name = "duration" > The duration of the animation.</param>
        public static void NEOSlideUpIn(this RectTransform rectTransform, float duration)
        {
            float rectHeight = - rectTransform.rect.height * rectTransform.localScale.y;
            SlideIn(rectTransform, duration, new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + rectHeight * 0.5f));
        }
        ///<summary>
        /// Creates a slide-left animation for a RectTransform using DOTween, starting from a position slightly to the right of the initial position.
        ///</summary>
        ///<param name = "duration" > The duration of the animation.</param>
        public static void NEOSlideLeftIn(this RectTransform rectTransform, float duration)
        {
            float rectWidth = rectTransform.rect.width * rectTransform.localScale.x;
            SlideIn(rectTransform, duration, new Vector2(rectTransform.anchoredPosition.x + rectWidth * 0.5f, rectTransform.anchoredPosition.y));
        }
        ///<summary>
        /// Creates a slide-right animation for a RectTransform using DOTween, starting from a position slightly to the left of the initial position.
        ///</summary>
        ///<param name = "duration" > The duration of the animation.</param>
        public static void NEOSlideRightIn(this RectTransform rectTransform, float duration)
        {
            float rectWidth = - rectTransform.rect.width * rectTransform.localScale.x;
            SlideIn(rectTransform, duration, new Vector2(rectTransform.anchoredPosition.x + rectWidth * 0.5f,rectTransform.anchoredPosition.y));
        }
        ///<summary> 
        /// Creates a big slide-down animation for a RectTransform using DOTween, starting from the top edge of the canvas.
        ///</summary>
        ///<param name="duration">The duration of the animation.</param>
        public static void NEOBigSlideDownIn(this RectTransform rectTransform, float duration)
        {
            if (rectTransform == null) return;  // Ensure the RectTransform exists before starting the animation

            Canvas canvas = rectTransform.GetComponentInParent<Canvas>();
            if (canvas == null)
            {
                Debug.LogError("RectTransform is not inside a Canvas.");
                return;
            }

            RectTransform canvasRect = canvas.GetComponent<RectTransform>();

            float rectHeight = rectTransform.rect.height * rectTransform.localScale.y;

            // Calculate the offset to move the RectTransform completely above the canvas view
            float offset = canvasRect.rect.height * (1 - rectTransform.anchorMin.y) + rectTransform.pivot.y * rectHeight;

            SlideIn(rectTransform, duration, new Vector2(rectTransform.anchoredPosition.x,offset));
        }
        ///<summary> 
        /// Creates a big slide-up animation for a RectTransform using DOTween, starting from the bottom edge of the canvas.
        ///</summary>
        ///<param name="duration">The duration of the animation.</param>
        public static void NEOBigSlideUpIn(this RectTransform rectTransform, float duration)
        {
            if (rectTransform == null) return;  // Ensure the RectTransform exists before starting the animation

            Canvas canvas = rectTransform.GetComponentInParent<Canvas>();
            if (canvas == null)
            {
                Debug.LogError("RectTransform is not inside a Canvas.");
                return;
            }

            RectTransform canvasRect = canvas.GetComponent<RectTransform>();

            float rectHeight = rectTransform.rect.height * rectTransform.localScale.y;

            // Calculate the offset to move the RectTransform completely below the canvas view
            float offset = canvasRect.rect.height * rectTransform.anchorMax.y + (1 - rectTransform.pivot.y) * rectHeight;

            SlideIn(rectTransform, duration, new Vector2(rectTransform.anchoredPosition.x, - offset));
        }
        ///<summary> 
        /// Creates a big slide-left animation for a RectTransform using DOTween, starting from the right edge of the canvas.
        ///</summary>
        ///<param name="duration">The duration of the animation.</param>
        public static void NEOBigSlideLeftIn(this RectTransform rectTransform, float duration)
        {
            if (rectTransform == null) return;  // Ensure the RectTransform exists before starting the animation

            Canvas canvas = rectTransform.GetComponentInParent<Canvas>();
            if (canvas == null)
            {
                Debug.LogError("RectTransform is not inside a Canvas.");
                return;
            }

            RectTransform canvasRect = canvas.GetComponent<RectTransform>();

            float rectWidth = rectTransform.rect.width * rectTransform.localScale.x;

            float offset = canvasRect.rect.width * rectTransform.anchorMax.x + (1 - rectTransform.pivot.x) * rectWidth;

            SlideIn(rectTransform, duration, new Vector2(- offset,rectTransform.anchoredPosition.y));
        }
        ///<summary> 
        /// Creates a big slide-right animation for a RectTransform using DOTween, starting from the left edge of the canvas.
        ///</summary>
        ///<param name="duration">The duration of the animation.</param>
        public static void NEOBigSlideRightIn(this RectTransform rectTransform, float duration)
        {
            if (rectTransform == null) return;  // Ensure the RectTransform exists before starting the animation

            Canvas canvas = rectTransform.GetComponentInParent<Canvas>();
            if (canvas == null)
            {
                Debug.LogError("RectTransform is not inside a Canvas.");
                return;
            }

            RectTransform canvasRect = canvas.GetComponent<RectTransform>();

            float rectWidth = rectTransform.rect.width * rectTransform.localScale.x;

            float offset = canvasRect.rect.width * (1 - rectTransform.anchorMin.x) + rectTransform.pivot.x * rectWidth;

            SlideIn(rectTransform, duration, new Vector2(offset, rectTransform.anchoredPosition.y));
        }
        #endregion

        #region ExitAnimations
        ///<summary> 
        /// Creates a bounce-out reverse animation for a RectTransform using DOTween, applying a fade-out effect with CanvasGroup and a scale transformation.
        ///</summary>
        ///<param name = "duration" > The duration of the animation.</param>
        public static void NEOBounceOutReverse(this RectTransform rectTransform, float duration)
        {
            if (rectTransform == null) return;  // Ensure the object exists before starting the animation

            Vector2 originalScale = rectTransform.localScale;
            Sequence sequence = DOTween.Sequence();
            CanvasGroup canvasGroup = CheckCanvasGroup(rectTransform);

            sequence
            // Scale the RectTransform using an InElastic easing
            // Step 1: Scale to 0.9 at 20% of the total duration
            .Append(rectTransform.DOScale(originalScale * 1.1f, duration * 0.1f).SetEase(Ease.Linear))

            // Step 2: Scale up to 1.1 and hold the opacity at 1 (50%-55% of the total duration)
            .Append(rectTransform.DOScale(originalScale * 0.8f, duration * 0.4f).SetEase(Ease.Linear))

            // Step 3: Fade out the CanvasGroup alpha from 1 to 0 and shrink the RectTransform to 0.3
            .Join(rectTransform.DOScale(originalScale * 1.7f, duration * 0.5f).SetEase(Ease.InBack))

            .Join(DOVirtual.Float(1, 0, duration * 0.5f, value => canvasGroup.alpha = value))

            // OnUpdate: Continuously check if the RectTransform or CanvasGroup is destroyed during the animation
            .OnUpdate(() =>
            {
                if (rectTransform == null)
                {
                    sequence.Kill();
                }
            })
            //OnComplete: Ensure reset local scale
            .OnComplete(() =>
            {
                canvasGroup.alpha = 0;
                rectTransform.localScale = originalScale;
            });
        }
        ///<summary> 
        /// Creates a bounce-out animation for a RectTransform using DOTween, applying a fade-out effect with CanvasGroup and a scale transformation.
        ///</summary>
        ///<param name = "duration" > The duration of the animation.</param>
        public static void NEOBounceOut(this RectTransform rectTransform, float duration)
        {
            if (rectTransform == null) return;  // Ensure the object exists before starting the animation

            Vector2 originalScale = rectTransform.localScale;
            Sequence sequence = DOTween.Sequence();
            CanvasGroup canvasGroup = CheckCanvasGroup(rectTransform);

            sequence
            // Scale the RectTransform using an InElastic easing
            // Step 1: Scale to 0.9 at 20% of the total duration
            .Append(rectTransform.DOScale(originalScale * 0.9f, duration * 0.1f).SetEase(Ease.Linear))

            // Step 2: Scale up to 1.1 and hold the opacity at 1 (50%-55% of the total duration)
            .Append(rectTransform.DOScale(originalScale * 1.2f, duration * 0.4f).SetEase(Ease.Linear))

            // Step 3: Fade out the CanvasGroup alpha from 1 to 0 and shrink the RectTransform to 0.3
            .Join(rectTransform.DOScale(originalScale * 0.3f, duration * 0.5f).SetEase(Ease.InBack))

            .Join(DOVirtual.Float(1, 0, duration * 0.5f, value => canvasGroup.alpha = value))

            // OnUpdate: Continuously check if the RectTransform or CanvasGroup is destroyed during the animation
            .OnUpdate(() =>
            {
                if (rectTransform == null)
                {
                    sequence.Kill();
                }
            })
            //OnComplete: Ensure reset local scale
            .OnComplete(() =>
            {
                canvasGroup.alpha = 0;
                rectTransform.localScale = originalScale;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            });
        }
        ///<summary> 
        /// Creates a fade-out animation for a RectTransform using DOTween, applying a gradual increase in opacity with CanvasGroup.
        ///</summary>
        ///<param name = "duration" > The duration of the animation.</param>
        public static void NEOFadeOut(this RectTransform rectTransform, float duration)
        {
            if (rectTransform == null) return;  // Ensure the object exists before starting the animation

            CanvasGroup canvasGroup = CheckCanvasGroup(rectTransform);
            Sequence sequence = DOTween.Sequence();

            sequence
            // Fade out the CanvasGroup alpha over the duration
            .Join(DOVirtual.Float(1, 0, duration, (value) => canvasGroup.alpha = value))
            // OnUpdate: Continuously check if the RectTransform or CanvasGroup is destroyed during the animation
            .OnUpdate(() =>
            {
                if (rectTransform == null || canvasGroup == null)
                {
                    sequence.Kill();
                }
            })
            //OnComplete: Ensure CanvasGroup stays enabled after animation finishes
            .OnComplete(() =>
            {
                canvasGroup.enabled = true;
            });
        }
        ///<summary> 
        /// Creates a slide-down animation for a RectTransform using DOTween, ending at a position slightly below the initial position.
        ///</summary>
        ///<param name = "duration" > The duration of the animation.</param>
        public static void NEOSlideDownOut(this RectTransform rectTransform, float duration)
        {
            float rectHeight = -1 * rectTransform.rect.height * rectTransform.localScale.y;
            SlideOut(rectTransform, duration, new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + rectHeight * 0.5f));
        }
        ///<summary> 
        /// Creates a slide-up animation for a RectTransform using DOTween, ending at a position slightly above the initial position.
        ///</summary>
        ///<param name = "duration" > The duration of the animation.</param>
        public static void NEOSlideUpOut(this RectTransform rectTransform, float duration)
        {
            float rectHeight = rectTransform.rect.height * rectTransform.localScale.y;
            SlideOut(rectTransform, duration, new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + rectHeight * 0.5f));
        }
        ///<summary> 
        /// Creates a slide-right animation for a RectTransform using DOTween, ending at a position slightly to the right of the initial position.
        ///</summary>
        ///<param name="duration">The duration of the animation.</param>
        public static void NEOSlideRightOut(this RectTransform rectTransform, float duration)
        {
            float rectWidth = rectTransform.rect.width * rectTransform.localScale.x;
            SlideOut(rectTransform, duration, new Vector2(rectTransform.anchoredPosition.x + rectWidth * 0.5f, rectTransform.anchoredPosition.y));
        }
        ///<summary> 
        /// Creates a slide-left animation for a RectTransform using DOTween, ending at a position slightly to the left of the initial position.
        ///</summary>
        ///<param name="duration">The duration of the animation.</param>
        public static void NEOSlideLeftOut(this RectTransform rectTransform, float duration)
        {
            float rectWidth = - rectTransform.rect.width * rectTransform.localScale.x;
            SlideOut(rectTransform, duration, new Vector2(rectTransform.anchoredPosition.x + rectWidth * 0.5f, rectTransform.anchoredPosition.y));
        }
        ///<summary> 
        /// Creates a big slide-down animation for a RectTransform using DOTween, moving it far below the screen.
        ///</summary>
        ///<param name = "duration" > The duration of the animation.</param>
        public static void NEOBigSlideDownOut(this RectTransform rectTransform, float duration)
        {
            if (rectTransform == null) return;  // Ensure the RectTransform exists before starting the animation

            Canvas canvas = rectTransform.GetComponentInParent<Canvas>();
            if (canvas == null)
            {
                Debug.LogError("RectTransform is not inside a Canvas.");
                return;
            }

            RectTransform canvasRect = canvas.GetComponent<RectTransform>();

            float rectHeight = rectTransform.rect.height * rectTransform.localScale.y;

            // Calculate the offset to move the RectTransform completely below the canvas view
            float offset = canvasRect.rect.height * rectTransform.anchorMax.y + (1 - rectTransform.pivot.y) * rectHeight;

            SlideOut(rectTransform, duration, new Vector2(rectTransform.anchoredPosition.x, - offset));
        }
        ///<summary> 
        /// Creates a big slide-up animation for a RectTransform using DOTween, moving it far above the screen.
        ///</summary>
        ///<param name = "duration" > The duration of the animation.</param>
        public static void NEOBigSlideUpOut(this RectTransform rectTransform, float duration)
        {
            if (rectTransform == null) return;  // Ensure the RectTransform exists before starting the animation

            Canvas canvas = rectTransform.GetComponentInParent<Canvas>();
            if (canvas == null)
            {
                Debug.LogError("RectTransform is not inside a Canvas.");
                return;
            }

            RectTransform canvasRect = canvas.GetComponent<RectTransform>();

            float rectHeight = rectTransform.rect.height * rectTransform.localScale.y;

            // Calculate the offset to move the RectTransform completely above the canvas view
            float offset = canvasRect.rect.height * (1 - rectTransform.anchorMin.y) + rectTransform.pivot.y * rectHeight;

            SlideOut(rectTransform, duration, new Vector2(rectTransform.anchoredPosition.x, offset));
        }
        ///<summary> 
        /// Creates a big slide-right animation for a RectTransform using DOTween, moving it far to the right of the screen.
        ///</summary>
        ///<param name="duration">The duration of the animation.</param>
        public static void NEOBigSlideRightOut(this RectTransform rectTransform, float duration)
        {
            if (rectTransform == null) return;  // Ensure the RectTransform exists before starting the animation

            Canvas canvas = rectTransform.GetComponentInParent<Canvas>();
            if (canvas == null)
            {
                Debug.LogError("RectTransform is not inside a Canvas.");
                return;
            }

            RectTransform canvasRect = canvas.GetComponent<RectTransform>();

            float rectWidth = rectTransform.rect.width * rectTransform.localScale.x;

            float offset = canvasRect.rect.width * (1 - rectTransform.anchorMin.x) + rectTransform.pivot.x * rectWidth;

            SlideOut(rectTransform, duration, new Vector2(offset, rectTransform.anchoredPosition.y));
        }
        ///<summary> 
        /// Creates a big slide-left animation for a RectTransform using DOTween, moving it far to the left of the screen.
        ///</summary>
        ///<param name="duration">The duration of the animation.</param>
        public static void NEOBigSlideLeftOut(this RectTransform rectTransform, float duration)
        {
            if (rectTransform == null) return;  // Ensure the RectTransform exists before starting the animation

            Canvas canvas = rectTransform.GetComponentInParent<Canvas>();
            if (canvas == null)
            {
                Debug.LogError("RectTransform is not inside a Canvas.");
                return;
            }

            RectTransform canvasRect = canvas.GetComponent<RectTransform>();

            float rectWidth = rectTransform.rect.width * rectTransform.localScale.x;

            // Calculate the offset to move the RectTransform completely above the canvas view
            float offset = canvasRect.rect.width * rectTransform.anchorMax.x + (1 - rectTransform.pivot.x) * rectWidth;

            SlideOut(rectTransform, duration, new Vector2(- offset, rectTransform.anchoredPosition.y));
        }
        #endregion

        #region AttentionAnimations
        ///<summary> 
        /// Creates a shake animation for a RectTransform using DOTween, shaking it horizontally to draw attention,  useful for feedback like taking damage or other alert mechanisms.
        ///</summary> 
        ///<param name="duration">The duration of the animation.</param>
        ///<param name="strength">The strength of the shake effect.</param>
        public static void NEOShakeX(this RectTransform rectTransform, float duration = 0.2f,float strength = 20f)
        {
            if (rectTransform == null) return;  // Ensure the RectTransform exists before starting the animation

            rectTransform
                .DOShakeAnchorPos(duration, new Vector2(strength, 0), vibrato: 200, randomness: 0)
                .SetEase(Ease.OutQuad);
        }
        ///<summary> 
        /// Creates a shake animation for a RectTransform using DOTween, shaking it vertically to draw attention, useful for feedback like taking damage or other alert mechanisms.
        ///</summary> 
        ///<param name="duration">The duration of the animation.</param>
        ///<param name="strength">The strength of the shake effect.</param>
        public static void NEOShakeY(this RectTransform rectTransform, float duration = 0.2f, float strength = 20f)
        {
            if (rectTransform == null) return;  // Ensure the RectTransform exists before starting the animation

            rectTransform
                .DOShakeAnchorPos(duration, new Vector2(0, strength), vibrato: 200, randomness: 0)
                .SetEase(Ease.OutQuad);
        }
        ///<summary> 
        /// Creates a shake animation for a RectTransform using DOTween, shaking randomly to draw attention, useful for feedback like taking damage or other alert mechanisms.
        ///</summary> 
        ///<param name="duration">The duration of the animation.</param>
        ///<param name="strength">The strength of the shake effect.</param>
        public static void NEORandomShake(this RectTransform rectTransform,float duration = 0.2f,float strength = 20f)
        {
            if (rectTransform == null) return;  // Ensure the RectTransform exists before starting the animation

            rectTransform
                .DOShakeAnchorPos(duration, strength,vibrato: 200)
                .SetEase(Ease.OutQuad);
        }
        #endregion

        #region InternalUtils
        private static void SlideIn(RectTransform rectTransform, float duration,Vector2 startPosition)
        {
            if (rectTransform == null) return;  // Ensure the object exists before starting the animation

            CanvasGroup canvasGroup = CheckCanvasGroup(rectTransform);
            Vector2 originalPosition = rectTransform.anchoredPosition;
            Sequence sequence = DOTween.Sequence();

            sequence
            // Add the fade-in animation over
            .Join(DOVirtual.Float(0, 1, duration , value => canvasGroup.alpha = value))
            // Add the slide-down animation to move the RectTransform back to its original position
            .Join(rectTransform.DOAnchorPos(originalPosition, duration).SetEase(Ease.OutBack,1.5f))
            // OnStart: Move the RectTransform to a position above the original position
            .OnStart(() =>
            {
                rectTransform.anchoredPosition = startPosition;
            })
            // OnUpdate: Continuously check if the RectTransform or CanvasGroup is destroyed during the animation
            .OnUpdate(() =>
            {
                if (rectTransform == null || canvasGroup == null)
                {
                    sequence.Kill();
                }
            })
            //OnComplete: Ensure CanvasGroup stays enabled after animation finishes
            .OnComplete(() =>
            {
                canvasGroup.enabled = true;
            });
        }
        private static void SlideOut(RectTransform rectTransform, float duration, Vector2 endPosition)
        {
            if (rectTransform == null) return;  // Ensure the object exists before starting the animation

            CanvasGroup canvasGroup = CheckCanvasGroup(rectTransform);
            Vector2 originalPosition = rectTransform.anchoredPosition;
            Sequence sequence = DOTween.Sequence();

            sequence
            // Add the fade-in animation over
            .Join(DOVirtual.Float(1, 0, duration, value => canvasGroup.alpha = value))
            // Add the slide-down animation to move the RectTransform back to its original position
            .Join(rectTransform.DOAnchorPos(endPosition, duration).SetEase(Ease.InBack,1.5f))
            // OnUpdate: Continuously check if the RectTransform or CanvasGroup is destroyed during the animation
            .OnUpdate(() =>
            {
                if (rectTransform == null || canvasGroup == null)
                {
                    sequence.Kill();
                }
            })
            //OnComplete: Ensure CanvasGroup stays enabled after animation finishes
            .OnComplete(() =>
            {
                rectTransform.anchoredPosition = originalPosition;
                canvasGroup.enabled = true;
            });
        }
        private static CanvasGroup CheckCanvasGroup(RectTransform rectTransform)
        {
            CanvasGroup canvasGroup = rectTransform.GetComponent<CanvasGroup>();

            if (canvasGroup == null)
            {
                // If the CanvasGroup doesn't exist, add one to control the alpha (fade) of the RectTransform
                canvasGroup = rectTransform.gameObject.AddComponent<CanvasGroup>();
                Debug.LogWarning("CanvasGroup added to " + rectTransform.name);
            }

            canvasGroup.enabled = true;  // Make sure the CanvasGroup is active
            return canvasGroup;
        }
        #endregion
    }
}
