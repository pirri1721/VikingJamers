﻿// Crest Ocean System

// This file is subject to the MIT License as seen in the root of this folder structure (LICENSE)

using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace Crest
{
    /// <summary>
    /// Sets shader parameters for each geometry tile/chunk.
    /// </summary>
    public class OceanChunkRenderer : MonoBehaviour
    {
        public bool _drawRenderBounds = false;

        Bounds _boundsLocal;
        Mesh _mesh;
        Renderer _rend;
        MaterialPropertyBlock _mpb;

        // Cache these off to support regenerating ocean surface
        int _lodIndex = -1;
        int _totalLodCount = -1;
        int _lodDataResolution = 256;
        int _geoDownSampleFactor = 1;

        int _reflectionTexId = -1;

        void Start()
        {
            _rend = GetComponent<Renderer>();
            _mesh = GetComponent<MeshFilter>().mesh;
            _boundsLocal = _mesh.bounds;

            _reflectionTexId = Shader.PropertyToID("_ReflectionTex");

            UpdateMeshBounds();
        }

        private void Update()
        {
            // This needs to be called on Update because the bounds depend on transform scale which can change. Also OnWillRenderObject depends on
            // the bounds being correct. This could however be called on scale change events, but would add slightly more complexity.
            UpdateMeshBounds();
        }

        void UpdateMeshBounds()
        {
            var newBounds = _boundsLocal;
            ExpandBoundsForDisplacements(transform, ref newBounds);
            _mesh.bounds = newBounds;
        }

        private void OnEnable()
        {
            RenderPipeline.beginCameraRendering += BeginCameraRendering;
        }
        private void OnDisable()
        {
            RenderPipeline.beginCameraRendering -= BeginCameraRendering;
        }

        static Camera _currentCamera = null;

        private void BeginCameraRendering(Camera cam)
        {
            _currentCamera = cam;
        }

        // Called when visible to a camera
        void OnWillRenderObject()
        {
            // check if built-in pipeline being used
            if (Camera.current != null)
            {
                _currentCamera = Camera.current;
            }

            // Depth texture is used by ocean shader for transparency/depth fog, and for fading out foam at shoreline.
            _currentCamera.depthTextureMode |= DepthTextureMode.Depth;

            if (_rend.sharedMaterial != OceanRenderer.Instance.OceanMaterial)
            {
                _rend.sharedMaterial = OceanRenderer.Instance.OceanMaterial;
            }

            // per instance data

            if (_mpb == null)
            {
                _mpb = new MaterialPropertyBlock();
            }
            _rend.GetPropertyBlock(_mpb);

            // blend LOD 0 shape in/out to avoid pop, if the ocean might scale up later (it is smaller than its maximum scale)
            var needToBlendOutShape = _lodIndex == 0 && OceanRenderer.Instance.ScaleCouldIncrease;
            var meshScaleLerp = needToBlendOutShape ? OceanRenderer.Instance.ViewerAltitudeLevelAlpha : 0f;

            // blend furthest normals scale in/out to avoid pop, if scale could reduce
            var needToBlendOutNormals = _lodIndex == _totalLodCount - 1 && OceanRenderer.Instance.ScaleCouldDecrease;
            var farNormalsWeight = needToBlendOutNormals ? OceanRenderer.Instance.ViewerAltitudeLevelAlpha : 1f;
            _mpb.SetVector("_InstanceData", new Vector4(meshScaleLerp, farNormalsWeight, _lodIndex));

            // geometry data
            // compute grid size of geometry. take the long way to get there - make sure we land exactly on a power of two
            // and not inherit any of the lossy-ness from lossyScale.
            var scale_pow_2 = Mathf.Pow(2f, Mathf.Round(Mathf.Log(transform.lossyScale.x) / Mathf.Log(2f)));
            var gridSizeGeo = scale_pow_2 / (0.25f * _lodDataResolution / _geoDownSampleFactor);
            var gridSizeLodData = gridSizeGeo / _geoDownSampleFactor;
            var mul = 1.875f; // fudge 1
            var pow = 1.4f; // fudge 2
            var normalScrollSpeed0 = Mathf.Pow(Mathf.Log(1f + 2f * gridSizeLodData) * mul, pow);
            var normalScrollSpeed1 = Mathf.Pow(Mathf.Log(1f + 4f * gridSizeLodData) * mul, pow);
            _mpb.SetVector("_GeomData", new Vector4(gridSizeLodData, gridSizeGeo, normalScrollSpeed0, normalScrollSpeed1));

            // assign lod data to ocean shader
            var ldaws = OceanRenderer.Instance._lodDataAnimWaves;
            var ldsds = OceanRenderer.Instance._lodDataSeaDepths;
            var ldfoam = OceanRenderer.Instance._lodDataFoam;
            var ldflow = OceanRenderer.Instance._lodDataFlow;
            var ldshadows = OceanRenderer.Instance._lodDataShadow;

            ldaws.BindResultData(_lodIndex, 0, _mpb);
            if (ldflow) ldflow.BindResultData(_lodIndex, 0, _mpb);
            if (ldfoam) ldfoam.BindResultData(_lodIndex, 0, _mpb);
            if (ldsds) ldsds.BindResultData(_lodIndex, 0, _mpb);
            if (ldshadows) ldshadows.BindResultData(_lodIndex, 0, _mpb); else LodDataMgrShadow.BindNull(0, _mpb);

            if (_lodIndex + 1 < OceanRenderer.Instance.CurrentLodCount)
            {
                ldaws.BindResultData(_lodIndex + 1, 1, _mpb);
                if (ldflow) ldflow.BindResultData(_lodIndex + 1, 1, _mpb);
                if (ldfoam) ldfoam.BindResultData(_lodIndex + 1, 1, _mpb);
                if (ldsds) ldsds.BindResultData(_lodIndex + 1, 1, _mpb);
                if (ldshadows) ldshadows.BindResultData(_lodIndex + 1, 1, _mpb); else LodDataMgrShadow.BindNull(1, _mpb);
            }

            var reflTex = OceanPlanarReflection.GetRenderTexture(_currentCamera.targetDisplay);
            if (reflTex)
            {
                _mpb.SetTexture(_reflectionTexId, reflTex);
            }
            else
            {
                _mpb.SetTexture(_reflectionTexId, Texture2D.blackTexture);
            }

            // Hack - due to SV_IsFrontFace occasionally coming through as true for backfaces,
            // add a param here that forces ocean to be in undrwater state. I think the root
            // cause here might be imprecision or numerical issues at ocean tile boundaries, although
            // i'm not sure why cracks are not visible in this case.
            var heightOffset = OceanRenderer.Instance.ViewerHeightAboveWater;
            _mpb.SetFloat("_ForceUnderwater", heightOffset < -2f ? 1f : 0f);

            _rend.SetPropertyBlock(_mpb);

            if (_drawRenderBounds)
            {
                _rend.bounds.DebugDraw();
            }
        }

        // this is called every frame because the bounds are given in world space and depend on the transform scale, which
        // can change depending on view altitude
        public static void ExpandBoundsForDisplacements(Transform transform, ref Bounds bounds)
        {
            float boundsPadding = OceanRenderer.Instance.MaxHorizDisplacement;
            float expandXZ = boundsPadding / transform.lossyScale.x;
            float boundsY = OceanRenderer.Instance.MaxVertDisplacement / transform.lossyScale.y;
            // extend the kinematic bounds slightly to give room for dynamic sim stuff
            boundsY = Mathf.Max(boundsY, 1f);
            bounds.extents = new Vector3(bounds.extents.x + expandXZ, boundsY, bounds.extents.z + expandXZ);
        }

        public void SetInstanceData(int lodIndex, int totalLodCount, int lodDataResolution, int geoDownSampleFactor)
        {
            _lodIndex = lodIndex; _totalLodCount = totalLodCount; _lodDataResolution = lodDataResolution; _geoDownSampleFactor = geoDownSampleFactor;
        }
    }

    public static class BoundsHelper
    {
        public static void DebugDraw(this Bounds b)
        {
            // source: https://github.com/UnityCommunity/UnityLibrary
            // license: mit - https://github.com/UnityCommunity/UnityLibrary/blob/master/LICENSE.md

            // bounding box using Debug.Drawline

            // bottom
            var p1 = new Vector3(b.min.x, b.min.y, b.min.z);
            var p2 = new Vector3(b.max.x, b.min.y, b.min.z);
            var p3 = new Vector3(b.max.x, b.min.y, b.max.z);
            var p4 = new Vector3(b.min.x, b.min.y, b.max.z);

            Debug.DrawLine(p1, p2, Color.blue);
            Debug.DrawLine(p2, p3, Color.red);
            Debug.DrawLine(p3, p4, Color.yellow);
            Debug.DrawLine(p4, p1, Color.magenta);

            // top
            var p5 = new Vector3(b.min.x, b.max.y, b.min.z);
            var p6 = new Vector3(b.max.x, b.max.y, b.min.z);
            var p7 = new Vector3(b.max.x, b.max.y, b.max.z);
            var p8 = new Vector3(b.min.x, b.max.y, b.max.z);

            Debug.DrawLine(p5, p6, Color.blue);
            Debug.DrawLine(p6, p7, Color.red);
            Debug.DrawLine(p7, p8, Color.yellow);
            Debug.DrawLine(p8, p5, Color.magenta);

            // sides
            Debug.DrawLine(p1, p5, Color.white);
            Debug.DrawLine(p2, p6, Color.gray);
            Debug.DrawLine(p3, p7, Color.green);
            Debug.DrawLine(p4, p8, Color.cyan);
        }
    }
}
