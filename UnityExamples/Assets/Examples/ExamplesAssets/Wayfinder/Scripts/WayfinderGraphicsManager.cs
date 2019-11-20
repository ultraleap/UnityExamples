using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class WayfinderGraphicsManager : MonoBehaviour {

    public PlayableDirector YouAreHereTimeline;
    public GameObject YouAreHereObject;
	public GameObject RoomListView;
	public GameObject RestaurantListView;

	public CanvasGroup medicalListViewCanvasGroup;

	public CanvasGroup restaurantListViewCanvasGroup;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Played back when the hand becomes present.
    public void AnimateInYouAreHere()
    {
        YouAreHereObject.SetActive(true);
        YouAreHereTimeline.Play();
    }

    // Played back when the hand becomes present.
    public void StopAnimatingYouAreHere()
    {
        YouAreHereTimeline.Stop();
        YouAreHereObject.SetActive(false);
    }

    // When the Hospital Room mode is enabled, show the listview
    public void SetRoomListViewEnabled(bool enabled)
    {
		restaurantListViewCanvasGroup.alpha = 0.0f;
		medicalListViewCanvasGroup.alpha = enabled ? 1.0f : 0.0f;
    }

	// When the Hospital Room mode is enabled, show the listview
	public void SetRestaurantListViewEnabled(bool enabled)
	{
		medicalListViewCanvasGroup.alpha = 0.0f;
		restaurantListViewCanvasGroup.alpha = enabled ? 1.0f : 0.0f;
	}

	//IEnumerator FadeImage(bool fadeAway)
 //   {
 //       // fade from opaque to transparent
 //       if (!fadeAway)
 //       {
 //           // loop over 1 second backwards
 //           for (float i = 1; i >= 0; i -= Time.deltaTime)
 //           {
	//			// set with i as alpha
	//			medicalListViewCanvasGroup.alpha = i;
 //               yield return null;
 //           }
 //       }
 //       // fade from transparent to opaque
 //       else
 //       {
 //           // loop over 1 second
 //           for (float i = 0; i <= 1; i += Time.deltaTime)
 //           {
 //               // set color with i as alpha
 //               listViewCanvasGroup.alpha = i;
 //               yield return null;
 //           }
 //       }
 //   }
}
