using SldWorks;
using SwConst;

namespace AutoBalloon
{
    class Program
    {
        static void Main(string[] args)
        {
            var swInstance = new SldWorks.SldWorks();

            var model = (ModelDoc2)swInstance.ActiveDoc;
            var drawing = (DrawingDoc)model;

            var boolStatus = drawing.ActivateView("Drawing View1");

            boolStatus = model.Extension.SelectByID2("Drawing View1",
                "DRAWINGVIEW", 0, 0, 0, false, 0, null, 0);

            var autoballoonParameters = drawing.CreateAutoBalloonOptions();

            autoballoonParameters.Layout = (int)swBalloonLayoutType_e.swDetailingBalloonLayout_Square;
            autoballoonParameters.ReverseDirection = false;
            autoballoonParameters.IgnoreMultiple = true;
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

            model.ClearSelection2(true);
        }
    }
}
