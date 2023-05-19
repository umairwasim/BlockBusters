using UnityEngine;
using TMPro;
using DG.Tweening;

public class FeedbackPopUp : MonoBehaviour
{
    [SerializeField] private TMP_Text feedbackText;

    public static FeedbackPopUp CreateFeedback(Vector3 position, string message)
    {
        //Instantiate feedback and set feedback message on it
        Transform feedbackTransform = Instantiate(UIManager.Instance.feedbackPopUp, position, Camera.main.transform.rotation);
        FeedbackPopUp feedbackPopUp = feedbackTransform.GetComponent<FeedbackPopUp>();
        feedbackPopUp.SetFeedback(message);

        float feedbackValue = 3f;
        //Tween at y position, with ease animation and on completion, deactivate gameObject
        feedbackTransform.DOScale(Vector3.one, 0.5f)
            .SetEase(Ease.InQuad)
            .OnComplete(() =>
            {
                feedbackTransform.DOMoveY(position.y + feedbackValue, feedbackValue)
                    .OnComplete(() => Destroy(feedbackTransform.gameObject));
            });

        return feedbackPopUp;
    }

    public void SetFeedback(int amount)
    {
        feedbackText.text = amount.ToString();
    }

    public void SetFeedback(string message)
    {
        feedbackText.text = message.ToString();
    }
}
