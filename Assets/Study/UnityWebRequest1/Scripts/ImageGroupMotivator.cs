using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageGroupMotivator : MonoBehaviour
{
    private int motivateTerm = 1;
    private float rotationTerm = 5f;
    private float startRotation;
    private float endRotation;
    private float time;

    private float moveDistance = 4f;
    private Vector3 basePosition;
    private float startPositionZ;
    private float endPositionZ;
    private bool movingRight = true;

    private float duration = 2f;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(motivateTerm);

        StartCoroutine(CustomRotateImageGrouop());
    }
    
    IEnumerator CustomRotateImageGrouop()
    {
        while (true)
        {
            endRotation = startRotation + 360.0f;
            startRotation = transform.localEulerAngles.y;
            time = 0.0f;

            basePosition = transform.localPosition;
            startPositionZ = basePosition.z;
            endPositionZ = startPositionZ + moveDistance;

            while (time < rotationTerm)
            {
                //rotate 360 degree
                float yRotation = Mathf.Lerp(startRotation, endRotation, time / duration) % 360.0f;
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, yRotation, transform.localEulerAngles.z);

                //Mover();
                time += Time.deltaTime;
                yield return null;
            }

            yield return rotationTerm;
        }
    }

    private void Mover()
    {
        if (movingRight)
        {
            float z = Mathf.Lerp(startPositionZ, endPositionZ, time / (duration / 2)) % moveDistance;
            transform.localPosition = new Vector3(basePosition.x, basePosition.y, z); 
            if (transform.localPosition.z >= 1f)
            {
                movingRight = false;
                Debug.Log(movingRight);
            }
        }
        else
        {
            float z = Mathf.Lerp(endPositionZ, startPositionZ, time / (duration  / 2)) % moveDistance;
            transform.localPosition = new Vector3(basePosition.x, basePosition.y, z);
            if (transform.localPosition.z <= 0f)
            {
                movingRight = true;
                Debug.Log(movingRight);
            }
        }
    }

    //called when awake function
    private void SetImageGroupMotivateStartTerm(int term) => motivateTerm = term;
}
