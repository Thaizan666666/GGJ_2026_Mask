using UnityEngine;

[RequireComponent(typeof(MainGameSystem))]
public class CharacterMaker : MonoBehaviour
{
    private MainGameSystem MGS;
    public SpriteRenderer SpriteRenderer_Mask;
    public SpriteRenderer SpriteRenderer_FoxEar;
    public SpriteRenderer SpriteRenderer_Body;
    public SpriteRenderer SpriteRenderer_Flame;
    public SpriteRenderer SpriteRenderer_Tail;

    public enum Gender
    {
        None,
        Male,
        Female
    }
    private Gender CharGender = Gender.None;

    [Header("Mask Sprite Set")]
    public SO_Mask_Normal Mask_Normal_SO;
    public SO_Mask_Anomaly Mask_Anamoly_SO;

    [Header("Body Female")]
    public SO_FemaleBody_Normal FemaleBody_Normal_SO;
    public SO_FemaleBody_Anomaly FemaleBody_Anamoly_SO;

    [Header("Body Male")]
    public SO_MaleBody_Normal MaleBody_Normal_SO;
    public SO_MaleBody_Anomaly MaleBody_Anamoly_SO;

    [Header("Flame Anomaly")]
    public SO_Flame_Anomaly Flame_Anamoly_SO;

    [Header("Tail")]
    public SO_Tail_Anomaly Tail_Normal_SO;

    [Header("FoxEar")]
    public SO_FoxEar_Anomaly FoxEar_Anamoly_SO;

    

    private void Awake()
    {
        MGS = GetComponent<MainGameSystem>();
        ResetSpriteCustomer();
    }

    public void RandomSpriteSet()
    {
        ResetSpriteCustomer();

        if (MGS.CustomerType == E_CharacterType.Anamoly)
        {
            SetSpriteAnomalyCustomer();
        }

        if (MGS.CustomerType == E_CharacterType.Normal)
        {
            SetSpriteNormalCustomer();
        }
    }

    // Enum to identify each anomaly part slot
    private enum AnomalyPart
    {
        Body,
        Mask,
        FoxEar,
        Flame,
        Tail
    }

    public void SetSpriteAnomalyCustomer()
    {
        SetSpriteNormalCustomer();

        int anomalyCount = 2;
        SetSpriteAnomalyCustomer(anomalyCount, new AnomalyPart[0]);
    }

    private void SetSpriteAnomalyCustomer(int remaining, AnomalyPart[] usedParts)
    {
        if (remaining <= 0) return;

        AnomalyPart picked = (AnomalyPart)Random.Range(0, 5); // 0-4 now covers all 5 parts

        if (System.Array.IndexOf(usedParts, picked) != -1)
        {
            SetSpriteAnomalyCustomer(remaining, usedParts);
            return;
        }

        switch (picked)
        {
            case AnomalyPart.Body:
                if (CharGender == Gender.Male)
                    SpriteRenderer_Body.sprite = MaleBody_Anamoly_SO.sprites.RandomKey();
                else if (CharGender == Gender.Female)
                    SpriteRenderer_Body.sprite = FemaleBody_Anamoly_SO.sprites.RandomKey();
                break;
            case AnomalyPart.Mask:
                SpriteRenderer_Mask.sprite = Mask_Anamoly_SO.sprites.RandomKey();
                break;
            case AnomalyPart.FoxEar:
                SpriteRenderer_FoxEar.sprite = FoxEar_Anamoly_SO.sprites.RandomKey();
                break;
            case AnomalyPart.Flame:
                SpriteRenderer_Flame.sprite = Flame_Anamoly_SO.sprites.RandomValue();
                break;
            case AnomalyPart.Tail:
                SpriteRenderer_Tail.sprite = Tail_Normal_SO.sprites.RandomKey();
                break;
        }

        AnomalyPart[] newUsedParts = new AnomalyPart[usedParts.Length + 1];
        System.Array.Copy(usedParts, newUsedParts, usedParts.Length);
        newUsedParts[usedParts.Length] = picked;

        SetSpriteAnomalyCustomer(remaining - 1, newUsedParts);
    }

