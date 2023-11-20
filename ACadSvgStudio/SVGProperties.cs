#region copyright LGPL nanoLogika
//  Copyright 2023, nanoLogika GmbH.
//  All rights reserved. 
//  This source code is licensed under the "LGPL v3 or any later version" license. 
//  See LICENSE file in the project root for full license information.
#endregion


using ACadSvg;
using System.ComponentModel;


namespace ACadSvgStudio {

	internal class SvgProperties {

		private MainForm _mainForm;

		public SvgProperties(MainForm mainForm) {
			_mainForm = mainForm;
		}

        #region View

        [Category("View")]
        [DisplayName("Background Color")]
		[Description("Background color.")]
		public Color BackgroundColor {
			get {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					return Settings.Default.BackgroundColor;
				}
				else {
					return Color.Black;
				}
			}
			set {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					Settings.Default.BackgroundColor = value;
					Settings.Default.Save();

					_mainForm.UpdateHTML();
				}
			}
		}


		[Category("View")]
        [DisplayName("Scales Enabled")]
		[Description("Show SVG elements defined in the Scales tab.")]
		public bool ScalesEnabled {
			get {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					return Settings.Default.ScalesEnabled;
				}
				else {
					return true;
				}
			}
			set {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					Settings.Default.ScalesEnabled = value;
					Settings.Default.Save();

					_mainForm.UpdateHTML();
				}
			}
		}


		[Category("View")]
        [DisplayName("Apply \"CSS for Preview\"")]
		[Description("Apply styles from \"CSS for Preview\" editor.")]
		public bool CSSPreviewEnabled {
			get {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					return Settings.Default.CSSPreviewEnabled;
				}
				else {
					return true;
				}
			}
			set {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					Settings.Default.CSSPreviewEnabled = value;
					Settings.Default.Save();

					_mainForm.UpdateHTML();
				}
			}
		}

        #endregion
        #region SVG Viewbox

        [Category("SVG Viewbox")]
		[DisplayName("Viewbox MinX")]
		[Description("ViewBox minimum x parameter.")]
		public double ViewBoxMinX {
			get {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					return Settings.Default.ViewBoxMinX;
				}
				else {
					return 0;
				}
			}
			set {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					Settings.Default.ViewBoxMinX = value;
					Settings.Default.Save();

					_mainForm.UpdateHTML();
				}
			}
		}


		[Category("SVG Viewbox")]
		[DisplayName("Viewbox MinY")]
		[Description("ViewBox minimum y parameter.\r\nNote: Viewbox MinY and SizeY will be transformed accordingly when'Reverse Y-Direction' is set.")]
		public double ViewBoxMinY {
			get {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					return Settings.Default.ViewBoxMinY;
				}
				else {
					return 0;
				}
			}
			set {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					Settings.Default.ViewBoxMinY = value;
					Settings.Default.Save();

					_mainForm.UpdateHTML();
				}
			}
		}

		[Category("SVG Viewbox")]
		[DisplayName("Viewbox SizeX")]
		[Description("ViewBox width parameter.")]
		public double ViewBoxWidth {
			get {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					return Settings.Default.ViewBoxWidth;
				}
				else {
					return 512;
				}
			}
			set {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					Settings.Default.ViewBoxWidth = value;
					Settings.Default.Save();

					_mainForm.UpdateHTML();
				}
			}
		}

		[Category("SVG Viewbox")]
		[DisplayName("Viewbox SizeY")]
		[Description("ViewBox height parameter.\r\nNote: Viewbox MinY and SizeY will be transformed accordingly when'Reverse Y-Direction' is set.")]
		public double ViewBoxHeight {
			get {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					return Settings.Default.ViewBoxHeight;
				}
				else {
					return 512;
				}
			}
			set {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					Settings.Default.ViewBoxHeight = value;
					Settings.Default.Save();

					_mainForm.UpdateHTML();
				}
			}
		}

		[Category("SVG Viewbox")]
		[DisplayName("Viewbox Attribute enabled")]
		[Description("ViewBox attributes will not be added to the SVG tag when disabled.")]
		public bool ViewBoxEnabled {
			get {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					return Settings.Default.ViewBoxEnabled;
				}
				else {
					return true;
				}
			}
			set {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					Settings.Default.ViewBoxEnabled = value;
					Settings.Default.Save();

					_mainForm.UpdateHTML();
				}
			}
		}


		public ViewboxData GetViewbox() {
			return new ViewboxData() {
				Enabled = ViewBoxEnabled,
				MinX = ViewBoxMinX,
				MinY = ViewBoxMinY,
				Width = ViewBoxWidth,
				Height = ViewBoxHeight
			};
        }

		#endregion
		#region SVG/Main Group Attributes

		[Category("SVG/Main Group Attributes")]
        [DisplayName("Stroke Attribute Enabled")]
		[Description("Indicates that, when true, a stroke=\"{color}\" attribute is to be set at the svg element; otherwise, no stroke attribute is set.")]
		public bool StrokeEnabled {
			get {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					return Settings.Default.StrokeEnabled;
				}
				else {
					return true;
				}
			}
			set {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					Settings.Default.StrokeEnabled = value;
					Settings.Default.Save();

					_mainForm.UpdateHTML();
				}
			}
		}


		[Category("SVG/Main Group Attributes")]
        [DisplayName("Fill Attribute Enabled")]
		[Description("Indicates that, when true, a fill=\"{color}\" attribute is to be set at the svg element; otherwise, a fill=\"none\" is set.")]
		public bool FillEnabled {
			get {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					return Settings.Default.FillEnabled;
				}
				else {
					return true;
				}
			}
			set {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					Settings.Default.FillEnabled = value;
					Settings.Default.Save();

					_mainForm.UpdateHTML();
				}
			}
		}


		[Category("SVG/Main Group Attributes")]
        [DisplayName("Stroke Color Attribute")]
		[Description("stroke=\"{color}\" defines the standard color of all strokes. {color} must be a html-color as #RGB or al color name. The attribute is set only when the 'Stroke Attribute Enabled' setting is true.")]
		public Color StrokeColor {
			get {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					return Settings.Default.StrokeColor;
				}
				else {
					return Color.Black;
				}
			}
			set {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					Settings.Default.StrokeColor = value;
					Settings.Default.Save();

					_mainForm.UpdateHTML();
				}
			}
		}


		[Category("SVG/Main Group Attributes")]
		[DisplayName("Stroke Width Attribute")]
		[Description("stroke-width=\"{width}\" defines the standard width of all strokes. {width} must be a number. The attribute is set only when the 'Stroke Attribute Enabled' setting is true.")]
		public double StrokeWidth {
			get {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					return Settings.Default.StrokeWidth;
				}
				else {
					return 1.5;
				}
			}
			set {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					Settings.Default.StrokeWidth = value;
					Settings.Default.Save();

					_mainForm.UpdateHTML();
				}
			}
		}


		[Category("SVG/Main Group Attributes")]
        [DisplayName("Fill Color Attribute")]
		[Description("fill=\"{color}\" defines the standard color of all shapes. {color} must be a html-color as #RGB or a color name, none.")]
		public Color FillColor {
			get {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					return Settings.Default.FillColor;
				}
				else {
					return Color.Black;
				}
			}
			set {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					Settings.Default.FillColor = value;
					Settings.Default.Save();

					_mainForm.UpdateHTML();
				}
			}
		}


		[Category("SVG/Main Group Attributes")]
		[DisplayName("Translation in X")]
		[Description("Used to create the translation clause of a transform attribute: transform=\"translate({x}, {y})\"")]
		public double TransformTranslationX {
			get {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					return Settings.Default.TransformTranslationX;
				}
				else {
					return 1;
				}
			}
			set {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					Settings.Default.TransformTranslationX = value;
					Settings.Default.Save();

					_mainForm.UpdateHTML();
				}
			}
		}


		[Category("SVG/Main Group Attributes")]
		[DisplayName("Translation in Y")]
		[Description("Used to create the translation clause of a transform attribute: transform=\"translate({x}, {y})\"")]
		public double TransformTranslationY {
			get {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					return Settings.Default.TransformTranslationY;
				}
				else {
					return 1;
				}
			}
			set {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					Settings.Default.TransformTranslationY = value;
					Settings.Default.Save();

					_mainForm.UpdateHTML();
				}
			}
		}


		[Category("SVG/Main Group Attributes")]
		[DisplayName("Scale in X")]
		[Description("Used to create the scale clause of a transform attribute: transform=\"scale({x}, {y})\"")]
		public double TransformScaleX {
			get {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					return Settings.Default.TransformScaleX;
				}
				else {
					return 1;
				}
			}
			set {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					Settings.Default.TransformScaleX = value;
					Settings.Default.Save();

					_mainForm.UpdateHTML();
				}
			}
		}


		[Category("SVG/Main Group Attributes")]
		[DisplayName("Scale in Y")]
		[Description("Used to create the translation clause of a transform attribute: transform=\"scale({x}, {y})\". The y scale factor should be specified unsigned. Use the Reverse y-coordinate conversion option when y shall grow from bottom to top.")]
		public double TransformScaleY {
			get {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					return Settings.Default.TransformScaleY;
				}
				else {
					return 1;
				}
			}
			set {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					Settings.Default.TransformScaleY = value;
					Settings.Default.Save();
				}

				_mainForm.UpdateHTML();
			}
		}


		[Category("SVG/Main Group Attributes")]
		[DisplayName("Rotation")]
		[Description("Used to create the rotate clause of a transform attribute: transform=\"rotate({r})\"")]
		public double TransformRotation {
			get {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					return Settings.Default.TransformRotation;
				}
				else {
					return 1;
				}
			}
			set {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					Settings.Default.TransformRotation = value;
					Settings.Default.Save();

					_mainForm.UpdateHTML();
				}
			}
		}


		public GlobalAttributeData GetGlobalAttributeData() {
			return new GlobalAttributeData();
        }

        #endregion
        #region Conversion Options

        [Category("Conversion Options")]
		[DisplayName("Reverse Y-Direction")]
		[Description("In SVG coordinate system y grows from top to bottom. This option allows to reverse the y-coodinates so that y grows from bottom to top and AutoCAD coordinates can be used as they are delivered.")]
		public bool ReverseY {
			get {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					return Settings.Default.ReverseY;
				}
				else {
					return true;
				}
			}
			set {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					Settings.Default.ReverseY = value;
					Settings.Default.Save();
				}
			}
		}


		[Category("Conversion Options")]
		[DisplayName("Create id-Attribute from Entity Handles.")]
		[Description("If this option is set id-attributes are created for every converted AutoCAD Entity from the entity handle (Hex). Otherwise, no id-attributes are created.")]
		public bool ExportHandleAsID {
			get {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					return Settings.Default.ExportHandleAsID;
				}
				else {
					return true;
				}
			}
			set {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					Settings.Default.ExportHandleAsID = value;
					Settings.Default.Save();
				}
			}
		}


		[Category("Conversion Options")]
		[DisplayName("Create class-Attribute from Layer name.")]
		[Description("If this option is set class-attributes are created for every converted AutoCAD Entity from the name of the entity's layer. Otherwise, no class attributes are created. Styles for these classes can be defined in the CSS for preview tab.")]
		public bool ExportLayerAsClass {
			get {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					return Settings.Default.ExportLayerAsClass;
				}
				else {
					return true;
				}
			}
			set {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					Settings.Default.ExportLayerAsClass = value;
					Settings.Default.Save();
				}
			}
		}


		[Category("Conversion Options")]
		[DisplayName("Create class-Attribute from Object Type.")]
		[Description("If this option is set class-attributes are created for every converted AutoCAD Entity from the object type. Otherwise, no class attributes are created. Styles for these classes can be defined in the CSS for preview tab.")]
		public bool ExportObjectTypeAsClass {
			get {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					return Settings.Default.ExportObjectTypeAsClass;
				}
				else {
					return true;
				}
			}
			set {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					Settings.Default.ExportObjectTypeAsClass = value;
					Settings.Default.Save();
				}
			}
		}


		[Category("Conversion Options")]
        [DisplayName("Create Comment for Entities")]
        [Description("A comment for every converted entity is created.")]
		public bool EnableComments {
			get {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					return Settings.Default.EnableComments;
				}
				else {
					return true;
				}
			}
			set {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					Settings.Default.EnableComments = value;
					Settings.Default.Save();
				}
			}
		}


		public ConversionOptions GetConversionOptions() {
			return new ConversionOptions() {
				EnableComments = this.EnableComments,
				ExportHandleAsID = this.ExportHandleAsID,
				ExportLayerAsClass = this.ExportLayerAsClass,
				ExportObjectTypeAsClass = this.ExportObjectTypeAsClass,
				ReverseY = this.ReverseY
			};
		}

		#endregion
	}
}
