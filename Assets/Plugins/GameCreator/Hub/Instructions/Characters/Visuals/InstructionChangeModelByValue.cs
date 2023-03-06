using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using UnityEngine;
using UnityEngine.UI;
using GameCreator.Runtime.VisualScripting;
using System.Collections;
using System.Collections.Generic;
using GameCreator.Runtime.Characters;
using UnityEngine.Serialization;

namespace GameCreator.Runtime.VisualScripting
{
	[Version(0, 1, 3)]
	[Title("Change Character Model by Value")]
	[Category("Characters/Visuals/Change Model by Value")]
	[Keywords("Characters", "Model")]
	[Image(typeof(IconCharacter), ColorTheme.Type.Yellow)]
	[Description("Changes the Character current model from a list that correspond to a Name reference")] 

	[Serializable]
	public class InstructionChangeModelByValue : Instruction
	{
		[SerializeField] private PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();
		[SerializeField] private Skeleton m_Skeleton;
		[FormerlySerializedAs("m_FootstepSounds")] [SerializeField] private MaterialSoundsAsset m_MaterialSounds;
		[SerializeField] private Vector3 m_Offset = Vector3.zero;
	
		[SerializeField] private PropertyGetString referenceActivation = GetStringString.Create;
	
		[Tooltip("Set Names for Objects that are at same index in Object List")]
		[SerializeField] private List<string>referenceNameOfCharacter;	
	
		[SerializeField] private GameObject[] characterList;
	
		
		private bool objCanBeChange = true;
		private string objActivated ;
		private string m_referenceActivation;
		private int indexInList ;
	
		public override string Title => $"Change Model List = {this.referenceActivation} ";
	
		protected override Task Run(Args args)
		{
		
			m_referenceActivation = this.referenceActivation.Get(args);
			indexInList = referenceNameOfCharacter.IndexOf(m_referenceActivation);
		
			if(objActivated != m_referenceActivation && !string.IsNullOrEmpty(m_referenceActivation) )
			{
				objCanBeChange = true;
			}
			else objCanBeChange = false;
		
			/// /////////////////////////////////////////////////////////
			if(referenceNameOfCharacter.Contains(m_referenceActivation)  
				&&  !string.IsNullOrEmpty(m_referenceActivation) 
				&& objCanBeChange  )
			{
			
				Character character = this.m_Character.Get<Character>(args);
				if (character == null) return DefaultResult;

				GameObject model = characterList[indexInList];
				if (model == null) return DefaultResult;

				character.ChangeModel(model, new Character.ChangeOptions
				{
					skeleton = this.m_Skeleton,
					materials = this.m_MaterialSounds,
					offset = this.m_Offset
         });
			
			
				objActivated = referenceNameOfCharacter[indexInList];
				if(characterList[indexInList])
				{
					objCanBeChange = false;
				}
				
				//// ////////////////////////////////////////////////
			}
			return DefaultResult;
		}

	}
}
