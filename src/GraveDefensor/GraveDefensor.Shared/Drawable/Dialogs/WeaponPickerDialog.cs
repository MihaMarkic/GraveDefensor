using Settings = GraveDefensor.Engine.Settings;
using Microsoft.Xna.Framework;
using GraveDefensor.Shared.Service.Abstract;
using GraveDefensor.Shared.Drawable.Buttons;
using GraveDefensor.Engine.Services.Abstract;
using GraveDefensor.Shared.Services.Implementation;

namespace GraveDefensor.Shared.Drawable
{
    public class WeaponPickerDialog : Dialog
    {
        public const int HorizontalPadding = 5;
        public WeaponPod Pod { get; private set; }
        Settings.Weapon[] weaponSettings;
        public WeaponPickerButton[] Buttons { get; private set; }
        public void Init(IInitContext initContext, WeaponPod pod, Point topLeft, Settings.Weapon[] weaponSettings)
        {
            const int width = 300;
            const int headerHeight = 30;
            Pod = pod;
            this.weaponSettings = weaponSettings;
            int height = MeasureContentHeight(weaponSettings.Length);
            Init(topLeft, width, height, headerHeight: headerHeight, null);
            Buttons = CreateButtons(initContext.ObjectPool, weaponSettings);
        }

        internal static int MeasureContentHeight(int numberOfButtons)
        {
            return 2 + (numberOfButtons-1) * HorizontalPadding + numberOfButtons * WeaponPickerButton.Height;
        }
        internal WeaponPickerButton[] CreateButtons(IObjectPool pool, Settings.Weapon[] weaponSettings)
        {
            const int delta = 5;
            
            Point topLeftButton = new Point(TopLeft.X + HorizontalPadding, TopLeft.Y + 5 + HeaderHeight.Value);
            var buttons = new WeaponPickerButton[weaponSettings.Length];
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i] = pool.GetObject<WeaponPickerButton>();
                buttons[i].Init(weaponSettings[i], topLeftButton, Width - 2 * HorizontalPadding);
                topLeftButton = new Point(topLeftButton.X, topLeftButton.Y + WeaponPickerButton.Height + delta);
            }
            return buttons;
        }
        public override void InitContent(IInitContentContext context)
        {
            foreach (var button in Buttons)
            {
                button.InitContent(context);
            }
            base.InitContent(context);
        }
        public override void Update(UpdateContext context, int currentAmount)
        {
            foreach (var button in Buttons)
            {
                button.Update(context, currentAmount);
            }
            base.Update(context, currentAmount);
        }
        internal override void DrawContent(IDrawContext context)
        {
            context.FillRectangle(new Rectangle(TopLeft, new Point(Width, HeaderHeight.Value)), new Color(Color.White, 0.2f));
            context.DrawString(GlobalContent.Default.CoordinatesFont, "Pick weapon", TopLeft.AsVector2(), Color.Black);
            foreach (var button in Buttons)
            {
                button.Draw(context);
            }
        }

        public override void ReleaseResources(IObjectPool objectPool)
        {
            objectPool.ReleaseObjects(Buttons);
            base.ReleaseResources(objectPool);
        }
    }
}