    public void SetSpriteNormalCustomer()
    {
        ResetSpriteCustomer();

        //random 50/50 fore M or F
        int randGender = Random.Range(0, 2);
        if (randGender == 0) //Use Male
        {
            CharGender = Gender.Male;
            SpriteRenderer_Body.sprite = MaleBody_Normal_SO.sprites.RandomKey();
        }
        else if (randGender == 1) //Use Female
        {
            CharGender = Gender.Female;
            SpriteRenderer_Body.sprite = FemaleBody_Normal_SO.sprites.RandomKey();
        }
        SpriteRenderer_Mask.sprite = Mask_Normal_SO.sprites.RandomKey();
    }

    public void ResetSpriteCustomer()
    {
        SpriteRenderer_Body.sprite = null;
        SpriteRenderer_Mask.sprite = null;
        SpriteRenderer_Flame.sprite = null;
        SpriteRenderer_Tail.sprite = null;
        SpriteRenderer_FoxEar.sprite = null;
    }

    public void SwitchToWetImage()
    {
        if (SpriteRenderer_Body.sprite != null)
        {
            Sprite wetBody = null;
            if (CharGender == Gender.Male)
            {
                if (!MaleBody_Normal_SO.sprites.TryGetValue(SpriteRenderer_Body.sprite, out wetBody))
                    MaleBody_Anamoly_SO.sprites.TryGetValue(SpriteRenderer_Body.sprite, out wetBody);
            }
            else if (CharGender == Gender.Female)
            {
                if (!FemaleBody_Normal_SO.sprites.TryGetValue(SpriteRenderer_Body.sprite, out wetBody))
                    FemaleBody_Anamoly_SO.sprites.TryGetValue(SpriteRenderer_Body.sprite, out wetBody);
            }
            SpriteRenderer_Body.sprite = wetBody ?? SpriteRenderer_Body.sprite;
        }

        if (SpriteRenderer_Mask.sprite != null)
        {
            Sprite wetMask = null;
            if (!Mask_Normal_SO.sprites.TryGetValue(SpriteRenderer_Mask.sprite, out wetMask))
                Mask_Anamoly_SO.sprites.TryGetValue(SpriteRenderer_Mask.sprite, out wetMask);
            SpriteRenderer_Mask.sprite = wetMask ?? SpriteRenderer_Mask.sprite;
        }

        if (SpriteRenderer_FoxEar.sprite != null)
        {
            Sprite wetFoxEar = null;
            // FoxEar only exists as anomaly, so only check anomaly SO
            FoxEar_Anamoly_SO.sprites.TryGetValue(SpriteRenderer_FoxEar.sprite, out wetFoxEar);
            SpriteRenderer_Mask.sprite = wetFoxEar ?? SpriteRenderer_FoxEar.sprite;
        }

        if (SpriteRenderer_Flame.sprite != null)
        {
            //Sprite wetFlame = null;
            //Sprite wetFlame = null;
            //// Flame only exists as anomaly, so only check anomaly SO
            //Flame_Anamoly_SO.sprites.TryGetValue(SpriteRenderer_Flame.sprite, out wetFlame);
            //SpriteRenderer_Flame.sprite = wetFlame ?? SpriteRenderer_Flame.sprite;
        }

        if (SpriteRenderer_Tail.sprite != null)
        {
            Sprite wetTail = null;
            // Tail only exists as anomaly, so only check anomaly SO
            Tail_Normal_SO.sprites.TryGetValue(SpriteRenderer_Tail.sprite, out wetTail);
            SpriteRenderer_Tail.sprite = wetTail ?? SpriteRenderer_Tail.sprite;
        }
    }
}