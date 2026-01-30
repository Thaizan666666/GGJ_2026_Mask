using UnityEngine;

public class CharacterMaker : MonoBehaviour
{
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
}
