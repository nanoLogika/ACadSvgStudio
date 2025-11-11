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

		public enum DefaultLineweightType {
			W0 = ACadSharp.LineWeightType.W0,
            W5 = ACadSharp.LineWeightType.W5,
            W9 = ACadSharp.LineWeightType.W9,
            W13 = ACadSharp.LineWeightType.W13,
            W15 = ACadSharp.LineWeightType.W15,
            W18 = ACadSharp.LineWeightType.W18,
            W20 = ACadSharp.LineWeightType.W20,
            W25 = ACadSharp.LineWeightType.W25,
            W30 = ACadSharp.LineWeightType.W30,
            W35 = ACadSharp.LineWeightType.W35,
            W40 = ACadSharp.LineWeightType.W40,
            W50 = ACadSharp.LineWeightType.W50,
            W53 = ACadSharp.LineWeightType.W53,
            W60 = ACadSharp.LineWeightType.W60,
            W70 = ACadSharp.LineWeightType.W70,
            W80 = ACadSharp.LineWeightType.W80,
            W90 = ACadSharp.LineWeightType.W90,
            W100 = ACadSharp.LineWeightType.W100,
            W106 = ACadSharp.LineWeightType.W106,
            W120 = ACadSharp.LineWeightType.W120,
            W140 = ACadSharp.LineWeightType.W140,
            W158 = ACadSharp.LineWeightType.W158,
            W200 = ACadSharp.LineWeightType.W200,
            W211 = ACadSharp.LineWeightType.W211
        }


        private MainForm _mainForm;

		public SvgProperties(MainForm mainForm) {
			_mainForm = mainForm;
		}
		#region Svg Viewer

		[Category("Svg Viewer")]
		[DisplayName("Debug Enabled")]
		[Description("Show Svg Viewer debug info.")]
		public bool DebugEnabled
		{
			get
			{
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime)
				{
					return Settings.Default.SvgViewerDebugEnabled;
				}
				else
				{
					return false;
				}
			}
			set
			{
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime)
				{
					Settings.Default.SvgViewerDebugEnabled = value;
					Settings.Default.Save();

					_mainForm.ProposeUpdateHTML();
					_mainForm.UpdateSvgViewer();
				}
			}
		}

		#endregion
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

					_mainForm.ProposeUpdateHTML();
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

					_mainForm.ProposeUpdateHTML();
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

					_mainForm.ProposeUpdateHTML();
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
				return 0;
			}
			set {

			}
		}


		[Category("SVG Viewbox")]
		[DisplayName("Viewbox MinY")]
		[Description("ViewBox minimum y parameter.\r\nNote: Viewbox MinY and SizeY will be transformed accordingly when'Reverse Y-Direction' is set.")]
		public double ViewBoxMinY {
			get {
				return 0;
			}
			set {

			}
		}

		[Category("SVG Viewbox")]
		[DisplayName("Viewbox SizeX")]
		[Description("ViewBox width parameter.")]
		public double ViewBoxWidth {
			get {
				return 512;
			}
			set {
				
			}
		}

		[Category("SVG Viewbox")]
		[DisplayName("Viewbox SizeY")]
		[Description("ViewBox height parameter.\r\nNote: Viewbox MinY and SizeY will be transformed accordingly when'Reverse Y-Direction' is set.")]
		public double ViewBoxHeight {
			get {
				return 512;
			}
			set {
				
			}
		}

		[Category("SVG Viewbox")]
		[DisplayName("Viewbox Attribute enabled")]
		[Description("ViewBox attributes will not be added to the SVG tag when disabled.")]
		public bool ViewBoxEnabled {
			get {
				return true;
			}
			set {
				
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
        [DisplayName("Stroke-Color Attribute Enabled")]
		[Description("Indicates that, when true, a stroke=\"{color}\" attribute is to be set at the svg element; otherwise, no stroke attribute is set.")]
		public bool StrokeColorEnabled {
			get {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					return Settings.Default.StrokeColorEnabled;
				}
				else {
					return true;
				}
			}
			set {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					Settings.Default.StrokeColorEnabled = value;
					Settings.Default.Save();

					_mainForm.ProposeUpdateHTML();
				}
			}
		}


        [Category("SVG/Main Group Attributes")]
        [DisplayName("Stroke-Width Attribute Enabled")]
        [Description("Indicates that, when true, a stroke-width=\"{width}\" attribute is to be set at the svg element; otherwise, no stroke-width attribute is set.")]
        public bool StrokeWidthEnabled {
            get {
                if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
                    return Settings.Default.StrokeWidthEnabled;
                }
                else {
                    return true;
                }
            }
            set {
                if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
                    Settings.Default.StrokeWidthEnabled = value;
                    Settings.Default.Save();

                    _mainForm.ProposeUpdateHTML();
                }
            }
        }


        [Category("SVG/Main Group Attributes")]
        [DisplayName("Fill-Color Attribute Enabled")]
		[Description("Indicates that, when true, a fill=\"{color}\" attribute is to be set at the svg element; otherwise, a fill=\"none\" is set.")]
		public bool FillColorEnabled {
			get {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					return Settings.Default.FillColorEnabled;
				}
				else {
					return true;
				}
			}
			set {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					Settings.Default.FillColorEnabled = value;
					Settings.Default.Save();

					_mainForm.ProposeUpdateHTML();
				}
			}
		}


		[Category("SVG/Main Group Attributes")]
        [DisplayName("Stroke-Color Attribute")]
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

					_mainForm.ProposeUpdateHTML();
				}
			}
		}


		[Category("SVG/Main Group Attributes")]
		[DisplayName("Stroke-Width Attribute")]
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

					_mainForm.ProposeUpdateHTML();
				}
			}
		}


		[Category("SVG/Main Group Attributes")]
        [DisplayName("Fill-Color Attribute")]
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

					_mainForm.ProposeUpdateHTML();
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

					_mainForm.ProposeUpdateHTML();
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

					_mainForm.ProposeUpdateHTML();
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

					_mainForm.ProposeUpdateHTML();
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

				_mainForm.ProposeUpdateHTML();
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

					_mainForm.ProposeUpdateHTML();
				}
			}
		}


		public GlobalAttributeData GetGlobalAttributeData() {
			return new GlobalAttributeData() {
				StrokeEnabled = StrokeColorEnabled,
				Stroke = StrokeColor.Name,
				StrokeWidthEnabled = StrokeWidthEnabled,
				StrokeWidth = StrokeWidth,
				FillEnabled = FillColorEnabled,
				Fill = FillColor.Name,
                TransX = TransformTranslationX,
				TransY = TransformTranslationY,
				ScaleX = TransformScaleX,
				ScaleY = TransformScaleY,
				Rotation = TransformRotation
			};
        }

        #endregion
        #region Conversion Options

        [Category("Conversion Options")]
		[DisplayName("Reverse Y-Direction")]
		[Description("In the SVG coordinate system y grows from top to bottom. This option allows to reverse the y-coodinates so that y grows from bottom to top and AutoCAD coordinates can be used as they are delivered.")]
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
        [DisplayName("Default Line Weight.")]
        [Description("AutoCAD uses a default line weight of 25 in hundredths of mm or another value stored in an environment variable. Conversion will use the value set here.")]
        public DefaultLineweightType DefaultLineweight {
            get {
                if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
                    return (DefaultLineweightType)Settings.Default.DefaultLineweight;
                }
                else {
                    return DefaultLineweightType.W25;
                }
            }
            set {
                if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
                    Settings.Default.DefaultLineweight = (short)value;
                    Settings.Default.Save();
                }
            }
        }


        [Category("Conversion Options")]
        [DisplayName("Create Scale-Box from Model-Space Extent.")]
        [Description("A rectangle with the coordinates of the model-space extent is created as SVG and stored in the scales editor.")]
        public bool CreateScaleFromModelSpaceExtent {
            get {
                if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
                    return Settings.Default.CreateScaleFromModelSpaceExtent;
                }
                else {
                    return false;
                }
            }
            set {
                if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
                    Settings.Default.CreateScaleFromModelSpaceExtent = value;
                    Settings.Default.Save();
                }
            }
        }


        [Category("Conversion Options")]
        [DisplayName("Create Viewbox Rectangle from Model Space Extent.")]
        [Description("The coordinates of the model-space extent are entered into the viewbox settings.")]
        public bool CreateViewboxFromModelSpaceExtent {
            get {
                if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
                    return Settings.Default.CreateViewboxFromModelSpaceExtent;
                }
                else {
                    return false;
                }
            }
            set {
                if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
                    Settings.Default.CreateViewboxFromModelSpaceExtent = value;
                    Settings.Default.Save();
                }
            }
        }


        [Category("Conversion Options")]
        [DisplayName("Lineweight Scale Factor.")]
        [Description("This scale factor is used to scale lineweights. Specify 0 to use lineweights as they are.")]
        public double LineweightScaleFactor {
            get {
                if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
                    return Settings.Default.LineweightScaleFactor;
                }
                else {
                    return 0;
                }
            }
            set {
                if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
                    Settings.Default.LineweightScaleFactor = value;
                    Settings.Default.Save();
                }
            }
        }


		[Category("Conversion Options")]
		[DisplayName("Filter for Blocks (by Name)")]
		[Description("By default all Blocks are read from the AutoCAD file and converted to SVG groups. Here you can specify a Regular Expression to filter the blocks by their name. The filter can be an exclude or include filter.")]
		public string GroupFilterRegex {
			get {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					return Settings.Default.GroupFilterRegex;
				}
				else {
					return string.Empty;
				}
			}
			set {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					Settings.Default.GroupFilterRegex = value;
					Settings.Default.Save();
				}
			}
		}


		[Category("Conversion Options")]
		[DisplayName("Filter Mode for Blocks") ]
		[Description("Specifies how the Filter for Blocks ist o be applied.\n- Exclude: Blocks with a name matching the filter expression are not read.\n- Include: Only Blocks with a nname matching the filter expression are read.\n- Off: The filter expression is ignored, all Blocks are read.")]
		public ConversionOptions.FilterMode GroupFilterMode {
			get {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					return (ConversionOptions.FilterMode)Settings.Default.FilterMode;
				}
				else {
					return 0;
				}
			}
			set {
				if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
					Settings.Default.FilterMode = (int)value;
					Settings.Default.Save();
				}
			}
		}


        [Category("Conversion Options")]
		[DisplayName("Concentrate Inserts (converted to <use>)")]
		[Description("Inserts converted to <use> elements are concentrated at the end of the main group.")]
		public bool ConcentrateInserts {
			get {
                if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
                    return Settings.Default.GroupInserts;
                }
                else {
					return false;
                }
			}
			set {
                if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
                    Settings.Default.GroupInserts = value;
                    Settings.Default.Save();
                }
            }
        }


        [Category("Conversion Options")]
        [DisplayName("Create extra group for free elements")]
        [Description("Elemens that are not part of a block converted to <defs><g> ... are grouped in a new group included in the <defs> section. A <use> element referring to that group is placed at the beginning of the main group.")]
        public bool CreateExtraGroupForFreeElements {
            get {
                if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
                    return Settings.Default.CreateExtraGroupForFreeElements;
                }
                else {
                    return false;
                }
            }
            set {
                if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
                    Settings.Default.CreateExtraGroupForFreeElements = value;
                    Settings.Default.Save();
                }
            }
        }


        [Category("Conversion Options")]
        [DisplayName("Create debug elements")]
        [Description("SVG elements are included that reperesent coordinates or definition points, e.g., in leaders or dimensions.")]
        public bool CreateDebugElements {
            get {
                if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
                    return Settings.Default.CreateDebugElements;
                }
                else {
                    return false;
                }
            }
            set {
                if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
                    Settings.Default.CreateDebugElements = value;
                    Settings.Default.Save();
                }
            }
        }


		[Category("Batch Processing")]
		[DisplayName("ACad-Load Base Directory")]
		[Description("The base directory of the repository where AutoCAD documents (DWG or DXF) are to be loaded from. A relative path specified as an input path is assumed to be relative to this base path.")]
        public string BatchACadLoadBaseDirectory {
            get {
                if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
                    return Settings.Default.BatchACadLoadBaseDirectory;
                }
                else {
                    return string.Empty;
                }
            }
            set {
                if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
                    Settings.Default.BatchACadLoadBaseDirectory = value;
                    Settings.Default.Save();
                }
            }
        }


        [Category("Batch Processing")]
        [DisplayName("SVG Base Directory")]
        [Description("The base directory of the repository where converted SVG documents hall be saved to. A relative path specified as an output path is assumed to be relative to this base path.")]
        public string BatchSvgExportBaseDirectory {
            get {
                if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
                    return Settings.Default.BatchSvgExportBaseDirectory;
                }
                else {
                    return string.Empty;
                }
            }
            set {
                if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) {
                    Settings.Default.BatchSvgExportBaseDirectory = value;
                    Settings.Default.Save();
                }
            }
        }


        public ConversionOptions GetConversionOptions() {
			return new ConversionOptions() {
				ExportHandleAsID = this.ExportHandleAsID,
				ExportLayerAsClass = this.ExportLayerAsClass,
				ExportObjectTypeAsClass = this.ExportObjectTypeAsClass,
				ReverseY = this.ReverseY,
				DefaultLineWeight = (ACadSharp.LineWeightType)this.DefaultLineweight,
				CreateScaleFromModelSpaceExtent = this.CreateScaleFromModelSpaceExtent,
				CreateViewboxFromModelSpaceExtent = this.CreateViewboxFromModelSpaceExtent,
				LineweightScaleFactor = this.LineweightScaleFactor,
				GroupFilterRegex = this.GroupFilterRegex,
				GroupFilterMode = this.GroupFilterMode,
				ConcentrateInserts = this.ConcentrateInserts,
				CreateExtraGroupForFreeElements = this.CreateExtraGroupForFreeElements,
				BlockVisibilityParametersPrefix = "_",
                CreateDebugElements = this.CreateDebugElements
            };
		}

		#endregion
	}
}
