using UnityEngine;
using System.Collections;

public class NonUniformRandom {

  public static AnimationCurve distribution = null;
  // we approximate the distribution with samples
  private static float[] distribSamples;
  // how much samples do we take
	private static int distribSampleDensity = 30;
	private static float distribSum = 0;

	public static void setDistribution(AnimationCurve distrib){
    distribution = distrib;
    distribSamples = new float[distribSampleDensity];
    // compute sum of distribution
    for(int i=0; i<distribSampleDensity; i++){
      distribSamples[i] = distribution.Evaluate((float)i/(float)distribSampleDensity);
      distribSum += distribSamples[i];
    }
    // divide each sample by sum to get probabilities
    for(int i=0; i<distribSampleDensity; i++){
      distribSamples[i] /= distribSum;
    }
  }

  /// <summary>
  /// Pick a random number in a weighted distribution
  /// </summary>
  /// <returns>
  /// A random number according to provided distribution of probabilities to appear
  /// </returns>
  public static float getValue(){
    // generate a random number from 0 to 1
    float randomValue = Random.value;
    int idx = Mathf.FloorToInt(randomValue*distribSampleDensity);
    float probaOfAppearing = distribSamples[idx];
    
    // generate a value to decide wether to discard or accept the number
    float discardValue = Random.value/distribSum;
    int nbAttempts = 20;
    
    // if the accept value is below the probability of our random number to appear, accept it
    while(discardValue > probaOfAppearing && nbAttempts > 0){
      randomValue = Random.value;
      discardValue = Random.value/distribSum;
      idx = Mathf.FloorToInt(randomValue*distribSampleDensity);
      probaOfAppearing = distribSamples[idx];
    }
    
    return randomValue;
  }
  
  /*void TestDistribution(int nbNumbersToGenerate){
    string probasDistribStr = "[";
    foreach(float proba in distribSamples) probasDistribStr += ""+proba+", ";
    probasDistribStr += "]";
    
    Debug.Log("NonUniformGenerator > TESTING distribution: "+probasDistribStr);
    
    // TEST the non uniform random generation
    int[] appearancedistribution = new int[distribSampleDensity];
    for(int i=0; i< nbNumbersToGenerate; i++){
      float nbGenerated = getValue();
      int index = Mathf.FloorToInt(nbGenerated*distribSampleDensity);
      appearancedistribution[index]++;
    }
    string resultDistrib = "[";
    foreach(int appearanceCount in appearancedistribution) resultDistrib += ""+((float)appearanceCount/(float)nbNumbersToGenerate)+", ";
    resultDistrib += "]";
    Debug.Log("RESULT DISTRIBUTION: "+resultDistrib);
  }*/
}
