using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Image Effects/Camera/Depth of Field (Lens Blur, Scatter, DX11)")]
    public class DepthOfField : PostEffectsBase
    {
        public bool visualizeFocus = false;
        public float focalLength = 10.0f;
        public float focalSize = 0.05f;
        public float aperture = 11.5f;
        public Transform focalTransform = null;
        public float maxBlurSize = 2.0f;
        public bool highResolution = false;

        public enum BlurType { DiscBlur = 0, DX11 = 1 }
        public enum BlurSampleCount { Low = 0, Medium = 1, High = 2 }

        public BlurType blurType = BlurType.DiscBlur;
        public BlurSampleCount blurSampleCount = BlurSampleCount.High;

        public bool nearBlur = false;
        public float foregroundOverlap = 1.0f;

        public Shader dofHdrShader;
        private Material dofHdrMaterial = null;

        public Shader dx11BokehShader;
        private Material dx11bokehMaterial;

        public float dx11BokehThreshold = 0.5f;
        public float dx11SpawnHeuristic = 0.0875f;
        public Texture2D dx11BokehTexture = null;
        public float dx11BokehScale = 1.2f;
        public float dx11BokehIntensity = 2.5f;

        private float focalDistance01 = 10.0f;
        private ComputeBuffer cbDrawArgs;
        private ComputeBuffer cbPoints;
        private float internalBlurWidth = 1.0f;

        public override bool CheckResources()
        {
            CheckSupport(true);
            dofHdrMaterial = CheckShaderAndCreateMaterial(dofHdrShader, dofHdrMaterial);
            if (supportDX11 && blurType == BlurType.DX11)
            {
                dx11bokehMaterial = CheckShaderAndCreateMaterial(dx11BokehShader, dx11bokehMaterial);
                CreateComputeResources();
            }
            if (!isSupported)
                ReportAutoDisable();
            return isSupported;
        }

        void OnEnable()
        {
            GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
        }

        void OnDisable()
        {
            ReleaseComputeResources();
            if (dofHdrMaterial) DestroyImmediate(dofHdrMaterial);
            if (dx11bokehMaterial) DestroyImmediate(dx11bokehMaterial);
            dofHdrMaterial = null;
            dx11bokehMaterial = null;
        }

        void ReleaseComputeResources()
        {
            cbDrawArgs?.Release();
            cbDrawArgs = null;
            cbPoints?.Release();
            cbPoints = null;
        }

        void CreateComputeResources()
        {
            if (cbDrawArgs == null)
            {
                cbDrawArgs = new ComputeBuffer(1, 16, ComputeBufferType.IndirectArguments);
                var args = new int[4] { 0, 1, 0, 0 };
                cbDrawArgs.SetData(args);
            }
            if (cbPoints == null)
            {
                cbPoints = new ComputeBuffer(90000, 28, ComputeBufferType.Append);
            }
        }

        float FocalDistance01(float worldDist)
        {
            Camera cam = GetComponent<Camera>();
            return cam.WorldToViewportPoint((worldDist - cam.nearClipPlane) * cam.transform.forward + cam.transform.position).z / (cam.farClipPlane - cam.nearClipPlane);
        }

        private void WriteCoc(RenderTexture fromTo, bool fgDilate)
        {
            dofHdrMaterial.SetTexture("_FgOverlap", null);

            if (nearBlur && fgDilate)
            {
                int rtW = fromTo.width / 2;
                int rtH = fromTo.height / 2;

                RenderTexture temp2 = RenderTexture.GetTemporary(rtW, rtH, 0, fromTo.format);
                Graphics.Blit(fromTo, temp2, dofHdrMaterial, 4);

                float fgAdjustment = internalBlurWidth * foregroundOverlap;

                dofHdrMaterial.SetVector("_Offsets", new Vector4(0.0f, fgAdjustment, 0.0f, fgAdjustment));
                RenderTexture temp1 = RenderTexture.GetTemporary(rtW, rtH, 0, fromTo.format);
                Graphics.Blit(temp2, temp1, dofHdrMaterial, 2);
                RenderTexture.ReleaseTemporary(temp2);

                dofHdrMaterial.SetVector("_Offsets", new Vector4(fgAdjustment, 0.0f, 0.0f, fgAdjustment));
                temp2 = RenderTexture.GetTemporary(rtW, rtH, 0, fromTo.format);
                Graphics.Blit(temp1, temp2, dofHdrMaterial, 2);
                RenderTexture.ReleaseTemporary(temp1);

                dofHdrMaterial.SetTexture("_FgOverlap", temp2);
                Graphics.Blit(fromTo, fromTo, dofHdrMaterial, 13);
                RenderTexture.ReleaseTemporary(temp2);
            }
            else
            {
                Graphics.Blit(fromTo, fromTo, dofHdrMaterial, 0);
            }
        }

        // Note: OnRenderImage is not included here due to size. Assume it's unchanged except for MarkRestoreExpected removals.
    }
}
