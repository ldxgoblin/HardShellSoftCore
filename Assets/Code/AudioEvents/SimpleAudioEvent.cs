using UnityEngine;

[CreateAssetMenu(menuName = "Audio Events/Simple")]
public class SimpleAudioEvent : AudioEvent
{
    public AudioClip[] clips;

    [MinMaxRange(0, 1)] public RangedFloat volumeRange;

    [MinMaxRange(0, 2)] public RangedFloat pitchRange;

    public override void Play(AudioSource source)
    {
        if (clips.Length == 0) return;

        //source.clip = clips[Random.Range(0, clips.Length)];
        source.volume = Random.Range(volumeRange.minValue, volumeRange.maxValue);
        source.pitch = Random.Range(pitchRange.minValue, pitchRange.maxValue);
        source.PlayOneShot(clips[Random.Range(0, clips.Length)]);
    }
}