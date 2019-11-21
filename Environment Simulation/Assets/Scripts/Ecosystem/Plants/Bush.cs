using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour, IEatable
{
    [Range(0, 100)]
    [SerializeField] private float maxFoodAmount = 100f;
    [SerializeField] private float foodGainedPerSecond = 10f;
    [SerializeField] private float actualFoodAmount;

    [SerializeField] private float perceptableFoodAmount = 15f;

    [Header("Animation")]
    [SerializeField, Range(0, 1)] private float minScale = 0.5f;
    [SerializeField, Range(0, 1)] private float maxScale = 1;
    [SerializeField] private float eatAnimationLength;
    [SerializeField] private AnimationCurve eatAnimationCurve;

    public bool IsAvailableToEat => this && actualFoodAmount > perceptableFoodAmount;
    public Vector3 Position => transform.position;

    private Coroutine eatAnimationCoroutine;
    private float desiredScale;

    public float Eat(float requestedEnergy)
    {
        float foodRetrieved = actualFoodAmount - requestedEnergy;
        foodRetrieved = Mathf.Clamp(foodRetrieved, 0, requestedEnergy);

        actualFoodAmount = foodRetrieved;

        StopAllCoroutines();
        eatAnimationCoroutine = StartCoroutine(_EatenAnimation());

        return foodRetrieved;
    }

    private IEnumerator _EatenAnimation()
    {
        float t = 0;
        float dt;

        float topValue = transform.localScale.x;

        while (t < 1)
        {
            dt = Time.deltaTime / eatAnimationLength;
            t += dt;
            t = Mathf.Min(t, 1);

            transform.localScale = Vector3.one * Mathf.LerpUnclamped(desiredScale, topValue, eatAnimationCurve.Evaluate(t));

            yield return null;
        }

        eatAnimationCoroutine = null;
    }




    private void Awake()
    {
        actualFoodAmount = maxFoodAmount;
    }

    

    private void FixedUpdate()
    {
        if(actualFoodAmount < maxFoodAmount)
        {
            actualFoodAmount += foodGainedPerSecond * Time.fixedDeltaTime;
            actualFoodAmount = Mathf.Clamp(actualFoodAmount, 0, maxFoodAmount);

            desiredScale = Mathf.Lerp(minScale, maxScale, actualFoodAmount / maxFoodAmount);

            if (eatAnimationCoroutine == null)
            {
                transform.localScale = Vector3.one * desiredScale;
            }
        }
    }

}
