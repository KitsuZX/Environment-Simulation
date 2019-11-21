using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct GenesData 
{
    public float variance; //Porcentaje de variacion de un stat

    public float maxEnergy;
    public float maxHydration;
    public float speed;
    public float childCountMean;
    public float perceptionRadius;
    public float gestationPeriodLength;

    public void ApplyRandomVariance()
    {
        this.maxEnergy += RandomVariation(this.maxEnergy, this.variance);
        this.maxHydration += RandomVariation(this.maxHydration, this.variance);
        this.speed += RandomVariation(this.speed, this.variance);
        this.childCountMean += RandomVariation(this.childCountMean, this.variance);
        this.perceptionRadius+= RandomVariation(this.perceptionRadius, this.variance);
        this.gestationPeriodLength += RandomVariation(this.gestationPeriodLength, this.variance);
    }

    public static GenesData Mix(GenesData a, GenesData b)
    {
        //Se calculan los genes base de cada hijo
        float meanVariance = Mean(a.variance, b.variance);
        float meanmaxEnergy = Mean(a.maxEnergy, b.maxEnergy);
        float meanmaxHydration = Mean(a.maxHydration, b.maxHydration);
        float meanspeed = Mean(a.speed, b.speed);
        float meanchildCountMean = Mean(a.childCountMean, b.childCountMean);
        float meanperceptionRadius = Mean(a.perceptionRadius, b.perceptionRadius);
        float meangestationPeriodLength = Mean(a.gestationPeriodLength, b.gestationPeriodLength);

        GenesData childGenesData = new GenesData();
        childGenesData.variance = meanVariance;
        childGenesData.maxEnergy = meanmaxEnergy + RandomVariation(meanmaxEnergy, childGenesData.variance);
        childGenesData.maxHydration = meanmaxHydration + RandomVariation(meanmaxHydration, childGenesData.variance);
        childGenesData.speed = meanspeed + RandomVariation(meanspeed, childGenesData.variance);
        childGenesData.childCountMean = meanchildCountMean + RandomVariation(meanchildCountMean, childGenesData.variance);
        childGenesData.perceptionRadius = meanperceptionRadius + RandomVariation(meanperceptionRadius, childGenesData.variance);
        childGenesData.gestationPeriodLength = meangestationPeriodLength + RandomVariation(meangestationPeriodLength, childGenesData.variance);

        return childGenesData;
    }

    

    private static float Mean(float mother, float father)
    {
        return (mother + father) / 2;
    }

    private static float RandomVariation(float stat, float variance)
    {
        float random = Random.Range(-variance * stat, variance * stat);
        return random;
    }
}
