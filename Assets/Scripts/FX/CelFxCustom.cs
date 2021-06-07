using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(CelFxRenderer), PostProcessEvent.BeforeStack, "Custom/CelFx")]
public sealed class CelFx : PostProcessEffectSettings
{
    public FloatParameter thickness = new FloatParameter { value = 1.0f };
    public FloatParameter depthMultiplier = new FloatParameter { value = 1.0f };
    public FloatParameter depthBias = new FloatParameter { value = 1.0f };
    public FloatParameter normalMultiplier = new FloatParameter { value = 1.0f };
    public FloatParameter normalBias = new FloatParameter { value = 10.0f };
    public ColorParameter color = new ColorParameter { value = Color.black };
    public TextureParameter rampTexture = new TextureParameter { value = null, defaultState = TextureParameterDefault.White };

}

public sealed class CelFxRenderer : PostProcessEffectRenderer<CelFx>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/CelFx"));
        sheet.properties.SetFloat("_OutlineThickness", settings.thickness);
        sheet.properties.SetFloat("_OutlineDepthMultiplier", settings.depthMultiplier);
        sheet.properties.SetFloat("_OutlineDepthBias", settings.depthBias);
        sheet.properties.SetFloat("_OutlineNormalMultiplier", settings.normalMultiplier);
        sheet.properties.SetFloat("_OutlineNormalBias", settings.normalBias);
        sheet.properties.SetColor("_OutlineColor", settings.color);

        Texture texParameter = settings.rampTexture.value;
        if ( texParameter == null)
        {
            TextureParameterDefault def = settings.rampTexture.defaultState;
            switch (def)
            {
                case TextureParameterDefault.Black:
                    texParameter = RuntimeUtilities.blackTexture;
                    break;
                case TextureParameterDefault.White:
                    texParameter = RuntimeUtilities.whiteTexture;
                    break;
                case TextureParameterDefault.Transparent:
                    texParameter = RuntimeUtilities.transparentTexture;
                    break;
            }
        }
        sheet.properties.SetTexture("_RampTexture", texParameter);
//
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}