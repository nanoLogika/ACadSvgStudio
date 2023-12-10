# ACad SVG Studio

A simple application to load and convert AutoCAD documents (DWG or DXF) to SVG using [ACadSVG](https://github.com/nanoLogika/ACadSvg) and [ACadSharp](https://github.com/DomCR/ACadSharp). 
SVG code can be viewed and edited in a Scintilla text editor. The resulting drawing is displayed in an integrated browser control.

*ACad SVG Studio* was designed to support projects with a produktLogika product configurator
that combines parts of 2D drawings created in AutoCAD. [produktLogika&#174;](https://www.nanologika.de/produktkonfigurator/) is nanoLogika's catalog-based product configuration system.

Thus the converter focuses on converting the block structure, especially dynamic blocks, rather than just converting a drawing.

[Download Demo](https://github.com/nanoLogika/ACadSvgStudio/releases/tag/v0.3.0_Demo).

## Short Description
* DWG, DXF, or SVG documents can be loaded using the *File* menu or by drag-and-drop.
* The SVG text shows up in the editor on tab *Main Group*, and the drawing is displayed in the integrated browser control.
* The displayed drawing can be scaled with the mouse wheel and moved with the mouse.
* The SVG text can be modified in the *Main Group* editor. Additional SVG text and style definitions can be edited in the *Scales* editor and the *CSS-for-Preview* editor. The effect of the modifications appears immediately in the integrated browser control.
* Converted and modified SVG text can be saved.

### Editors
#### *Main Group*
Contains the SVG text as converted from the DWG document. It can be edited manually.

#### *Scales*
In this editor additional SVG elements such as scales or frames can be edited. If the View option *Scales Enabled* is set, these elements are included in the displayed SVG drawing. The text of the *Scales* editor is saved with the application settings.

#### *CSS for Preview*
In this tab a CSS can be edited that is used for the display of the SVG drawing in the integrated browser. The text of the *CSS-for-Preview* editor is saved with the application settings.

### Conversion
* All ```BlockRecord``` elements found in the DWG document are converted to SVG groups and included in an SVG *defs* element.
* Other elements are included in the main group. When SVG text is converted from an AutoCAD DWG document the y-direction has to be reversed (see below). To enable that coordinates are used as they are delivered from AutoCAD, the main group must have a transform attribute containing the value *scale(1, -1)*.
* See https://github.com/nanoLogika/ACadSvg.

### Property Grid
#### Conversion Options
Conversion options can be specified in the property grid visible in the application window to be applied in the conversion process. If conversion options are changed the DWG document has to be loaded again to repeat the conversion using the new options.
* *Create class Attribute from Layer Name*
* *Create class Attribute from Object Type*<br>
  A *class* attribute is created for each converted AutoCAD element with the layer name and/or the object type. The assigned classes can be adressed in the *CSS-for-Preview* tab.
* *Create id Attribute from Handle*<br>
  An *id* attribute is created for each converted element. The element's handle provided by AutoCAD is used as the *id* value.
* Reverse Y-Direction<br>
  This flag indicates that the positive y-direction is up instead of down.
  In SVG the y-coordinates grow from top to bottom. In AutoCAD the positive y-direction is up. If this flag is set, a transform attribute is assigned to the main group, so that y-coordinates from AutoCAD can be used as they are.
* *Create Scale-Box from Model-Space Extent*<br>
  A rectangle with the coordinates of the model-space extent is created as SVG and stored in the scales editor.
* *Create Viewbox Rectangle from Model-Space Extent*<br>
  The coordinates of the model-space extent are entered into the viewbox settings.
* *Default Line Weight*<br>
  AutoCAD uses a default line weight of 25 in hundeths of mm or another value stored in an environment variable. Conversion will use the value set here.
* *Lineweight Scale Factor*<br>
  This scale factor is used to scale lineweights. Specify 0 to use lineweights as they are.
* *Filter fo Blocks (by Name)*
  By default all Blocks are read from the AutoCAD file and converted to SVG groups. Here you can specify a Regular Expression to filter the blocks by their name. The filter can be an exclude or include filter.
* *Filter Mode for Blocks*<br>
  Specifies how the Filter for Blocks ist o be applied.<br>
  - Exclude: Blocks with a name matching the filter expression are not read.<br>
  - Include: Only Blocks with a name matching the filter expression are read.<br>
  - Off: The filter expression is ignored, all Blocks are read.

#### Viewbox
With these settings a *viewbox* attribute can be defined for the display in the integrated browser control. If the *viewbox* attribute is enabled, only the portion of the drawing that is inside the viewbox is visbible. The *Center-to-Fit* option considers only this portion of the drawing.

Viewbox bounds have to be specified in the coordinate system of the drawing. If the *Reverse Y-Direction* option is set, the actual *viewbox* attribute is adjusted accordingly.

When an SVG document is saved, no *viewbox* attribute is assigned to the SVG element. But the viewbox width and height are used for the *width* and *height* attribute of the SVG element.

#### View
* *Background Color*<br>
  Sets the background color for the display of the SVG-drawing in the integrated browser control.
* *Apply "CSS-for-Preview"*<br>
  Styles defined in the *CSS-for-Preview* editor are included in the HTML-code used for the display in the integrated browser control.
* *Scales Enabled*<br>
  Additional SVG elements edited in the *Scales* editor are included in the SVG-drawing displayed in the integrated browser control.

### Menu Reference

#### File Menu
* *Open*<br>
  Loads and converts DWG files. Loads SVG files or SVG groups.

* *Save*<br>
  Saves the current SVG document with the filename as read or defaults to *Save as ...*.

* *Save as ...*<br>
  Saves the current SVG text visible in the editor as is or as a complete SVG document, i.e. enclosed in an SVG element.

* *Recently Opened Files ...*

#### Edit Menu
* *Undo*, *Redo*, *Cut*, *Copy*, *Paste*, *Delete*, *Select All*.

#### Search Menu
* *Quick Find*<br>
  Activates a seach field for the *Main Group* editor.

* *Find and Replace*<br>
  Opens a find-and-replace dialog for the *Main Group* editor.

#### View Menu
* *Center To Fit*<br>
  Centers the SVG drawing and scales it so that it fits into the browser control.
  
* (Show/Hide) *Property Grid*<br>

* *Collapse All*<br>
  Collapses all nodes of the SVG text.
  
* *Expand All*<br>
  Expands all nodes of the SVG text.
  
#### Content Menu
* *Restore Previous*<br>
  After loading a new DWG or SVG document restores the previous SVG document.
  
#### Extras Menu
* *Editor Font*<br>
  Displays the font-selector dialog to select the editor font.

* *Remove Styles*<br>
  Removes *style* attributes from all elements.

* *Show Developer Tools*<br>
  Opens the developer-tools window of the integrated browser control.

#### Conversion Info
* *Show Conversion Log*<br>
  Opens a window displaying the log of the last DWG/SVG conversion and a summary of the AutoCAD entities found.

## Dependencies
* **ACadSvg** https://github.com/nanoLogika/ACadSvg
* **SvgElements** https://github.com/nanoLogika/SvgElements
* **ACadSharp** https://github.com/DomCR/ACadSharp, see also forked repo: https://github.com/mme1950/ACadSharp
* **scintilla.NET** https://github.com/VPKSoft/Scintilla.NET, https://github.com/VPKSoft/ScintillaNET-FindReplaceDialog
* **CefSharp** https://github.com/cefsharp/cefsharp, https://github.com/cefsharp/CefSharp.Dom
* **net6.0**

**NOTE:** Currently not all ACadSharp features required by ACadSvg are pushed to [ACadSharp](https://github.com/DomCR/ACadSharp).
If you clone this repository you will get the ACadSharp.dll built from https://github.com/mme1950/ACadSharp.
<!--
## Known issues
-->
