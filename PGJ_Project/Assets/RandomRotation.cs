using UnityEngine;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class RandomRotation : MonoBehaviour
{
    private void OnValidate()
    {
        foreach (Transform child in transform)
        {
            var randomYRotation = Random.Range(0f, 360f);

            child.localRotation = Quaternion.Euler(child.localRotation.eulerAngles.x, randomYRotation, child.localRotation.eulerAngles.z);
        }
    }
}