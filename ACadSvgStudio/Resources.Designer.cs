﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ACadSvgStudio {
    using System;
    
    
    /// <summary>
    ///   Eine stark typisierte Ressourcenklasse zum Suchen von lokalisierten Zeichenfolgen usw.
    /// </summary>
    // Diese Klasse wurde von der StronglyTypedResourceBuilder automatisch generiert
    // -Klasse über ein Tool wie ResGen oder Visual Studio automatisch generiert.
    // Um einen Member hinzuzufügen oder zu entfernen, bearbeiten Sie die .ResX-Datei und führen dann ResGen
    // mit der /str-Option erneut aus, oder Sie erstellen Ihr VS-Projekt neu.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Gibt die zwischengespeicherte ResourceManager-Instanz zurück, die von dieser Klasse verwendet wird.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ACadSvgStudio.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Überschreibt die CurrentUICulture-Eigenschaft des aktuellen Threads für alle
        ///   Ressourcenzuordnungen, die diese stark typisierte Ressourcenklasse verwenden.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die function resetZoomAndPan() {
        ///    try {
        ///		// Destroy panZoom if already exists
        ///		window.panZoomWasDefined = window.panZoom != null &amp;&amp; window.panZoom != undefined;
        ///        window.prevPan = window.panZoomWasDefined ? window.panZoom.getPan() : undefined;
        ///		window.prevZoom = window.panZoomWasDefined ? window.panZoom.getZoom() : undefined;
        ///
        ///        if (window.panZoomWasDefined &amp;&amp; window.panZoom != undefined) {
        ///            window.panZoom.destroy();
        ///        }
        ///
        ///        window.panZoom = svgPanZoom(&apos;#svg-el [Rest der Zeichenfolge wurde abgeschnitten]&quot;; ähnelt.
        /// </summary>
        internal static string reset_pan_zoom {
            get {
                return ResourceManager.GetString("reset-pan-zoom", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die // svg-pan-zoom v3.6.1
        ///// https://github.com/ariutta/svg-pan-zoom
        ///(function(){function r(e,n,t){function o(i,f){if(!n[i]){if(!e[i]){var c=&quot;function&quot;==typeof require&amp;&amp;require;if(!f&amp;&amp;c)return c(i,!0);if(u)return u(i,!0);var a=new Error(&quot;Cannot find module &apos;&quot;+i+&quot;&apos;&quot;);throw a.code=&quot;MODULE_NOT_FOUND&quot;,a}var p=n[i]={exports:{}};e[i][0].call(p.exports,function(r){var n=e[i][1][r];return o(n||r)},p,p.exports,r,e,n,t)}return n[i].exports}for(var u=&quot;function&quot;==typeof require&amp;&amp;require,i=0;i&lt;t.length;i++)o(t[i]);return [Rest der Zeichenfolge wurde abgeschnitten]&quot;; ähnelt.
        /// </summary>
        internal static string svg_pan_zoom {
            get {
                return ResourceManager.GetString("svg-pan-zoom", resourceCulture);
            }
        }
    }
}
