using UnityEngine;

public class ClockMovement : MonoBehaviour
{

    private Joycon joycon;

    [SerializeField]
    private float[] targetsPos = new float[3];

    [SerializeField]
    [Range(0.0f, 50.0f)]
    private float floatTollerance = 0.1f;

    private int currentIndex = 0;
    private int direction = 1;

    private void Start()
    {
        joycon = JoyconManager.Instance.j[0];
    }

    private void Update()
    {
        if (joycon == null)
        {
            return;
        }

        float currentZRotation = joycon.GetVector().eulerAngles.z;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, currentZRotation);

        while (currentZRotation > 180.0f)
        {
            currentZRotation -= 360.0f;
        }

        while (currentZRotation < -180.0f)
        {
            currentZRotation += 360.0f;
        }

        if(Mathf.Abs(currentZRotation - targetsPos[currentIndex]) <= floatTollerance)
        {
            currentIndex += direction;

            if (currentIndex == 0 || currentIndex == targetsPos.Length - 1)
            {
                direction *= -1;
            }
        }
    }
}
