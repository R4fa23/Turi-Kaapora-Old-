using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineController : MonoBehaviour
{
    public void PlayTimeline(PlayableDirector Director)
    {
        Director.Play();
    }
}
