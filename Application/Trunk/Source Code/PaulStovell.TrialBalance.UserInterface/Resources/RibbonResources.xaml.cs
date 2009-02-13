using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media;

namespace PaulStovell.TrialBalance.UserInterface.Resources
{
    public partial class RibbonResources : ResourceDictionary
    {
        private class FadeInformation
        {
            private Color[] _backgroundColors;
            private Color _borderColor;
            private int _durationMilliseconds;
            private Color _foregroundColor;

            public Color[] BackgroundColors
            {
                get { return _backgroundColors; }
                set { _backgroundColors = value; }
            }

            public Color BorderColor
            {
                get { return _borderColor; }
                set { _borderColor = value; }
            }

            public Color ForegroundColor
            {
                get { return _foregroundColor; }
                set { _foregroundColor = value; }
            }

            public int DurationMilliseconds
            {
                get { return _durationMilliseconds; }
                set { _durationMilliseconds = value; }
            }
        }

        private FadeInformation _activeFadeInformation;
        private FadeInformation _inactiveFadeInformation;
        private FadeInformation _activeHoverFadeInformation;
        private FadeInformation _inactiveHoverFadeInformation;

        public RibbonResources()
        {
            _activeHoverFadeInformation = new FadeInformation();
            _activeHoverFadeInformation.BackgroundColors = new Color[] { Color.FromArgb(0xFF, 0xF0, 0xF6, 0xFE), Color.FromArgb(0xFF, 0xDB, 0xE6, 0xF5) };
            _activeHoverFadeInformation.BorderColor = Colors.Orange;
            _activeHoverFadeInformation.ForegroundColor = Colors.Navy;
            _activeHoverFadeInformation.DurationMilliseconds = 100;

            _inactiveHoverFadeInformation = new FadeInformation();
            _inactiveHoverFadeInformation.BackgroundColors = new Color[] { Color.FromArgb(0xFF, 0xC8, 0xDA, 0xED), Color.FromArgb(0xFF, 0xE4, 0xE5, 0xDD) };
            _inactiveHoverFadeInformation.BorderColor = Colors.Transparent;
            _inactiveHoverFadeInformation.ForegroundColor = Colors.Navy;
            _inactiveHoverFadeInformation.DurationMilliseconds = 100;

            _activeFadeInformation = new FadeInformation();
            _activeFadeInformation.BackgroundColors = new Color[] { Color.FromArgb(0xFF, 0xF0, 0xF6, 0xFE), Color.FromArgb(0xFF, 0xDB, 0xE6, 0xF5) };
            _activeFadeInformation.BorderColor = Colors.Transparent;
            _activeFadeInformation.ForegroundColor = Colors.Navy;
            _activeFadeInformation.DurationMilliseconds = 500;

            _inactiveFadeInformation = new FadeInformation();
            _inactiveFadeInformation.BackgroundColors = new Color[] { Colors.Transparent, Colors.Transparent };
            _inactiveFadeInformation.BorderColor = Colors.Transparent;
            _inactiveFadeInformation.ForegroundColor = Colors.Navy;
            _inactiveFadeInformation.DurationMilliseconds = 500;
        }

        protected void TabControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                TabControl tabControl = (TabControl)e.Source;
                tabControl.SelectionChanged += new SelectionChangedEventHandler(TabControl_SelectionChanged);
            }
        }

        protected void TabItem_Loaded(object sender, RoutedEventArgs e)
        {
            if (e.Source is TabItem)
            {
                TabItem tabItem = (TabItem)e.Source;
                tabItem.MouseEnter += delegate(object delegateSender, MouseEventArgs delegateEventArgs) { PerformEnterAction(delegateEventArgs.OriginalSource); };
                tabItem.MouseLeave += delegate(object delegateSender, MouseEventArgs delegateEventArgs) { PerformExitAction(delegateEventArgs.OriginalSource); };
                ResetItem(tabItem);
            }
        }

        void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                foreach (TabItem item in ((TabControl)e.Source).Items)
                {
                    ResetItem(item);
                }
            }
        }

        private void ResetItem(object tabItem)
        {
            if (tabItem is TabItem)
            {
                if (((TabItem)tabItem).IsMouseOver)
                {
                    PerformEnterAction(tabItem);
                }
                else
                {
                    PerformExitAction(tabItem);
                }
            }
        }

        private void PerformEnterAction(object tabItem)
        {
            SelectFadeAnimation(
                _activeHoverFadeInformation,
                _inactiveHoverFadeInformation,
                tabItem);
        }

        private void PerformExitAction(object tabItem)
        {
            SelectFadeAnimation(
               _activeFadeInformation,
               _inactiveFadeInformation,
               tabItem);
        }

        private void SelectFadeAnimation(
            FadeInformation selectedColors,
            FadeInformation nonSelectedColors, 
            object originalSource)
        {
            if (originalSource is TabItem)
            {
                TabItem tabItem = (TabItem)originalSource;

                if (tabItem.IsSelected)
                {
                    ApplyFadeAnimation(tabItem, selectedColors);
                }
                else
                {
                    ApplyFadeAnimation(tabItem, nonSelectedColors);
                }
            }
        }

        private void ApplyFadeAnimation(TabItem tabItem, FadeInformation fadeInfo)
        {
            Border border = (Border)tabItem.Template.FindName("_border", tabItem);
            TextBlock textBlock = (TextBlock)tabItem.Template.FindName("_textBlock", tabItem);
            
            Storyboard storyboard = new Storyboard();

            // Initialize a color animation for every gradient stop we'll be animating. We assume the colors are passed in order.
            ColorAnimation[] colorAnimations = new ColorAnimation[fadeInfo.BackgroundColors.Length];
            for (int i = 0; i < fadeInfo.BackgroundColors.Length; i++)
            {
                colorAnimations[i] = new ColorAnimation();
                colorAnimations[i].To = fadeInfo.BackgroundColors[i];
                colorAnimations[i].Duration = new Duration(TimeSpan.FromMilliseconds(fadeInfo.DurationMilliseconds));
                storyboard.Children.Add(colorAnimations[i]);
                Storyboard.SetTargetProperty(colorAnimations[i], new PropertyPath("Background.GradientStops[" + i.ToString() + "].Color", border));
            }

            ColorAnimation borderAnimation = new ColorAnimation();
            borderAnimation.To = fadeInfo.BorderColor;
            borderAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(fadeInfo.DurationMilliseconds));
            storyboard.Children.Add(borderAnimation);
            Storyboard.SetTargetProperty(borderAnimation, new PropertyPath("BorderBrush.Color", border));
            
            storyboard.Begin(border);

            Storyboard s2 = new Storyboard();

            ColorAnimation foregroundAnimation = new ColorAnimation();
            foregroundAnimation.To = fadeInfo.ForegroundColor;
            foregroundAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(fadeInfo.DurationMilliseconds));
            s2.Children.Add(foregroundAnimation);
            Storyboard.SetTargetProperty(foregroundAnimation, new PropertyPath("Foreground.Color", textBlock));

            s2.Begin(textBlock);
        
        }
    }
}
