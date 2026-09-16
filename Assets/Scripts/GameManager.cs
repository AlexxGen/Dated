using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public static readonly int MESSAGE_MOOD_POSITIVE_NEGATIVE_CUTOFF = 50;

    [SerializeField] private float academicStatP = 100;
    //[SerializeField] private int socialStatP;
    [SerializeField] private float extracurricularStatP = 100;
    [SerializeField] private float academicStatJ = 100;
    [SerializeField] private float socialStat = 100;
    [SerializeField] private float extracurricularStatJ = 100;
    [SerializeField] private float lerpDuration = 0.583f;

    public int curWeek;

    //Take the type and count how many of a certain type are still in the new blocks to determine what changes are needed for the stats
    public List<BlockData> newBlocksJ = new List<BlockData>();
    public List<BlockData> newBlocksP = new List<BlockData>();
    public List<BlockData> placedBlocksJ = new List<BlockData>();
    public List<BlockData> placedBlocksP = new List<BlockData>();

    [SerializeField] private int freeHoursP;
    [SerializeField] private int freeHoursJ;

    [SerializeField] private DayColumn[] spawnColumns;

    [SerializeField] private BlockGenerator blockGenerator;
    [SerializeField] private Image[] shaders;

    private void OnEnable()
    {
        BlockDrag.onBlockDropped += HandleBlockDropped;
    }

    private void OnDisable()
    {
        BlockDrag.onBlockDropped -= HandleBlockDropped;
    }

    private void HandleBlockDropped(DayColumn targetColumn, BlockData droppedBlock)
    {
        if (targetColumn != spawnColumns[0]  && targetColumn != spawnColumns[1] && (newBlocksJ.Contains(droppedBlock) || newBlocksP.Contains(droppedBlock)))
        {
            switch (droppedBlock.OwningCharacter)
            {
                case Character.PETER:
                    newBlocksP.Remove(droppedBlock);
                    break;
                case Character.JESSIE:
                    newBlocksJ.Remove(droppedBlock);
                    break;
            }
        }

        else if (spawnColumns != null)
        {
            //If does not work try doing it based on target column instead
            switch (droppedBlock.OwningCharacter)
            {
                case Character.PETER:
                    newBlocksP.Add(droppedBlock);
                    break;
                case Character.JESSIE:
                    newBlocksJ.Add(droppedBlock);
                    break;
            }
        }
    }

    public void StatCheck()
    {

        float newAcademicStatJ = academicStatJ;
        float newAcademicStatP = academicStatP;
        float newSocialStat = socialStat;
        float newExtraStatJ = extracurricularStatJ;
        float newExtraStatP = extracurricularStatP;

        if (curWeek == 4)
        {
            Endings();
            return;
        }

        foreach (BlockData block in newBlocksJ)
        {
            switch (block.Category)
            {
                case EventCategory.ACADEMIC:
                    newAcademicStatJ -= block.Length / 20;
                    break;
                case EventCategory.SOCIAL:
                    newSocialStat -= block.Length / 20;
                    break;
                case EventCategory.EXTRACURRICULAR:
                    newExtraStatJ -= block.Length / 20;
                    break;
            }
        }

        foreach (BlockData block in newBlocksP)
        {
            switch (block.Category)
            {
                case EventCategory.ACADEMIC:
                    newAcademicStatP -= block.Length / 40;
                    break;
                case EventCategory.SOCIAL:
                    newSocialStat -= block.Length / 40;
                    break;
                case EventCategory.EXTRACURRICULAR:
                    newExtraStatP -= block.Length / 40;
                    break;
            }
        }

        foreach (BlockData block in placedBlocksJ)
        {
            switch (block.Category)
            {
                case EventCategory.ACADEMIC:
                    newAcademicStatJ += block.Length / 60;
                    break;
                case EventCategory.SOCIAL:
                    newSocialStat += block.Length / 60;
                    break;
                case EventCategory.EXTRACURRICULAR:
                    newExtraStatJ += block.Length / 60;
                    break;
            }
        }

        foreach (BlockData block in placedBlocksP)
        {
            switch (block.Category)
            {
                case EventCategory.ACADEMIC:
                    newAcademicStatP += block.Length / 60;
                    break;
                case EventCategory.SOCIAL:
                    newSocialStat += block.Length / 60;
                    break;
                case EventCategory.EXTRACURRICULAR:
                    newExtraStatP += block.Length / 60;
                    break;
            }
        }

        curWeek++;
        StartCoroutine(ShaderLerp(newExtraStatJ, newExtraStatP, newSocialStat, newAcademicStatJ, newAcademicStatP));
        blockGenerator.SpawnBlock();

    }

    public void Endings()
    {

    }
	
	public int ScoreOfCategory(EventCategory category, Character character)
	{
		switch (category)
		{
			case EventCategory.SOCIAL:
				return Mathf.RoundToInt(socialStat);
			case EventCategory.ACADEMIC:
				switch (character) {
					case Character.JESSIE:
						return Mathf.RoundToInt(academicStatJ);
					case Character.PETER:
						return Mathf.RoundToInt(academicStatP);
				}
				break;
			case EventCategory.EXTRACURRICULAR:
				switch (character) {
					case Character.JESSIE:
						return Mathf.RoundToInt(extracurricularStatJ);
					case Character.PETER:
						return Mathf.RoundToInt(extracurricularStatP);
				}
				break;
		}
		throw new ArgumentException("Error: Char or event category not found");
	}
	
	public MessageMood MoodOfCategory(EventCategory category, Character character)
	{
		if (ScoreOfCategory(category, character) > MESSAGE_MOOD_POSITIVE_NEGATIVE_CUTOFF)
		{
			return MessageMood.POSITIVE;
		}
		else
		{
			return MessageMood.NEGATIVE;
		}
	}



    IEnumerator ShaderLerp(float newExtraStatJ, float newExtraStatP, float newSocialStat, float newAcademicStatJ, float newAcademicStatP)
    {
        float timeElapsed = 0.0f;

        while (timeElapsed < lerpDuration)
        {
            float t = timeElapsed / lerpDuration;

            float curExtraStatJ = Mathf.Clamp(Mathf.Lerp(extracurricularStatJ, newExtraStatJ, t), 0.0f, 100.0f);
            float curExtraStatP = Mathf.Clamp(Mathf.Lerp(extracurricularStatP, newExtraStatP, t), 0.0f, 100.0f);

            float curSocialStat = Mathf.Clamp(Mathf.Lerp(socialStat, newSocialStat, t), 0.0f, 100.0f);

            float curAcademicStatJ = Mathf.Clamp(Mathf.Lerp(academicStatJ, newAcademicStatJ, t), 0.0f, 100.0f);
            float curAcademicStatP = Mathf.Clamp(Mathf.Lerp(academicStatP, newAcademicStatP, t), 0.0f, 100.0f);

            shaders[0].material.SetFloat("_amountRoseTint", curSocialStat / 100);
            shaders[1].material.SetFloat("_amountRoseTint", curSocialStat / 100);
            shaders[0].material.SetFloat("_amountOfStars", curAcademicStatJ / 100);
            shaders[1].material.SetFloat("_amountOfStars", curAcademicStatP / 100);

            //Change _amountOfStars to be whatever the reference value of the medals in the shadergraph is
            shaders[2].material.SetFloat("_amountExtracuricular", curExtraStatJ / 100);
            shaders[3].material.SetFloat("_amountExtracuricular", curExtraStatP / 100);

            timeElapsed += Time.deltaTime;

            yield return null;

        }


        shaders[0].material.SetFloat("_amountRoseTint", Mathf.Clamp(newSocialStat / 100, 0.0f, 100.0f));
        shaders[1].material.SetFloat("_amountRoseTint", Mathf.Clamp(newSocialStat / 100, 0.0f, 100.0f));
        shaders[0].material.SetFloat("_amountOfStars", Mathf.Clamp(newAcademicStatJ / 100, 0.0f, 100.0f));
        shaders[1].material.SetFloat("_amountOfStars", Mathf.Clamp(newAcademicStatP / 100, 0.0f, 100.0f));

        //Change _amountOfStars to be whatever the reference value of the medals in the shadergraph is
        shaders[2].material.SetFloat("_amountExtracuricular", Mathf.Clamp(newExtraStatJ / 100, 0.0f, 100.0f));
        shaders[3].material.SetFloat("_amountExtracuricular", Mathf.Clamp(newExtraStatP / 100, 0.0f, 100.0f));

        socialStat = Mathf.Clamp(newSocialStat, 0.0f, 100.0f);
        academicStatJ = Mathf.Clamp(newAcademicStatJ, 0.0f, 100.0f);
        academicStatP = Mathf.Clamp(newAcademicStatP, 0.0f, 100.0f);
        extracurricularStatJ = Mathf.Clamp(newExtraStatJ, 0.0f, 100.0f);
        extracurricularStatP = Mathf.Clamp(newExtraStatP, 0.0f, 100.0f);

    }

    private void Start()
    {
        shaders[0].material.SetFloat("_amountRoseTint", 1);
        shaders[1].material.SetFloat("_amountRoseTint", 1);
        shaders[0].material.SetFloat("_amountOfStars", 1);
        shaders[1].material.SetFloat("_amountOfStars", 1);

        //Change _amountOfStars to be whatever the reference value of the medals in the shadergraph is
        shaders[2].material.SetFloat("_amountExtracuricular", 1);
        shaders[3].material.SetFloat("_amountExtracuricular", 1);
    }

}
