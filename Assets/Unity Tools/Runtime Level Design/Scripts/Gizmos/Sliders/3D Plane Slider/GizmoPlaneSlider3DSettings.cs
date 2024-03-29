﻿using UnityEngine;
using System;

namespace RLD
{
    [Serializable]
    public class GizmoPlaneSlider3DSettings
    {
        [SerializeField]
        private float _areaHoverEps = 1e-5f;
        [SerializeField]
        private float _extrudeHoverEps = 1e-5f;
        [SerializeField]
        private float _borderLineHoverEps = 0.7f;
        [SerializeField]
        private float _borderBoxHoverEps = 0.7f;
        [SerializeField]
        private float _borderTorusHoverEps = 0.7f;

        [SerializeField]
        private bool _isCircleHoverCullEnabled;

        [SerializeField]
        private float _offsetSnapStepRight = 1.0f;
        [SerializeField]
        private float _offsetSnapStepUp = 1.0f;
        [SerializeField]
        private float _rotationSnapStep = 15.0f;
        [SerializeField]
        private GizmoSnapMode _rotationSnapMode = GizmoSnapMode.Relative;
        [SerializeField]
        private float _scaleSnapStepRight = 0.1f;
        [SerializeField]
        private float _scaleSnapStepUp = 0.1f;
        [SerializeField]
        private float _proportionalScaleSnapStep = 0.1f;

        [SerializeField]
        private float _offsetSensitivity = 1.0f;
        [SerializeField]
        private float _rotationSensitivity = 0.45f;
        [SerializeField]
        private float _scaleSensitivity = 1.0f;

        public float AreaHoverEps { get { return _areaHoverEps; } set { _areaHoverEps = Mathf.Max(0.0f, value); } }
        public float ExtrudeHoverEps { get { return _extrudeHoverEps; } set { _extrudeHoverEps = Mathf.Max(0.0f, value); } }
        public float BorderLineHoverEps { get { return _borderLineHoverEps; } set { _borderLineHoverEps = Mathf.Max(0.0f, value); } }
        public float BorderBoxHoverEps { get { return _borderBoxHoverEps; } set { _borderBoxHoverEps = Mathf.Max(0.0f, value); } }
        public float BorderTorusHoverEps { get { return _borderTorusHoverEps; } set { _borderTorusHoverEps = Mathf.Max(0.0f, value); } }
        public bool IsCircleHoverCullEnabled { get { return _isCircleHoverCullEnabled; } set { _isCircleHoverCullEnabled = value; } }
        public float OffsetSnapStepRight { get { return _offsetSnapStepRight; } set { _offsetSnapStepRight = Mathf.Max(1e-4f, value); } }
        public float OffsetSnapStepUp { get { return _offsetSnapStepUp; } set { _offsetSnapStepUp = Mathf.Max(1e-4f, value); } }
        public float RotationSnapStep { get { return _rotationSnapStep; } set { _rotationSnapStep = Mathf.Max(1e-4f, value); } }
        public GizmoSnapMode RotationSnapMode { get { return _rotationSnapMode; } set { _rotationSnapMode = value; } }
        public float ScaleSnapStepRight { get { return _scaleSnapStepRight; } set { _scaleSnapStepRight = Mathf.Max(1e-4f, value); } }
        public float ScaleSnapStepUp { get { return _scaleSnapStepUp; } set { _scaleSnapStepUp = Mathf.Max(1e-4f, value); } }
        public float ProportionalScaleSnapStep { get { return _proportionalScaleSnapStep; } set { _proportionalScaleSnapStep = Mathf.Max(1e-4f, value); } }
        public float OffsetSensitivity { get { return _offsetSensitivity; } set { _offsetSensitivity = Mathf.Max(1e-4f, value); } }
        public float RotationSensitivity { get { return _rotationSensitivity; } set { _rotationSensitivity = Mathf.Max(1e-4f, value); } }
        public float ScaleSensitivity { get { return _scaleSensitivity; } set { _scaleSensitivity = Mathf.Max(1e-4f, value); } }
    }
}
