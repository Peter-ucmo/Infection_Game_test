using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CureZone : MonoBehaviour
{
    [SerializeField] ParticleSystem particleEmitter;
    [SerializeField] float targetRadius;
    [SerializeField] float initialGrowTime = 5f;
    [SerializeField] float timeBeforeShrinking = 10f;
    [SerializeField] float shrinkTime = 10f;

    float currentRadius = 0;

    //public UnityColliderEvent colliderEvent = new UnityColliderEvent();

    void Start()
    {
        StartCoroutine(LerpRadius(initialGrowTime, targetRadius));
        Invoke("shrinkRadius", initialGrowTime +  timeBeforeShrinking);
    }

    private IEnumerator LerpRadius(float targetSeconds, float targetRadius, bool destroyAfterLerp = false)
    {
        float t = 0f;
        float startRadius = currentRadius;

        while (t/targetSeconds < 1)
        {
            t += Time.deltaTime;
            t = Mathf.Clamp(t, 0f, targetSeconds);

            currentRadius = Mathf.Lerp(startRadius, targetRadius, t/targetSeconds);

            GetComponent<CircleCollider2D>().radius = currentRadius;
            var newShape = particleEmitter.shape;
            newShape.radius = currentRadius;

            yield return null;
        }

        if (destroyAfterLerp) { Destroy(gameObject); }

    }

    private void shrinkRadius()
    {
        StartCoroutine(LerpRadius(shrinkTime, 0f, true));
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Stay");
        //colliderEvent.Invoke(collision);
        Debug.Log(collision.gameObject);
        if (collision.gameObject.transform.parent.gameObject.GetComponent<PlayerController>() != null)
        {
            collision.gameObject.transform.parent.gameObject.GetComponent<PlayerController>().setInCureZone(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Exit");

        if (collision.gameObject.transform.parent.gameObject.GetComponent<PlayerController>() != null)
        {
            collision.gameObject.transform.parent.gameObject.GetComponent<PlayerController>().setInCureZone(false);
        }
    }

}