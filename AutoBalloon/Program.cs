using SldWorks;
using SwConst;
using System.Threading;

namespace AutoBalloon
{
    class Program
    {
        static void Main(string[] args)
        {
            var swInstance = new SldWorks.SldWorks();
            int errors = 0;

            // must suppress cover to balloon hidden features
            var delay = 300;
            var assemblyName = @"C:\Users\bolinger\Documents\SolidWorks Projects\Prefab Blob - Cover Blob\blob - L2\blob.L2_cover.SLDASM";
            var model = (ModelDoc2)swInstance.ActivateDoc3(assemblyName, true,
                (int)swRebuildOnActivation_e.swRebuildActiveDoc, ref errors);
            Thread.Sleep(delay);
            // write to blob.L2_cover.txt '0' to the field "Is Dimensioned"
            var coverConfigPath = @"C:\Users\bolinger\Documents\SolidWorks Projects\Prefab Blob - Cover Blob\blob - L2\blob.L2_cover.txt";
            var coverConfigLines = System.IO.File.ReadAllLines(coverConfigPath);
            var index = 0;
            var stop = false;
            while (!stop)
            {
                var line = coverConfigLines[index];

                if (line.Contains("Is Dimensioned 2 Bool") &&
                    !line.Contains("IIF"))
                {
                    stop = true;
                    var newLine = coverConfigLines[index].Replace("1", "0");

                    coverConfigLines[index] = newLine;
                }
                else
                {
                    ++index;
                }
            }
            System.IO.File.WriteAllLines(coverConfigPath, coverConfigLines);
            Thread.Sleep(delay);
            model.ForceRebuild3(false);
            Thread.Sleep(delay);

            // switch to drawing
            var coverDrawingFileName = @"C:\Users\bolinger\Documents\SolidWorks Projects\Prefab Blob - Cover Blob\base blob - L1\blob.cover.SLDDRW";
            model = (ModelDoc2)swInstance.ActivateDoc3(coverDrawingFileName, true,
                (int)swRebuildOnActivation_e.swRebuildActiveDoc, ref errors);

            // draw balloons
            var drawing = (DrawingDoc)model;
            var boolStatus = drawing.ActivateView("Drawing View1");

            boolStatus = model.Extension.SelectByID2("Drawing View1",
                "DRAWINGVIEW", 0, 0, 0, false, 0, null, 0);

            var autoballoonParameters = drawing.CreateAutoBalloonOptions();
            
            autoballoonParameters.Layout = (int)swBalloonLayoutType_e.swDetailingBalloonLayout_Square;
            autoballoonParameters.ReverseDirection = false;
            autoballoonParameters.IgnoreMultiple = false;
            autoballoonParameters.InsertMagneticLine = true;
            autoballoonParameters.LeaderAttachmentToFaces = true;
            autoballoonParameters.Style = (int)swBalloonStyle_e.swBS_Box;
            autoballoonParameters.Size = (int)swBalloonFit_e.swBF_2Chars;
            autoballoonParameters.UpperTextContent = (int)swBalloonTextContent_e.swBalloonTextItemNumber;
            autoballoonParameters.Layername = "Format";
            autoballoonParameters.ItemNumberStart = 1;
            autoballoonParameters.ItemNumberIncrement = 1;
            autoballoonParameters.ItemOrder = (int)swBalloonItemNumbersOrder_e.swBalloonItemNumbers_DoNotChangeItemNumbers;
            autoballoonParameters.EditBalloons = true;
            autoballoonParameters.EditBalloonOption = (int)swEditBalloonOption_e.swEditBalloonOption_Resequence;

            var vNotes = drawing.AutoBalloon5(autoballoonParameters);

            // Drawing View2 if exists
            var status = drawing.ActivateView("Drawing View2");
            if (status)
            {
                boolStatus = model.Extension.SelectByID2("Drawing View2",
                    "DRAWINGVIEW", 0, 0, 0, false, 0, null, 0);
                drawing.AutoBalloon5(autoballoonParameters);
            }

            model.ClearSelection2(true);

            Thread.Sleep(delay);

            // switch back to assembly and unsuppress cover
            model = (ModelDoc2)swInstance.ActivateDoc3(assemblyName, true,
                (int)swRebuildOnActivation_e.swRebuildActiveDoc, ref errors);
            Thread.Sleep(delay);
            // write to config
            coverConfigLines[index] = coverConfigLines[index].Replace("0", "1");
            System.IO.File.WriteAllLines(coverConfigPath, coverConfigLines);
            Thread.Sleep(delay);
            model.ForceRebuild3(false);
            Thread.Sleep(delay);
            model.ForceRebuild3(false);
            Thread.Sleep(delay);

            // back to drawing
            swInstance.ActivateDoc3(coverDrawingFileName, true,
                (int)swRebuildOnActivation_e.swRebuildActiveDoc, ref errors);
        }
    }
}
