using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas;
using RobotKit;
using Windows.ApplicationModel;
using System.Diagnostics;
using RobotKit.Internal;
using Windows.UI.Popups;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SensorialRhythm
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Color[] randomColors = {
            Colors.AliceBlue,
        //
        // Summary:
        //     Gets the color value that represents the AntiqueWhite named color.
        //
        // Returns:
        //     The color value that represents the AntiqueWhite named color.
        Colors.AntiqueWhite,
        //
        // Summary:
        //     Gets the color value that represents the Aqua named color.
        //
        // Returns:
        //     The color value that represents the Aqua named color.
        Colors.Aqua,
        //
        // Summary:
        //     Gets the color value that represents the Aquamarine named color.
        //
        // Returns:
        //     The color value that represents the Aquamarine named color.
        Colors.Aquamarine,
        //
        // Summary:
        //     Gets the color value that represents the Azure named color.
        //
        // Returns:
        //     The color value that represents the Azure named color.
        Colors.Azure,
        //
        // Summary:
        //     Gets the color value that represents the Beige named color.
        //
        // Returns:
        //     The color value that represents the Beige named color.
        Colors.Beige,
        //
        // Summary:
        //     Gets the color value that represents the Bisque named color.
        //
        // Returns:
        //     The color value that represents the Bisque named color.
        Colors.Bisque,
        //
        // Summary:
        //     Gets the system-defined color that has the ARGB value of #FF000000.
        //
        // Returns:
        //     The system-defined color that has the ARGB value of #FF000000.
        Colors.Black,
        //
        // Summary:
        //     Gets the color value that represents the BlanchedAlmond named color.
        //
        // Returns:
        //     The color value that represents the BlanchedAlmond named color.
        Colors.BlanchedAlmond,
        //
        // Summary:
        //     Gets the system-defined color that has the ARGB value of #FF0000FF.
        //
        // Returns:
        //     The system-defined color that has the ARGB value of #FF0000FF.
        Colors.Blue,
        //
        // Summary:
        //     Gets the color value that represents the BlueViolet named color.
        //
        // Returns:
        //     The color value that represents the BlueViolet named color.
        Colors.BlueViolet,
        //
        // Summary:
        //     Gets the system-defined color that has the ARGB value of #FFA52A2A.
        //
        // Returns:
        //     The system-defined color that has the ARGB value of #FFA52A2A.
        Colors.Brown,
        //
        // Summary:
        //     Gets the color value that represents the BurlyWood named color.
        //
        // Returns:
        //     The color value that represents the BurlyWood named color.
        Colors.BurlyWood,
        //
        // Summary:
        //     Gets the color value that represents the CadetBlue named color.
        //
        // Returns:
        //     The color value that represents the CadetBlue named color.
        Colors.CadetBlue,
        //
        // Summary:
        //     Gets the color value that represents the Chartreuse named color.
        //
        // Returns:
        //     The color value that represents the Chartreuse named color.
        Colors.Chartreuse,
        //
        // Summary:
        //     Gets the color value that represents the Chocolate named color.
        //
        // Returns:
        //     The color value that represents the Chocolate named color.
        Colors.Chocolate,
        //
        // Summary:
        //     Gets the color value that represents the Coral named color.
        //
        // Returns:
        //     The color value that represents the Coral named color.
        Colors.Coral,
        //
        // Summary:
        //     Gets the color value that represents the CornflowerBlue named color.
        //
        // Returns:
        //     The color value that represents the CornflowerBlue named color.
        Colors.CornflowerBlue,
        //
        // Summary:
        //     Gets the color value that represents the Cornsilk named color.
        //
        // Returns:
        //     The color value that represents the Cornsilk named color.
        Colors.Cornsilk,
        //
        // Summary:
        //     Gets the color value that represents the Crimson named color.
        //
        // Returns:
        //     The color value that represents the Crimson named color.
        Colors.Crimson,
        //
        // Summary:
        //     Gets the system-defined color that has the ARGB value of #FF00FFFF.
        //
        // Returns:
        //     The system-defined color that has the ARGB value of ##FF00FFFF.
        Colors.Cyan,
        //
        // Summary:
        //     Gets the color value that represents the DarkBlue named color.
        //
        // Returns:
        //     The color value that represents the DarkBlue named color.
        Colors.DarkBlue,
        //
        // Summary:
        //     Gets the color value that represents the DarkCyan named color.
        //
        // Returns:
        //     The color value that represents the DarkCyan named color.
        Colors.DarkCyan,
        //
        // Summary:
        //     Gets the color value that represents the DarkGoldenrod named color.
        //
        // Returns:
        //     The color value that represents the DarkGoldenrod named color.
        Colors.DarkGoldenrod,
        //
        // Summary:
        //     Gets the system-defined color that has the ARGB value of #FFA9A9A9.
        //
        // Returns:
        //     The system-defined color that has the ARGB value of #FFA9A9A9.
        Colors.DarkGray,
        //
        // Summary:
        //     Gets the color value that represents the DarkGreen named color.
        //
        // Returns:
        //     The color value that represents the DarkGreen named color.
        Colors.DarkGreen,
        //
        // Summary:
        //     Gets the color value that represents the DarkKhaki named color.
        //
        // Returns:
        //     The color value that represents the DarkKhaki named color.
        Colors.DarkKhaki,
        //
        // Summary:
        //     Gets the color value that represents the DarkMagenta named color.
        //
        // Returns:
        //     The color value that represents the DarkMagenta named color.
        Colors.DarkMagenta,
        //
        // Summary:
        //     Gets the color value that represents the DarkOliveGreen named color.
        //
        // Returns:
        //     The color value that represents the DarkOliveGreen named color.
        Colors.DarkOliveGreen,
        //
        // Summary:
        //     Gets the color value that represents the DarkOrange named color.
        //
        // Returns:
        //     The color value that represents the DarkOrange named color.
        Colors.DarkOrange,
        //
        // Summary:
        //     Gets the color value that represents the DarkOrchid named color.
        //
        // Returns:
        //     The color value that represents the DarkOrchid named color.
        Colors.DarkOrchid,
        //
        // Summary:
        //     Gets the color value that represents the DarkRed named color.
        //
        // Returns:
        //     The color value that represents the DarkRed named color.
        Colors.DarkRed,
        //
        // Summary:
        //     Gets the color value that represents the DarkSalmon named color.
        //
        // Returns:
        //     The color value that represents the DarkSalmon named color.
        Colors.DarkSalmon,
        //
        // Summary:
        //     Gets the color value that represents the DarkSeaGreen named color.
        //
        // Returns:
        //     The color value that represents the DarkSeaGreen named color.
        Colors.DarkSeaGreen,
        //
        // Summary:
        //     Gets the color value that represents the DarkSlateBlue named color.
        //
        // Returns:
        //     The color value that represents the DarkSlateBlue named color.
        Colors.DarkSlateBlue,
        //
        // Summary:
        //     Gets the color value that represents the DarkSlateGray named color.
        //
        // Returns:
        //     The color value that represents the DarkSlateGray named color.
        Colors.DarkSlateGray,
        //
        // Summary:
        //     Gets the color value that represents the DarkTurquoise named color.
        //
        // Returns:
        //     The color value that represents the DarkTurquoise named color.
        Colors.DarkTurquoise,
        //
        // Summary:
        //     Gets the color value that represents the DarkViolet named color.
        //
        // Returns:
        //     The color value that represents the DarkViolet named color.
        Colors.DarkViolet,
        //
        // Summary:
        //     Gets the color value that represents the DeepPink named color.
        //
        // Returns:
        //     The color value that represents the DeepPink named color.
        Colors.DeepPink,
        //
        // Summary:
        //     Gets the color value that represents the DeepSkyBlue named color.
        //
        // Returns:
        //     The color value that represents the DeepSkyBlue named color.
        Colors.DeepSkyBlue,
        //
        // Summary:
        //     Gets the color value that represents the DimGray named color.
        //
        // Returns:
        //     The color value that represents the DimGray named color.
        Colors.DimGray,
        //
        // Summary:
        //     Gets the color value that represents the DodgerBlue named color.
        //
        // Returns:
        //     The color value that represents the DodgerBlue named color.
        Colors.DodgerBlue,
        //
        // Summary:
        //     Gets the color value that represents the Firebrick named color.
        //
        // Returns:
        //     The color value that represents the Firebrick named color.
        Colors.Firebrick,
        //
        // Summary:
        //     Gets the color value that represents the FloralWhite named color.
        //
        // Returns:
        //     The color value that represents the FloralWhite named color.
        Colors.FloralWhite,
        //
        // Summary:
        //     Gets the color value that represents the ForestGreen named color.
        //
        // Returns:
        //     The color value that represents the ForestGreen named color.
        Colors.ForestGreen,
        //
        // Summary:
        //     Gets the color value that represents the Fuchsia named color.
        //
        // Returns:
        //     The color value that represents the Fuchsia named color.
        Colors.Fuchsia,
        //
        // Summary:
        //     Gets the color value that represents the Gainsboro named color.
        //
        // Returns:
        //     The color value that represents the Gainsboro named color.
        Colors.Gainsboro,
        //
        // Summary:
        //     Gets the color value that represents the GhostWhite named color.
        //
        // Returns:
        //     The color value that represents the GhostWhite named color.
        Colors.GhostWhite,
        //
        // Summary:
        //     Gets the color value that represents the Gold named color.
        //
        // Returns:
        //     The color value that represents the Gold named color.
        Colors.Gold,
        //
        // Summary:
        //     Gets the color value that represents the Goldenrod named color.
        //
        // Returns:
        //     The color value that represents the Goldenrod named color.
        Colors.Goldenrod,
        //
        // Summary:
        //     Gets the system-defined color that has the ARGB value of #FF808080.
        //
        // Returns:
        //     The system-defined color that has the ARGB value of #FF808080.
        Colors.Gray,
        //
        // Summary:
        //     Gets the system-defined color that has the ARGB value of #FF008000.
        //
        // Returns:
        //     The system-defined color that has the ARGB value of #FF008000.
        Colors.Green,
        //
        // Summary:
        //     Gets the color value that represents the GreenYellow named color.
        //
        // Returns:
        //     The color value that represents the GreenYellow named color.
        Colors.GreenYellow,
        //
        // Summary:
        //     Gets the color value that represents the Honeydew named color.
        //
        // Returns:
        //     The color value that represents the Honeydew named color.
        Colors.Honeydew,
        //
        // Summary:
        //     Gets the color value that represents the HotPink named color.
        //
        // Returns:
        //     The color value that represents the HotPink named color.
        Colors.HotPink,
        //
        // Summary:
        //     Gets the color value that represents the IndianRed named color.
        //
        // Returns:
        //     The color value that represents the IndianRed named color.
        Colors.IndianRed,
        //
        // Summary:
        //     Gets the color value that represents the Indigo named color.
        //
        // Returns:
        //     The color value that represents the Indigo named color.
        Colors.Indigo,
        //
        // Summary:
        //     Gets the color value that represents the Ivory named color.
        //
        // Returns:
        //     The color value that represents the Ivory named color.
        Colors.Ivory,
        //
        // Summary:
        //     Gets the color value that represents the Khaki named color.
        //
        // Returns:
        //     The color value that represents the Khaki named color.
        Colors.Khaki,
        //
        // Summary:
        //     Gets the color value that represents the Lavender named color.
        //
        // Returns:
        //     The color value that represents the Lavender named color.
        Colors.Lavender,
        //
        // Summary:
        //     Gets the color value that represents the LavenderBlush named color.
        //
        // Returns:
        //     The color value that represents the LavenderBlush named color.
        Colors.LavenderBlush,
        //
        // Summary:
        //     Gets the color value that represents the LawnGreen named color.
        //
        // Returns:
        //     The color value that represents the LawnGreen named color.
        Colors.LawnGreen,
        //
        // Summary:
        //     Gets the color value that represents the LemonChiffon named color.
        //
        // Returns:
        //     The color value that represents the LemonChiffon named color.
        Colors.LemonChiffon,
        //
        // Summary:
        //     Gets the color value that represents the LightBlue named color.
        //
        // Returns:
        //     The color value that represents the LightBlue named color.
        Colors.LightBlue,
        //
        // Summary:
        //     Gets the color value that represents the LightCoral named color.
        //
        // Returns:
        //     The color value that represents the LightCoral named color.
        Colors.LightCoral,
        //
        // Summary:
        //     Gets the color value that represents the LightCyan named color.
        //
        // Returns:
        //     The color value that represents the LightCyan named color.
        Colors.LightCyan,
        //
        // Summary:
        //     Gets the color value that represents the LightGoldenrodYellow named color.
        //
        // Returns:
        //     The color value that represents the LightGoldenrodYellow named color.
        Colors.LightGoldenrodYellow,
        //
        // Summary:
        //     Gets the system-defined color that has the ARGB value of #FFD3D3D3.
        //
        // Returns:
        //     Gets the system-defined color that has the ARGB value of #FFD3D3D3.
        Colors.LightGray,
        //
        // Summary:
        //     Gets the color value that represents the LightGreen named color.
        //
        // Returns:
        //     The color value that represents the LightGreen named color.
        Colors.LightGreen,
        //
        // Summary:
        //     Gets the color value that represents the LightPink named color.
        //
        // Returns:
        //     The color value that represents the LightPink named color.
        Colors.LightPink,
        //
        // Summary:
        //     Gets the color value that represents the LightSalmon named color.
        //
        // Returns:
        //     The color value that represents the LightSalmon named color.
        Colors.LightSalmon,
        //
        // Summary:
        //     Gets the color value that represents the LightSeaGreen named color.
        //
        // Returns:
        //     The color value that represents the LightSeaGreen named color.
        Colors.LightSeaGreen,
        //
        // Summary:
        //     Gets the color value that represents the LightSkyBlue named color.
        //
        // Returns:
        //     The color value that represents the LightSkyBlue named color.
        Colors.LightSkyBlue,
        //
        // Summary:
        //     Gets the color value that represents the LightSlateGray named color.
        //
        // Returns:
        //     The color value that represents the LightSlateGray named color.
        Colors.LightSlateGray,
        //
        // Summary:
        //     Gets the color value that represents the LightSteelBlue named color.
        //
        // Returns:
        //     The color value that represents the LightSteelBlue named color.
        Colors.LightSteelBlue,
        //
        // Summary:
        //     Gets the color value that represents the LightYellow named color.
        //
        // Returns:
        //     The color value that represents the LightYellow named color.
        Colors.LightYellow,
        //
        // Summary:
        //     Gets the color value that represents the Lime named color.
        //
        // Returns:
        //     The color value that represents the Lime named color.
        Colors.Lime,
        //
        // Summary:
        //     Gets the color value that represents the LimeGreen named color.
        //
        // Returns:
        //     The color value that represents the LimeGreen named color.
        Colors.LimeGreen,
        //
        // Summary:
        //     Gets the color value that represents the Linen named color.
        //
        // Returns:
        //     The color value that represents the Linen named color.
        Colors.Linen,
        //
        // Summary:
        //     Gets the system-defined color that has the ARGB value of #FFFF00FF..
        //
        // Returns:
        //     The system-defined color that has the ARGB value of #FFFF00FF..
        Colors.Magenta,
        //
        // Summary:
        //     Gets the color value that represents the Maroon named color.
        //
        // Returns:
        //     The color value that represents the Maroon named color.
        Colors.Maroon,
        //
        // Summary:
        //     Gets the color value that represents the MediumAquamarine named color.
        //
        // Returns:
        //     The color value that represents the MediumAquamarine named color.
        Colors.MediumAquamarine,
        //
        // Summary:
        //     Gets the color value that represents the MediumBlue named color.
        //
        // Returns:
        //     The color value that represents the MediumBlue named color.
        Colors.MediumBlue,
        //
        // Summary:
        //     Gets the color value that represents the MediumOrchid named color.
        //
        // Returns:
        //     The color value that represents the MediumOrchid named color.
        Colors.MediumOrchid,
        //
        // Summary:
        //     Gets the color value that represents the MediumPurple named color.
        //
        // Returns:
        //     The color value that represents the MediumPurple named color.
        Colors.MediumPurple,
        //
        // Summary:
        //     Gets the color value that represents the MediumSeaGreen named color.
        //
        // Returns:
        //     The color value that represents the MediumSeaGreen named color.
        Colors.MediumSeaGreen,
        //
        // Summary:
        //     Gets the color value that represents the MediumSlateBlue named color.
        //
        // Returns:
        //     The color value that represents the MediumSlateBlue named color.
        Colors.MediumSlateBlue,
        //
        // Summary:
        //     Gets the color value that represents the MediumSpringGreen named color.
        //
        // Returns:
        //     The color value that represents the MediumSpringGreen named color.
        Colors.MediumSpringGreen,
        //
        // Summary:
        //     Gets the color value that represents the MediumTurquoise named color.
        //
        // Returns:
        //     The color value that represents the MediumTurquoise named color.
        Colors.MediumTurquoise,
        //
        // Summary:
        //     Gets the color value that represents the MediumVioletRed named color.
        //
        // Returns:
        //     The color value that represents the MediumVioletRed named color.
        Colors.MediumVioletRed,
        //
        // Summary:
        //     Gets the color value that represents the MidnightBlue named color.
        //
        // Returns:
        //     The color value that represents the MidnightBlue named color.
        Colors.MidnightBlue,
        //
        // Summary:
        //     Gets the color value that represents the MintCream named color.
        //
        // Returns:
        //     The color value that represents the MintCream named color.
        Colors.MintCream,
        //
        // Summary:
        //     Gets the color value that represents the MistyRose named color.
        //
        // Returns:
        //     The color value that represents the MistyRose named color.
        Colors.MistyRose,
        //
        // Summary:
        //     Gets the color value that represents the Moccasin named color.
        //
        // Returns:
        //     The color value that represents the Moccasin named color.
        Colors.Moccasin,
        //
        // Summary:
        //     Gets the color value that represents the NavajoWhite named color.
        //
        // Returns:
        //     The color value that represents the NavajoWhite named color.
        Colors.NavajoWhite,
        //
        // Summary:
        //     Gets the color value that represents the Navy named color.
        //
        // Returns:
        //     The color value that represents the Navy named color.
        Colors.Navy,
        //
        // Summary:
        //     Gets the color value that represents the OldLace named color.
        //
        // Returns:
        //     The color value that represents the OldLace named color.
        Colors.OldLace,
        //
        // Summary:
        //     Gets the color value that represents the Olive named color.
        //
        // Returns:
        //     The color value that represents the Olive named color.
        Colors.Olive,
        //
        // Summary:
        //     Gets the color value that represents the OliveDrab named color.
        //
        // Returns:
        //     The color value that represents the OliveDrab named color.
        Colors.OliveDrab,
        //
        // Summary:
        //     Gets the system-defined color that has the ARGB value of #FFFFA500..
        //
        // Returns:
        //     The system-defined color that has the ARGB value of #FFFFA500..
        Colors.Orange,
        //
        // Summary:
        //     Gets the color value that represents the OrangeRed named color.
        //
        // Returns:
        //     The color value that represents the OrangeRed named color.
        Colors.OrangeRed,
        //
        // Summary:
        //     Gets the color value that represents the Orchid named color.
        //
        // Returns:
        //     The color value that represents the Orchid named color.
        Colors.Orchid,
        //
        // Summary:
        //     Gets the color value that represents the PaleGoldenrod named color.
        //
        // Returns:
        //     The color value that represents the PaleGoldenrod named color.
        Colors.PaleGoldenrod,
        //
        // Summary:
        //     Gets the color value that represents the PaleGreen named color.
        //
        // Returns:
        //     The color value that represents the PaleGreen named color.
        Colors.PaleGreen,
        //
        // Summary:
        //     Gets the color value that represents the PaleTurquoise named color.
        //
        // Returns:
        //     The color value that represents the PaleTurquoise named color.
        Colors.PaleTurquoise,
        //
        // Summary:
        //     Gets the color value that represents the PaleVioletRed named color.
        //
        // Returns:
        //     The color value that represents the PaleVioletRed named color.
        Colors.PaleVioletRed,
        //
        // Summary:
        //     Gets the color value that represents the PapayaWhip named color.
        //
        // Returns:
        //     The color value that represents the PapayaWhip named color.
        Colors.PapayaWhip,
        //
        // Summary:
        //     Gets the color value that represents the PeachPuff named color.
        //
        // Returns:
        //     The color value that represents the PeachPuff named color.
        Colors.PeachPuff,
        //
        // Summary:
        //     Gets the color value that represents the Peru named color.
        //
        // Returns:
        //     The color value that represents the Peru named color.
        Colors.Peru,
        //
        // Summary:
        //     Gets the color value that represents the Pink named color.
        //
        // Returns:
        //     The color value that represents the Pink named color.
        Colors.Pink,
        //
        // Summary:
        //     Gets the color value that represents the Plum named color.
        //
        // Returns:
        //     The color value that represents the Plum named color.
        Colors.Plum,
        //
        // Summary:
        //     Gets the color value that represents the PowderBlue named color.
        //
        // Returns:
        //     The color value that represents the PowderBlue named color.
        Colors.PowderBlue,
        //
        // Summary:
        //     Gets the system-defined color that has the ARGB value of #FF800080..
        //
        // Returns:
        //     The system-defined color that has the ARGB value of #FF800080..
        Colors.Purple,
        //
        // Summary:
        //     Gets the system-defined color that has the ARGB value of #FFFF0000.
        //
        // Returns:
        //     The system-defined color that has the ARGB value of #FFFF0000.
        Colors.Red,
        //
        // Summary:
        //     Gets the color value that represents the RosyBrown named color.
        //
        // Returns:
        //     The color value that represents the RosyBrown named color.
        Colors.RosyBrown,
        //
        // Summary:
        //     Gets the color value that represents the RoyalBlue named color.
        //
        // Returns:
        //     The color value that represents the RoyalBlue named color.
        Colors.RoyalBlue,
        //
        // Summary:
        //     Gets the color value that represents the SaddleBrown named color.
        //
        // Returns:
        //     The color value that represents the SaddleBrown named color.
        Colors.SaddleBrown,
        //
        // Summary:
        //     Gets the color value that represents the Salmon named color.
        //
        // Returns:
        //     The color value that represents the Salmon named color.
        Colors.Salmon,
        //
        // Summary:
        //     Gets the color value that represents the SandyBrown named color.
        //
        // Returns:
        //     The color value that represents the SandyBrown named color.
        Colors.SandyBrown,
        //
        // Summary:
        //     Gets the color value that represents the SeaGreen named color.
        //
        // Returns:
        //     The color value that represents the SeaGreen named color.
        Colors.SeaGreen,
        //
        // Summary:
        //     Gets the color value that represents the SeaShell named color.
        //
        // Returns:
        //     The color value that represents the SeaShell named color.
        Colors.SeaShell,
        //
        // Summary:
        //     Gets the color value that represents the Sienna named color.
        //
        // Returns:
        //     The color value that represents the Sienna named color.
        Colors.Sienna,
        //
        // Summary:
        //     Gets the color value that represents the Silver named color.
        //
        // Returns:
        //     The color value that represents the Silver named color.
        Colors.Silver,
        //
        // Summary:
        //     Gets the color value that represents the SkyBlue named color.
        //
        // Returns:
        //     The color value that represents the SkyBlue named color.
        Colors.SkyBlue,
        //
        // Summary:
        //     Gets the color value that represents the SlateBlue named color.
        //
        // Returns:
        //     The color value that represents the SlateBlue named color.
        Colors.SlateBlue,
        //
        // Summary:
        //     Gets the color value that represents the SlateGray named color.
        //
        // Returns:
        //     The color value that represents the SlateGray named color.
        Colors.SlateGray,
        //
        // Summary:
        //     Gets the color value that represents the Snow named color.
        //
        // Returns:
        //     The color value that represents the Snow named color.
        Colors.Snow,
        //
        // Summary:
        //     Gets the color value that represents the SpringGreen named color.
        //
        // Returns:
        //     The color value that represents the SpringGreen named color.
        Colors.SpringGreen,
        //
        // Summary:
        //     Gets the color value that represents the SteelBlue named color.
        //
        // Returns:
        //     The color value that represents the SteelBlue named color.
        Colors.SteelBlue,
        //
        // Summary:
        //     Gets the color value that represents the Tan named color.
        //
        // Returns:
        //     The color value that represents the Tan named color.
        Colors.Tan,
        //
        // Summary:
        //     Gets the color value that represents the Teal named color.
        //
        // Returns:
        //     The color value that represents the Teal named color.
        Colors.Teal,
        //
        // Summary:
        //     Gets the color value that represents the Thistle named color.
        //
        // Returns:
        //     The color value that represents the Thistle named color.
        Colors.Thistle,
        //
        // Summary:
        //     Gets the color value that represents the Tomato named color.
        //
        // Returns:
        //     The color value that represents the Tomato named color.
        Colors.Tomato,
        //
        // Summary:
        //     Gets the system-defined color that has the ARGB value of #00FFFFFF.
        //
        // Returns:
        //     The system-defined color that has the ARGB value of #00FFFFFF.
        Colors.Transparent,
        //
        // Summary:
        //     Gets the color value that represents the Turquoise named color.
        //
        // Returns:
        //     The color value that represents the Turquoise named color.
        Colors.Turquoise,
        //
        // Summary:
        //     Gets the color value that represents the Violet named color.
        //
        // Returns:
        //     The color value that represents the Violet named color.
        Colors.Violet,
        //
        // Summary:
        //     Gets the color value that represents the Wheat named color.
        //
        // Returns:
        //     The color value that represents the Wheat named color.
        Colors.Wheat,
        //
        // Summary:
        //     Gets the system-defined color that has the ARGB value of #FFFFFFFF.
        //
        // Returns:
        //     The system-defined color that has the ARGB value of #FFFFFFFF.
        Colors.White,
        //
        // Summary:
        //     Gets the color value that represents the WhiteSmoke named color.
        //
        // Returns:
        //     The color value that represents the WhiteSmoke named color.
        Colors.WhiteSmoke,
        //
        // Summary:
        //     Gets the system-defined color that has the ARGB value of #FFFFFF00.
        //
        // Returns:
        //     The system-defined color that has the ARGB value of #FFFFFF00.
        Colors.Yellow,
        //
        // Summary:
        //     Gets the color value that represents the YellowGreen named color.
        //
        // Returns:
        //     The color value that represents the YellowGreen named color.
        Colors.YellowGreen
        };

        Random RAND = new Random(DateTime.Now.Millisecond);
        int _colorIdx = 0; // color randomized index for the color array
        TimeSpan _previousElapsedTime;
        TimeSpan _elapsedTime;
        TimeSpan _colorTime;

        Sphero _robot = null;

        public class SpheroColor {
            public Color _main;
            public Color _inner;
            public Color _outter;
            public Color _glow;

            public SpheroColor(Color color)
            {
                //_glow = Color.FromArgb((byte)(Math.Max(color.A - 240, 0)), color.R, color.G, color.B);
                _main = Color.FromArgb(color.A, color.R, color.G, color.B);
                _inner = Color.FromArgb((byte)(Math.Max(color.A - 50,0)), (byte)(Math.Max(color.R - 50, 0)), (byte)(Math.Max(color.G - 50, 0)), (byte)(Math.Max(color.B - 50, 0)));
                _outter = Color.FromArgb((byte)(Math.Max(color.A - 100, 0)), (byte)(Math.Max(color.R - 100, 0)), (byte)(Math.Max(color.G - 100, 0)), (byte)(Math.Max(color.B - 100, 0)));
            }
        }

        public class SpheroCircle
        {
            public float _radius;
            public SpheroColor _color;

            public SpheroCircle(float radius, Color color)
            {
                _radius = radius;
                _color = new SpheroColor(color);
            }

            public void Draw(Vector2 pos, CanvasDrawingSession canvas)
            {
                //canvas.FillCircle(pos, _radius + 30, _color._glow);
                canvas.FillCircle(pos, _radius - 18, _color._main);
                canvas.FillCircle(pos, _radius - 5, _color._inner);
                canvas.FillCircle(pos, _radius, _color._outter);
                

                canvas.FillEllipse(new Vector2(pos.X, pos.Y - 60), _radius - 60, _radius - 90, Color.FromArgb(50, 255, 255, 255)); 
            }
        }


        public MainPage()
        {
            this.InitializeComponent();
        }
    
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            SetupRobotConnection();
            Application app = Application.Current;
            app.Suspending += OnSuspending;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            ShutdownRobotConnection();
            //ShutdownControls();

            Application app = Application.Current;
            app.Suspending -= OnSuspending;
        }


        private void OnSuspending(object sender, SuspendingEventArgs args)
        {
            ShutdownRobotConnection();
        }

        private void SetupRobotConnection()
        {
            //SpheroName.Text = kNoSpheroConnected;

            RobotProvider provider = RobotProvider.GetSharedProvider();
            provider.DiscoveredRobotEvent += OnRobotDiscovered;
            provider.NoRobotsEvent += OnNoRobotsEvent;
            provider.ConnectedRobotEvent += OnRobotConnected;
            provider.FindRobots();
        }
        
        private void ShutdownRobotConnection()
        {
            if (_robot != null)
            {
                _robot.SensorControl.StopAll();
                _robot.Sleep();
                // temporary while I work on Disconnect.
                //m_robot.Disconnect();
                //ConnectionToggle.OffContent = "Disconnected";
                //SpheroName.Text = kNoSpheroConnected;

                _robot.SensorControl.AccelerometerUpdatedEvent -= OnAccelerometerUpdated;
                _robot.SensorControl.AttitudeUpdatedEvent -= SensorControl_AttitudeUpdatedEvent;
                _robot.SensorControl.GyrometerUpdatedEvent -= OnGyrometerUpdated;

                //m_robot.CollisionControl.StopDetection();
                //m_robot.CollisionControl.CollisionDetectedEvent -= OnCollisionDetected;

                RobotProvider provider = RobotProvider.GetSharedProvider();
                provider.DiscoveredRobotEvent -= OnRobotDiscovered;
                provider.NoRobotsEvent -= OnNoRobotsEvent;
                provider.ConnectedRobotEvent -= OnRobotConnected;
            }
        }

        private void OnRobotDiscovered(object sender, Robot robot)
        {
            Debug.WriteLine(string.Format("Discovered \"{0}\"", robot.BluetoothName));

            if (_robot == null)
            {
                RobotProvider provider = RobotProvider.GetSharedProvider();
                provider.ConnectRobot(robot);
                //ConnectionToggle.OnContent = "Connecting...";
                _robot = (Sphero)robot;
                //SpheroName.Text = string.Format(kConnectingToSphero, robot.BluetoothName);
            }
        }


        private void OnNoRobotsEvent(object sender, EventArgs e)
        {
            MessageDialog dialog = new MessageDialog("No Sphero Paired");
            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 1;
            dialog.ShowAsync();
        }

        
        private void OnRobotConnected(object sender, Robot robot)
        {
            Debug.WriteLine(string.Format("Connected to {0}", robot));
            //ConnectionToggle.IsOn = true;
            //ConnectionToggle.OnContent = "Connected";

            _robot.SetBackLED(127);
            //m_robot.SetRGBLED(255, 255, 255);
            //SpheroName.Text = string.Format(kSpheroConnected, robot.BluetoothName);
            //SetupControls();

            //m_robot.SetHeading(0);

            //m_robot.SensorControl.StopAll();

            // stop rotors
            _robot.WriteToRobot(new DeviceMessage(2, 0x33, new byte[] { 0, 0, 0, 0 }));

            _robot.SensorControl.Hz = 10;

            _robot.SensorControl.AccelerometerUpdatedEvent += OnAccelerometerUpdated;
            _robot.SensorControl.AttitudeUpdatedEvent += SensorControl_AttitudeUpdatedEvent;
            _robot.SensorControl.GyrometerUpdatedEvent += OnGyrometerUpdated;

            //m_robot.CollisionControl.StartDetectionForWallCollisions();
            //m_robot.CollisionControl.CollisionDetectedEvent += OnCollisionDetected;
        }

        private void SensorControl_AttitudeUpdatedEvent(object sender, AttitudeReading reading)
        {
            //AtittudeRoll.Text = "" + reading.Roll;
            //AtittudePitch.Text = "" + reading.Pitch;
            //AtittudeYaw.Text = "" + reading.Yaw;
        }

        //private void ConnectionToggle_Toggled(object sender, RoutedEventArgs e)
        //{
        //    Debug.WriteLine("Connection Toggled : " + ConnectionToggle.IsOn);
        //    //ConnectionToggle.OnContent = "Connecting...";
        //    if (ConnectionToggle.IsOn)
        //    {
        //        if (m_robot == null)
        //        {
        //            SetupRobotConnection();
        //        }
        //    }
        //    else {
        //        ShutdownRobotConnection();
        //    }
        //}

        private void OnAccelerometerUpdated(object sender, AccelerometerReading reading)
        {
            //AccelerometerX.Text = "" + reading.X;
            //AccelerometerY.Text = "" + reading.Y;
            //AccelerometerZ.Text = "" + reading.Z;
        }

        private void OnGyrometerUpdated(object sender, GyrometerReading reading)
        {
            //GyroscopeX.Text = "" + reading.X;
            //GyroscopeY.Text = "" + reading.Y;
            //GyroscopeZ.Text = "" + reading.Z;
        }


        private void CanvasAnimatedControl_Update(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
        {
            _previousElapsedTime = _elapsedTime;
            _elapsedTime = args.Timing.ElapsedTime;

            _colorTime += _elapsedTime;

            if (_colorTime.Milliseconds > 500)
            {
                _colorIdx = RAND.Next(0, randomColors.Length);
                _colorTime = TimeSpan.Zero;

                // TODO change this to a proper place

                _robot.SetRGBLED(randomColors[_colorIdx].R, randomColors[_colorIdx].G, randomColors[_colorIdx].B);
            }
        }

        private void CanvasAnimatedControl_Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            sender.ClearColor = Colors.Black;
            

            Vector2 centerScreen = new Vector2((float)sender.Size.Width / 2, ((float)sender.Size.Height / 2f) + 20);
            Vector2 centerShadow = new Vector2((float)sender.Size.Width / 2, ((float)sender.Size.Height / 2f) + 180);


            //args.DrawingSession.DrawEllipse(155, 115, 80, 30, Colors.Black, 3);
            args.DrawingSession.DrawText("Sensorial Rhythm", 100, 100, Colors.Red);


            var gradientStops = new CanvasGradientStop[]
            {
                new CanvasGradientStop { Position = 0, Color = randomColors[_colorIdx] },
                new CanvasGradientStop { Position = 1, Color = Colors.Transparent }
            };

            //var middle = new Vector2((float)(sender.Size.Width / 2), (float)(sender.Size.Height / 2));

            var brush = new CanvasRadialGradientBrush(args.DrawingSession,
            gradientStops,
            CanvasEdgeBehavior.Mirror,
            CanvasAlphaMode.Premultiplied
            )
            {
                Center = new Vector2(centerShadow.X, centerShadow.Y),
                RadiusX = 300,
                RadiusY = 50,
            };

            //using (args.DrawingSession.CreateLayer(gradientBrush))
            {
                args.DrawingSession.FillEllipse(centerShadow, 300, 50, brush);
            }

            
            
            // Color.FromArgb(255,0,192,0) // green
            SpheroCircle sphero = new SpheroCircle(150, randomColors[_colorIdx]);
            sphero.Draw(centerScreen, args.DrawingSession);

            

            //var circleCenter = new Vector2(300, 300);
            //SpheroCircle sphero = new SpheroCircle(150, Colors.Green);
            //sphero.Draw(circleCenter, args.DrawingSession);

            //circleCenter = new Vector2(500, 300);
            //SpheroCircle sphero2 = new SpheroCircle(150, Colors.Blue);
            //sphero2.Draw(circleCenter, args.DrawingSession);

            //circleCenter = new Vector2(700, 300);
            //SpheroCircle sphero3 = new SpheroCircle(150, Colors.Red);
            //sphero3.Draw(circleCenter, args.DrawingSession);
        }

    }
}
