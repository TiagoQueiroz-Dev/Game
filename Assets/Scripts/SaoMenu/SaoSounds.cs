using UnityEngine;

// Sons do menu gerados por código (bipes cristalinos tipo o menu de SAO).
// Para usar os sons reais, arraste AudioClips nos campos do SaoMenu.
public static class SaoSounds
{
    private const int SampleRate = 44100;

    private static AudioClip open, close, click, equip, error;

    public static AudioClip Open => open ??= Chime("SAO Open", new[] { 1318.5f, 1760f, 2637f }, 0.055f, 0.35f);
    public static AudioClip Close => close ??= Chime("SAO Close", new[] { 2093f, 1568f }, 0.06f, 0.25f);
    public static AudioClip Click => click ??= Chime("SAO Click", new[] { 2349f }, 0f, 0.12f);
    public static AudioClip Equip => equip ??= Chime("SAO Equip", new[] { 1568f, 2349f }, 0.07f, 0.3f);
    public static AudioClip Error => error ??= Buzz("SAO Error", 196f, 0.22f);

    private static AudioClip Chime(string name, float[] notes, float noteGap, float decay)
    {
        float length = noteGap * (notes.Length - 1) + decay * 2.5f;
        int samples = Mathf.CeilToInt(length * SampleRate);
        float[] data = new float[samples];

        for (int n = 0; n < notes.Length; n++)
        {
            int start = Mathf.RoundToInt(n * noteGap * SampleRate);
            for (int i = start; i < samples; i++)
            {
                float t = (i - start) / (float)SampleRate;
                float env = Mathf.Exp(-t / decay) * Mathf.Clamp01(t * 400f);
                float s = Mathf.Sin(2f * Mathf.PI * notes[n] * t)
                          + 0.35f * Mathf.Sin(2f * Mathf.PI * notes[n] * 2f * t)
                          + 0.12f * Mathf.Sin(2f * Mathf.PI * notes[n] * 3.01f * t);
                data[i] += s * env * 0.22f;
            }
        }

        return Make(name, data);
    }

    private static AudioClip Buzz(string name, float freq, float length)
    {
        int samples = Mathf.CeilToInt(length * SampleRate);
        float[] data = new float[samples];
        for (int i = 0; i < samples; i++)
        {
            float t = i / (float)SampleRate;
            float env = Mathf.Clamp01(t * 200f) * Mathf.Clamp01((length - t) * 30f);
            float s = Mathf.Sign(Mathf.Sin(2f * Mathf.PI * freq * t)) * 0.5f + Mathf.Sin(2f * Mathf.PI * freq * 2f * t) * 0.3f;
            data[i] = s * env * 0.15f;
        }
        return Make(name, data);
    }

    private static AudioClip Make(string name, float[] data)
    {
        AudioClip clip = AudioClip.Create(name, data.Length, 1, SampleRate, false);
        clip.SetData(data, 0);
        clip.hideFlags = HideFlags.DontUnloadUnusedAsset; // sobrevive à troca de cena
        return clip;
    }
}
