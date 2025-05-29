using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.Playables;

public class CinematicControlRemover : MonoBehaviour
{
    private PlayableDirector _director;
    private GameObject _player;
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _director = GetComponent<PlayableDirector>();
        _director.played += DirectorOnPlayed;
        _director.stopped += DirectorStopped;
    }

    private void DirectorStopped(PlayableDirector obj)
    {
        _player.GetComponent<PlayerController>().enabled = true;
    }

    private void DirectorOnPlayed(PlayableDirector obj)
    {
        _player.GetComponent<ActionScheduler>().CancleCurrentAction();
        _player.GetComponent<PlayerController>().enabled = false;
    }

}
