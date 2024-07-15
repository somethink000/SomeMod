using System;

namespace GeneralGame;
public static class RandomExtensions
{
	public static float NextFloat(
		this Random random,
		float minValue,
		float maxValue )
	{
		return random.Float() * (maxValue - minValue) + minValue;
	}
}
