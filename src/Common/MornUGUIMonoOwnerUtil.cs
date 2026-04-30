using System.Collections.Generic;
using UnityEngine;

namespace MornLib
{
    internal static class MornUGUIMonoOwnerUtil
    {
        /// <summary>入れ子になったMornUGUI所有者の配下にあるモジュールを除外する。</summary>
        public static MornUGUIMonoBase[] FilterDirectlyOwned(IMornUGUIMonoOwner selfOwner, MornUGUIMonoBase[] candidates)
        {
            if (candidates == null) return System.Array.Empty<MornUGUIMonoBase>();
            var list = new List<MornUGUIMonoBase>(candidates.Length);
            foreach (var module in candidates)
            {
                if (module != null && IsDirectlyOwnedBy(selfOwner, module.transform))
                {
                    list.Add(module);
                }
            }

            return list.ToArray();
        }

        private static bool IsDirectlyOwnedBy(IMornUGUIMonoOwner selfOwner, Transform moduleTransform)
        {
            var t = moduleTransform;
            while (t != null)
            {
                var owner = t.GetComponent<IMornUGUIMonoOwner>();
                if (owner != null)
                {
                    return ReferenceEquals(owner, selfOwner);
                }

                t = t.parent;
            }

            return false;
        }
    }
}
