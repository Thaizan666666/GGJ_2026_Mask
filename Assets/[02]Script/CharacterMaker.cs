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

    [Header("Mask Female")]
    public SO_Mask_Normal Mask_Normal_F_SO;
    public SO_Mask_Anomaly Mask_Anamoly_F_SO;

    [Header("Mask Male")]
    public SO_Mask_Normal Mask_Normal_M_SO;
    public SO_Mask_Anomaly Mask_Anamoly_M_SO;

    [Header("Body Female")]
    public SO_FemaleBody_Normal FemaleBody_Normal_SO;
    public SO_FemaleBody_Anomaly FemaleBody_Anamoly_SO;

    [Header("Body Male")]
    public SO_MaleBody_Normal MaleBody_Normal_SO;
    public SO_MaleBody_Anomaly MaleBody_Anamoly_SO;

    [Header("Fox Ear Female")]
    public SO_FoxEar_F_Normal FoxEar_F_Normal_SO;
    public SO_FoxEar_F_Anomaly FoxEar_F_Anomaly_SO;

    [Header("Fox Ear Male")]
    public SO_FoxEar_M_Normal FoxEar_M_Normal_SO;
    public SO_FoxEar_M_Anomaly FoxEar_M_Anomaly_SO;

    [Header("Flame Anomaly")]
    public SO_Flame_Anomaly Flame_Anamoly_SO;

    [Header("Tail")]
    public SO_Tail_Anomaly Tail_Normal_SO;



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
        Mask,
        FoxEar,
        Flame,
        Tail
    }

    public void SetSpriteAnomalyCustomer()
    {
        SetSpriteNormalCustomer();

        // Always set anomaly body
        if (CharGender == Gender.Male)
            SpriteRenderer_Body.sprite = MaleBody_Anamoly_SO.sprites.RandomKey();
        else if (CharGender == Gender.Female)
            SpriteRenderer_Body.sprite = FemaleBody_Anamoly_SO.sprites.RandomKey();

        int anomalyCount = Random.Range(1, 3); // Random 1 or 2 additional anomalies
        SetSpriteAnomalyCustomer(anomalyCount, new AnomalyPart[0]);
    }

    private void SetSpriteAnomalyCustomer(int remaining, AnomalyPart[] usedParts)
    {
        if (remaining <= 0) return;

        AnomalyPart picked = (AnomalyPart)Random.Range(0, 4); // 0-3 now covers all 4 parts (Mask, FoxEar, Flame, Tail)

        // If this part was already used, try again
        if (System.Array.IndexOf(usedParts, picked) != -1)
        {
            SetSpriteAnomalyCustomer(remaining, usedParts);
            return;
        }

        // If mask already exists (from normal setup), cannot select fox ear
        if (picked == AnomalyPart.FoxEar && SpriteRenderer_Mask.sprite != null)
        {
            SetSpriteAnomalyCustomer(remaining, usedParts);
            return;
        }

        // If fox ear already exists (from normal setup), cannot select mask
        if (picked == AnomalyPart.Mask && SpriteRenderer_FoxEar.sprite != null)
        {
            SetSpriteAnomalyCustomer(remaining, usedParts);
            return;
        }

        // If mask is already selected in this anomaly pass, cannot select ear
        if (picked == AnomalyPart.FoxEar && System.Array.IndexOf(usedParts, AnomalyPart.Mask) != -1)
        {
            SetSpriteAnomalyCustomer(remaining, usedParts);
            return;
        }

        // If ear is already selected in this anomaly pass, cannot select mask
        if (picked == AnomalyPart.Mask && System.Array.IndexOf(usedParts, AnomalyPart.FoxEar) != -1)
        {
            SetSpriteAnomalyCustomer(remaining, usedParts);
            return;
        }

        switch (picked)
        {
            case AnomalyPart.Mask:
                if (CharGender == Gender.Male)
                    SpriteRenderer_Mask.sprite = Mask_Anamoly_M_SO.sprites.RandomKey();
                else if (CharGender == Gender.Female)
                    SpriteRenderer_Mask.sprite = Mask_Anamoly_F_SO.sprites.RandomKey();
                break;
            case AnomalyPart.FoxEar:
                if (CharGender == Gender.Male)
                    SpriteRenderer_FoxEar.sprite = FoxEar_M_Anomaly_SO.sprites.RandomKey();
                else if (CharGender == Gender.Female)
                    SpriteRenderer_FoxEar.sprite = FoxEar_F_Anomaly_SO.sprites.RandomKey();
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

        SpriteRenderer_FoxEar.sortingOrder = -1;

        SetSpriteAnomalyCustomer(remaining - 1, newUsedParts);
    }

    public void SetSpriteNormalCustomer()
    {
        ResetSpriteCustomer();

        //random 50/50 for M or F
        int randGender = Random.Range(0, 2);
        if (randGender == 0) //Use Male
        {
            CharGender = Gender.Male;
            SpriteRenderer_Body.sprite = MaleBody_Normal_SO.sprites.RandomKey();

            // 50/50 chance for mask or fox ear, but not both
            int randFaceFeature = Random.Range(0, 2);
            if (randFaceFeature == 0)
            {
                SpriteRenderer_Mask.sprite = Mask_Normal_M_SO.sprites.RandomKey();
            }
            else
            {
                SpriteRenderer_FoxEar.sprite = FoxEar_M_Normal_SO.sprites.RandomKey();
            }
        }
        else if (randGender == 1) //Use Female
        {
            CharGender = Gender.Female;
            SpriteRenderer_Body.sprite = FemaleBody_Normal_SO.sprites.RandomKey();

            // 50/50 chance for mask or fox ear, but not both
            int randFaceFeature = Random.Range(0, 2);
            if (randFaceFeature == 0)
            {
                SpriteRenderer_Mask.sprite = Mask_Normal_F_SO.sprites.RandomKey();
            }
            else
            {
                SpriteRenderer_FoxEar.sprite = FoxEar_F_Normal_SO.sprites.RandomKey();
            }
        }

        SpriteRenderer_FoxEar.sortingOrder = 0;
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
            if (CharGender == Gender.Male)
            {
                if (!Mask_Normal_M_SO.sprites.TryGetValue(SpriteRenderer_Mask.sprite, out wetMask))
                    Mask_Anamoly_M_SO.sprites.TryGetValue(SpriteRenderer_Mask.sprite, out wetMask);
            }
            else if (CharGender == Gender.Female)
            {
                if (!Mask_Normal_F_SO.sprites.TryGetValue(SpriteRenderer_Mask.sprite, out wetMask))
                    Mask_Anamoly_F_SO.sprites.TryGetValue(SpriteRenderer_Mask.sprite, out wetMask);
            }
            SpriteRenderer_Mask.sprite = wetMask ?? SpriteRenderer_Mask.sprite;
        }

        if (SpriteRenderer_FoxEar.sprite != null)
        {
            Sprite wetFoxEar = null;
            if (CharGender == Gender.Male)
            {
                if (!FoxEar_M_Normal_SO.sprites.TryGetValue(SpriteRenderer_FoxEar.sprite, out wetFoxEar))
                    FoxEar_M_Anomaly_SO.sprites.TryGetValue(SpriteRenderer_FoxEar.sprite, out wetFoxEar);
            }
            else if (CharGender == Gender.Female)
            {
                if (!FoxEar_F_Normal_SO.sprites.TryGetValue(SpriteRenderer_FoxEar.sprite, out wetFoxEar))
                    FoxEar_F_Anomaly_SO.sprites.TryGetValue(SpriteRenderer_FoxEar.sprite, out wetFoxEar);
            }
            SpriteRenderer_FoxEar.sprite = wetFoxEar ?? SpriteRenderer_FoxEar.sprite;
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