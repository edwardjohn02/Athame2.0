using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using UnityEngine;
using GameCreator.Runtime.VisualScripting;

[Version(1, 0, 1)]

[Title("Get Random Roaming Position")]
[Description("Finds a random position within a circular area, at a distance from character, and within an angle starting at player position.")]

[Category("Characters/Navigation/Get Random Roaming Position")]

[Parameter("Character", "The character game object")]
[Parameter("Circle Center", "The center of the circlular area")]
[Parameter("Circle Radius", "The radius of the circlular area")]
[Parameter("Min Distance", "The minimum distance from the Character")]
[Parameter("Max Distance", "The maximum distance from the Character")]
[Parameter("Angle", "The angle of the area in which to search for a position")]
[Parameter("Store Position", "The Vector3 in which to store the position")]

[Keywords("Character", "Roaming", "Position", "Random")]
[Image(typeof(IconVector3), ColorTheme.Type.Green)]

[Serializable]
public class InstructionGetRandomRoamingPosition : Instruction
{
	// MEMBERS: -------------------------------------------------------------------------------

    [SerializeField] private PropertyGetGameObject character = new PropertyGetGameObject();
    [SerializeField] private PropertyGetPosition circleCenter = new PropertyGetPosition();
  	[SerializeField] private PropertyGetDecimal circleRadius = new PropertyGetDecimal(30.0f);  
	[SerializeField] private PropertyGetDecimal minDistance = new PropertyGetDecimal(1.0f);
	[SerializeField] private PropertyGetDecimal maxDistance = new PropertyGetDecimal(10.0f);
	[SerializeField] private PropertyGetDecimal angle = new PropertyGetDecimal(90.0f);
    [SerializeField] private PropertySetVector3 storePosition;

	// PROPERTIES: ----------------------------------------------------------------------------

	public override string Title => string.Format("Get Random Roaming Position");

	// RUN METHOD: ----------------------------------------------------------------------------

	protected override Task Run(Args args)
	{
		GameObject goCharacter = character.Get(args);
		Vector3 vCircleCenter = circleCenter.Get(args);
		float fCircleRadius = (float)circleRadius.Get(args);
		float fMinDistance = (float)minDistance.Get(args);
		float fMaxDistance = (float)maxDistance.Get(args);
		float fAngle = (float)angle.Get(args);
		Vector3 vCharacterPosition = goCharacter.transform.position;
		Vector3 vCharacterForward = goCharacter.transform.forward;

		//is character in circle?
		if (Vector3.Distance(vCharacterPosition, vCircleCenter) > fCircleRadius)
		{
			Debug.Log("Roaming character is outside circle");
		}

		// draw a debug circle
		//DrawCircle(vCircleCenter, fCircleRadius);

		//in the character's direction starting at the character position is a wedge area with angle fAngle.
		//inside this wedge area we find a position between fMinDistance and fMaxDistance.
		Vector3 vRandomDirection = UnityEngine.Random.insideUnitCircle;
		vRandomDirection = new Vector3(vRandomDirection.x, 0f, vRandomDirection.y);
		float fRandomDistance = UnityEngine.Random.Range(fMinDistance, fMaxDistance);
		float fRandomAngle = UnityEngine.Random.Range(0f, fAngle);
		Vector3 vRandomPosition = vCharacterForward * fRandomDistance;
		vRandomPosition = Quaternion.AngleAxis(fRandomAngle, Vector3.up) * vRandomPosition;
		Vector3 randomPosition = vCharacterPosition + vRandomPosition;

		//if position is outside circle move to closest position inside
		if (Vector3.Distance(randomPosition, vCircleCenter) > fCircleRadius)
		{
			Vector3 direction = (randomPosition - vCircleCenter).normalized;
			randomPosition = vCircleCenter + direction * fCircleRadius;
		}

		// Debug.Log(randomPosition);
		storePosition.Set(randomPosition, args);

		return DefaultResult;
	}

	private void DrawCircle(Vector3 vCircleCenter, float fCircleRadius)
	{
	    int segments = 30;
	    float step = 360f / segments;
	    for (int i = 0; i <= segments; i++)
	    {
	        float angleA = step * i;
	        float angleB = step * (i + 1);
	        Vector3 pointA = GetCirclePoint(vCircleCenter, fCircleRadius, angleA);
	        Vector3 pointB = GetCirclePoint(vCircleCenter, fCircleRadius, angleB);
	        Debug.DrawLine(pointA, pointB, Color.red);
	    }
	}

	private Vector3 GetCirclePoint(Vector3 center, float radius, float angle)
	{
	    float x = center.x + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
	    float z = center.z + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
	    return new Vector3(x, 0, z);
	}
}