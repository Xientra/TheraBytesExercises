using UnityEngine;

public class PlayReversi : MonoBehaviour
{
    void Start()
    {
        int i = 0b0001;
    }

    public void Play()
    {
        Solution.Main();
    }
}
