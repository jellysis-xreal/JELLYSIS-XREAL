using System;
using System.Collections.Generic;
using EnumTypes;
using StructsType;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerBear : GlobalBears
{
    public DecorateType decorateType;

    public DecorateType GetDecorateType()
    {
        return decorateType;
    }

    public void ChangeDecorateType(DecorateType type)
    {
        decorateType = type;
    }
}
