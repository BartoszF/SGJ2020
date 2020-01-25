using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace _SGJ2020.Scripts.PPEffects
{
    [Serializable]
    [PostProcess(typeof(DrunkRenderer), PostProcessEvent.AfterStack, "Custom/Drunk")]
    public sealed class Drunk : PostProcessEffectSettings
    {
        [Range(0f, 1f), Tooltip("Drunk effect intensity.")]
        public FloatParameter waving = new FloatParameter { value = 0.1f };
        public FloatParameter duplicating = new FloatParameter { value = 0.1f };
    }

    public sealed class DrunkRenderer : PostProcessEffectRenderer<Drunk>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Drunk"));
            sheet.properties.SetFloat("_Waving", settings.waving);
            sheet.properties.SetFloat("_Duplicating", settings.duplicating);
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}