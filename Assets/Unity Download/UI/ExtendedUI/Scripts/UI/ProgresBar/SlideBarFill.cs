using UnityEngine.UI;

namespace DIG.UIExpansion
{
    public class SlideBarFill : BarFill
    {
        public override void InitilizeSliderBar()
        {
            FillImage.type = Image.Type.Filled;
            FillImage.fillMethod = Image.FillMethod.Horizontal;
        }
    }
}
