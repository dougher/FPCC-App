using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Video;

[SerializeField]
public class ScrubBar : MonoBehaviour, IDragHandler, IPointerDownHandler
{
	public bool isProgressionBar = true;

	[SerializeField]
	private VideoPlayer videoPlayer;
	[SerializeField]
	private AudioSource audioSource;
	[SerializeField]
	//private RectTransform scrubCursor;

	private Image progress;

    private void Awake() {
		progress = GetComponent<Image>();
    }

    private void Update() {
		if (videoPlayer.frameCount > 0){
			if (isProgressionBar)
				progress.fillAmount = (float)videoPlayer.frame / (float)videoPlayer.frameCount;
			else
				progress.fillAmount = audioSource.volume;

		}
    }

	public void OnDrag(PointerEventData eventData) {
		TrySkip(eventData);


	}

	public void OnPointerDown(PointerEventData eventData) {
		TrySkip(eventData);
	}

	private void TrySkip(PointerEventData eventData) {
		Vector2 localPoint;
		if(RectTransformUtility.ScreenPointToLocalPointInRectangle(progress.rectTransform, eventData.position, null, out localPoint)){
			float pct = Mathf.InverseLerp(progress.rectTransform.rect.xMin, progress.rectTransform.rect.xMax, localPoint.x);
			SkipToPercent(pct);
		}
	}

	private void SkipToPercent(float pct) {
		if (isProgressionBar){
			var frame = videoPlayer.frameCount * pct;
			videoPlayer.frame = (long) frame;
		} else {
			audioSource.volume = pct;
		}

	}
}
