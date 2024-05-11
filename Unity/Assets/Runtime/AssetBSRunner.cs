using BSDesigner.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BSDesigner.Unity
{
    public class AssetBSRunner : BSRunner
    {
        [SerializeField] BSPrototype prototype;
        protected override BehaviourEngine CreateBehaviourSystem()
        {
            return prototype.CreateBehaviourSystem();
        }
    }
}
