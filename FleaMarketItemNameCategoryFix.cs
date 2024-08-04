﻿using System;
using System.Reflection;
using EFT.UI;
using UnityEngine;
using SPT.Reflection.Patching;
using EFT.UI.Ragfair;
using UnityEngine.UI;
using TMPro;

namespace KoreanPatchFix
{
    public class SubcategoryViewPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(SubcategoryView).GetMethod("method_4", BindingFlags.Public | BindingFlags.Instance);
        }

        [PatchPostfix]
        static void PatchPostfix(SubcategoryView __instance, EntityNodeClass node)
        {
            AdjustSubcategoryView(__instance, node);
        }

        private static void AdjustSubcategoryView(SubcategoryView subcategoryView, EntityNodeClass node)
        {
            bool isParentOrHasChildren = node.Parent == null || node.Children.Count > 0;

            // 높이 조정
            var mainLayoutElementField = typeof(SubcategoryView).GetField("_mainLayoutElement", BindingFlags.NonPublic | BindingFlags.Instance);
            if (mainLayoutElementField != null)
            {
                var mainLayoutElement = mainLayoutElementField.GetValue(subcategoryView) as LayoutElement;
                if (mainLayoutElement != null)
                {
                    mainLayoutElement.preferredHeight = 45; // 모든 항목의 높이를 40으로 통일
                }
            }

            // 텍스트 컴포넌트 조정
            AdjustTextComponent(subcategoryView, "CategoryElementName");
            AdjustTextComponent(subcategoryView, "CategoryChildCount");

            // LayoutElement 추가 또는 조정
            var layoutElement = subcategoryView.GetComponent<LayoutElement>();
            if (layoutElement == null)
            {
                layoutElement = subcategoryView.gameObject.AddComponent<LayoutElement>();
            }
            layoutElement.minHeight = 45;

            // RectTransform 조정
            var rectTransform = subcategoryView.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 45);
            }
        }

        private static void AdjustTextComponent(SubcategoryView subcategoryView, string fieldName)
        {
            var field = typeof(NodeBaseView).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null)
            {
                var textComponent = field.GetValue(subcategoryView) as TextMeshProUGUI;
                if (textComponent != null)
                {
                    SetCommonTextProperties(textComponent);
                }
            }
        }

        private static void SetCommonTextProperties(TextMeshProUGUI text)
        {
            text.fontSize = 16;
            text.enableWordWrapping = true;
            text.overflowMode = TextOverflowModes.Overflow;
            text.alignment = TextAlignmentOptions.Left;

            // 텍스트 길이에 따른 폰트 크기 조정
            if (text.text.Length >= 65 && text.text.Length < 95)
            {
                text.fontSize = 14;
            }
            else if (text.text.Length >= 95 && text.text.Length < 110)
            {
                text.fontSize = 12;
            }
            else if (text.text.Length >= 110)
            {
                text.fontSize = 10;
            }
        }
    }

    public class FleaMarketItemCategoryFix : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return null;
        }

        public static void Enable()
        {
            new SubcategoryViewPatch().Enable();
        }
    }
}