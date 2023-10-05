using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "FPS/Footstep Audio Data")]
public class FootstepAudioData : ScriptableObject
{
    public List<FootstepAudio> FootstepAudios = new List<FootstepAudio>();
}
[System.Serializable]
public class FootstepAudio
{
    public string Tag;
    public float
        WalkDelay = 0.6f,
        SprintingDelay = 0.4f,
        CrouchingDelay = 0.7f,
        OthersDelay = 0.4f;
    public List<AudioClip> AudioClips = new List<AudioClip>();

}