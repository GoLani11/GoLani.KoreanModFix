using BepInEx;
using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace KoreanPatchFix
{
    [BepInPlugin("com.GoLani.koreanpatchfix", "Korean Patch Fix", "1.1.1")]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            FleaMarketItemNameFix.Enable();
            FleaMarketItemCategoryFix.Enable();
            GesturesMenuFix.Enable();
        }
    }
}
