using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace NestTemplatedControls.Controls
{
    public sealed class CollapsibleContentControl : ContentControl
    {
        static class PartName
        {
            public const string ContentPresenter = "part_ContentPresenter";
        }

        private Grid _contentPresenter;

        public CollapsibleContentControl()
        {
            this.DefaultStyleKey = typeof(CollapsibleContentControl);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _contentPresenter = GetPart<Grid>(PartName.ContentPresenter);
            if (_contentPresenter == null) { throw new NullReferenceException(PartName.ContentPresenter + " was not found in the ControlTemplate"); }

            SetStateToCollapsed(IsCollapsed);
        }

        #region IsCollapsed
        public static readonly DependencyProperty IsCollapsedProperty = DependencyProperty.Register("IsCollapsed", typeof(bool), typeof(CollapsibleContentControl), new PropertyMetadata(false, new PropertyChangedCallback(OnIsCollapsedChanged)));

        private static void OnIsCollapsedChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            CollapsibleContentControl collapsibleContentControl = o as CollapsibleContentControl;
            if (collapsibleContentControl != null)
                collapsibleContentControl.OnIsCollapsedChanged((bool)e.OldValue, (bool)e.NewValue);
        }

        internal void OnIsCollapsedChanged(bool oldValue, bool newValue)
        {
            SetStateToCollapsed(newValue);
        }

        public bool IsCollapsed
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get
            {
                return (bool)GetValue(IsCollapsedProperty);
            }
            set
            {
                SetValue(IsCollapsedProperty, value);
            }
        }
        #endregion

        private void SetStateToCollapsed(bool isCollapsed)
        {
            string state = isCollapsed
                         ? "HideContentState"
                         : "ShowContentState";
            VisualStateManager.GoToState(this, state, true);
        }

        private T GetPart<T>(string partName) where T : DependencyObject
        {
            return GetTemplateChild(partName) as T;
        }
    }
}
