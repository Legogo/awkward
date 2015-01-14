using UnityEngine;
using System.Collections;

public class NonUniformRandom {

  public static AnimationCurve distribution = null;
  // we approximate the distribution with samples
  private static float[] distribSamples;
  private static float maxProbaElementInDistrib;
  // how much samples do we take
	public static int distribSampleDensity = 30;
	private static float distribSum = 0;
  private static bool _normalizeProbabilities = true; // normalize proba curve so that sum of probabilities equals to 1

  public static bool normalizeProbabilities
  {
    get{ return _normalizeProbabilities; }
    set{ 
      _normalizeProbabilities = value;
      setDistribution(distribution);
    }
  }

	public static void setDistribution(AnimationCurve distrib){
    distribution = distrib;
    distribSamples = new float[distribSampleDensity];
    // compute sum of distribution
    float maxProbaElementInDistrib = 0;
    float maxProba = 0;
    for(int i=0; i<distribSampleDensity; i++){
      distribSamples[i] = distribution.Evaluate((float)i/(float)distribSampleDensity);
      distribSum += distribSamples[i];
      // remember element that has max probability in the distribution
      // as it will be returned when max iterations has been reached
      if(distribSamples[i] > maxProba)
      {
        maxProba = distribSamples[i];
        maxProbaElementInDistrib = maxProba / maxProbaElementInDistrib;
      }
    }

    if(_normalizeProbabilities)
    {
      // divide each sample by sum to get probabilities
      for(int i=0; i<distribSampleDensity; i++){
        distribSamples[i] /= distribSum;
      }
    }
  }

  /// <summary>
  /// Pick a random number in a weighted distribution
  /// </summary>
  /// <returns>
  /// A random number according to provided distribution of probabilities to appear
  /// </returns>
  public static float getValue(){
    float randomValue;
    float probaOfAppearing;
    float discardValue;
    int nbAttempts = 20;
    
    // if the accept value is below the probability of our random number to appear, accept it
    do{
      randomValue = Random.value;
      // generate a value to decide wether to discard or accept the number
      discardValue = Random.value;
      if(_normalizeProbabilities) discardValue /= distribSum;

      int idx = Mathf.FloorToInt(randomValue*distribSampleDensity);
      probaOfAppearing = distribSamples[idx];
      nbAttempts--;
    }while(discardValue > probaOfAppearing && nbAttempts > 0);

    if(nbAttempts == 0) return maxProbaElementInDistrib;
    return randomValue;
  }
  
  public static void TestDistribution(int nbNumbersToGenerate){

    // TEST the non uniform random generation
    int[] appearancedistribution = new int[distribSampleDensity];
    for(int i=0; i< nbNumbersToGenerate; i++){
      float nbGenerated = getValue();
      int index = Mathf.FloorToInt(nbGenerated*distribSampleDensity);
      appearancedistribution[index]++;
    }

    string resultDistrib = "[NonUniformGenerator > distribution test\n";
    for(int i=0; i<appearancedistribution.Length; i++)
    {
      resultDistrib += "("+((float)i/distribSampleDensity)+")\t\t[theoretical distrib:"+distribSamples[i]+" -> distrib in test:"+((float)appearancedistribution[i]/(float)nbNumbersToGenerate)+" (occurences:"+appearancedistribution[i]+")]\n";
    }
    resultDistrib += "]";
    Debug.Log(resultDistrib);
  }
}
