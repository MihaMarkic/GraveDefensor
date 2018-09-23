using GraveDefensor.Engine.Services.Abstract;
using GraveDefensor.Shared.Drawable.Enemies;
using GraveDefensor.Shared.Drawable.Weapons;
using GraveDefensor.Shared.Messages;
using GraveDefensor.Shared.Service.Abstract;
using GraveDefensor.Shared.Services.Implementation;
using Microsoft.Xna.Framework;
using NLog;
using Righthand.MessageBus;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Settings = GraveDefensor.Engine.Settings;

namespace GraveDefensor.Shared.Drawable
{
    public class BattleScene: ScrollableScene
    {
        static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        public WeaponPod[] WeaponPods { get; private set; }
        public EnemyWave[] Waves { get; private set; }
        Settings.Battle settings;
        Settings.Weapon[] weaponsSettings;
        public int Health { get; private set; }
        public int Amount { get; private set; }
        public WeaponPickerDialog WeaponPickerDialog { get; private set; }
        Settings.Size windowSize;
        Subscription changeStatusSubscription;
        /// <summary>
        /// Is in scrolling mode, when user drags it horizontally
        /// </summary>
        public bool IsScrolling { get; private set; }
        IInitContext initContext;
        IInitContentContext initContentContext;
        public Dialog ActiveDialog => WeaponPickerDialog;
        public void Init(IInitContext context, Settings.Battle settings, Settings.Enemies enemiesSettings, Settings.Weapon[] weaponsSettings, Settings.Size windowSize)
        {
            initContext = context;
            this.settings = settings;
            this.weaponsSettings = weaponsSettings;
            this.windowSize = windowSize;
            Health = settings.Health;
            Amount = settings.Amount;
            WeaponPods = new WeaponPod[settings.WeaponPlaces.Length];
            for (int i = 0; i < settings.WeaponPlaces.Length; i++)
            {
                var setting = settings.WeaponPlaces[i];
                var wp = context.ObjectPool.GetObject<WeaponPod>();
                wp.Init(setting);
                WeaponPods[i] = wp;
            }
            Waves = new EnemyWave[settings.Waves.Length];
            for (int i=0; i<settings.Waves.Length; i++)
            {
                var setting = settings.Waves[i];
                var wave = context.ObjectPool.GetObject<EnemyWave>();
                var enemySettings = enemiesSettings.GetEnemyById(setting.EnemyId);
                var pathSettings = settings.Paths.Single(p => string.Equals(p.Id, setting.PathId, StringComparison.Ordinal));
                wave.Init(context, setting, enemySettings, pathSettings);
                Waves[i] = wave;
            }
            changeStatusSubscription = context.Dispatcher.Subscribe<ChangeStatusMessage>(OnChangeStatus);
        }
        internal void OnChangeStatus(object key, ChangeStatusMessage message)
        {
            Health += message.Health;
            Amount += message.Amount;
        }
        public override void InitContent(IInitContentContext context)
        {
            initContentContext = context;
            foreach (var wp in WeaponPods)
            {
                wp.InitContent(context);
            }
            foreach (var wave in Waves)
            {
                wave.InitContent(context);
            }
            base.InitContent(context);
        }
        /// <summary>
        /// Click on entity is allowed only when not scrolling and dialog is absent
        /// </summary>
        internal bool CanEntityClick => !IsScrolling && WeaponPickerDialog == null;
        public override void Update(UpdateContext context)
        {
            // converts mouse coordinates to absolute to scene
            var childContext = OffsetUpdateContext(context);
            foreach (var wp in WeaponPods)
            {
                wp.Update(context, Waves);
                if (CanEntityClick && wp.ClickState == ClickState.Clicked)
                {
                    logger.Info($"Weapon pod {Array.IndexOf(WeaponPods, wp)} was clicked");
                    WeaponPickerDialog = CreateWeaponPickerDialog(context.ObjectPool, wp);
                }
            }
            foreach (var wave in Waves)
            {
                wave.Update(context);
            }
            var activeDialog = ActiveDialog;
            if (activeDialog != null)
            {
                activeDialog.Update(context, Amount);
                if (activeDialog.State == DialogState.Closing)
                {
                    context.ObjectPool.ReleaseObject(activeDialog);
                    WeaponPickerDialog = null;
                }
            }
            base.Update(context);
        }
        internal WeaponPickerDialog CreateWeaponPickerDialog(IObjectPool pool, WeaponPod pod)
        {
            Debug.Assert(ActiveDialog is null);
            var dialog = pool.GetObject<WeaponPickerDialog>();
            dialog.Init(initContext, pod, GetDialogTopLeft(pod.Bounds), weaponsSettings);
            dialog.InitContent(initContentContext);
            return dialog;
        }
        internal Point GetDialogTopLeft(Rectangle entityBounds)
        {
            return new Point(entityBounds.Right, entityBounds.Top);
        }
        public override void Draw(IDrawContext context)
        {
            foreach (var wp in WeaponPods)
            {
                wp.Draw(context);
            }
            foreach (var wave in Waves)
            {
                wave.Draw(context);
            }
            if (Globals.ShowPaths)
            {
                foreach (var path in settings.Paths)
                {
                    DrawPath(context, path);
                }
            }
            DrawHeader(context);
            ActiveDialog?.Draw(context);
            base.Draw(context);
        }
        void DrawHeader(IDrawContext context)
        {
            const float BetweenColumns = 80;
            const float BetweenRows = 20;
            const float Top = 10;
            const float Left = 10;
            context.FillRectangle(new Rectangle(0, 0, windowSize.Width, 60), new Color(Color.Black, 0.2f));
            context.DrawString(GlobalContent.Default.HudFont, "Health", new Vector2(Left, Top), Color.Yellow);
            context.DrawString(GlobalContent.Default.HudFont, Health.ToString("#,##0"), new Vector2(Left, Top+BetweenRows), Color.Yellow);
            context.DrawString(GlobalContent.Default.HudFont, "Amount", new Vector2(Left+BetweenColumns, Top), Color.Yellow);
            context.DrawString(GlobalContent.Default.HudFont, Amount.ToString("#,##0"), new Vector2(Left + BetweenColumns, Top+BetweenRows), Color.Yellow);

        }
        void DrawPath(IDrawContext context, Settings.Path path)
        {
            for (int i = 1; i < path.Points.Length; i++)
            {
                var prev = path.Points[i - 1];
                var next = path.Points[i];
                context.DrawLine(prev.X, prev.Y, next.X, next.Y, Color.Yellow, 3f);
            }
        }

        public override void ReleaseResources(IObjectPool objectPool)
        {
            objectPool.ReleaseObjects(WeaponPods);
            objectPool.ReleaseObject(WeaponPickerDialog);
            changeStatusSubscription.Dispose();
            base.ReleaseResources(objectPool);
        }
    }
}
