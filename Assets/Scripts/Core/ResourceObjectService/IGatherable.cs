using System;
using UnityEngine;

namespace Core.ResourceObjectService
{
    public interface IGatherable
    {
        void TakeGathering(Guid toolId, float damage, Vector3 point, Vector3 normal);
    }
}