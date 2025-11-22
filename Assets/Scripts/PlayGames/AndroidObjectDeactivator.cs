using UnityEngine;

public class AndroidObjectDeactivator : MonoBehaviour
{
    private void Awake()
    {
#if !UNITY_ANDROID
        gameObject.SetActive(false);
#endif
    }
}
