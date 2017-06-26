using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace NestTemplatedControls.Controls
{
    public sealed class LabeledContentControl : ContentControl
    {
        static class PartName
        {
            public const string IsRequiredTextBlock = "part_IsRequiredTextBlock";
            public const string InputIndicator = "part_InputIndicator";
            public const string InputIndicatorBorder = "part_InputIndicatorBorder";
            public const string LabelTextBlock = "part_LabelTextBlock";
        }

        TextBlock _labelTextBlock;
        TextBlock _requiredTextBlock;
        Brush _requiredBrush;
        Brush _notRequiredBrush;
        Grid _inputIndicatorGrid;
        Border _inputIndicatorBorder;


        public LabeledContentControl()
        {
            DefaultStyleKey = typeof(LabeledContentControl);

        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _labelTextBlock = GetPart<TextBlock>(PartName.LabelTextBlock);
            if (_labelTextBlock == null) { throw new NullReferenceException(PartName.LabelTextBlock + " was not found in the ControlTemplate"); }

            _requiredTextBlock = GetPart<TextBlock>(PartName.IsRequiredTextBlock);
            if (_requiredTextBlock == null) { throw new NullReferenceException(PartName.IsRequiredTextBlock + " was not found in the ControlTemplate"); }

            _inputIndicatorGrid = GetPart<Grid>(PartName.InputIndicator);
            if (_inputIndicatorGrid == null) { throw new NullReferenceException(PartName.InputIndicator + " was not found in the ControlTemplate"); }

            _inputIndicatorBorder = GetPart<Border>(PartName.InputIndicatorBorder);
            if (_inputIndicatorBorder == null) { throw new NullReferenceException(PartName.InputIndicatorBorder + " was not found in the ControlTemplate"); }

            _requiredBrush = Application.Current.Resources["AlertBrush"] as Brush;
            if (_requiredBrush == null) { throw new NullReferenceException("The AlertBrush resource was not found."); }

            if (LabelTextSize != 0)
                _labelTextBlock.FontSize = LabelTextSize;
            _notRequiredBrush = _inputIndicatorBorder.BorderBrush;

            _labelTextBlock.Text = LabelText;
            _inputIndicatorGrid.Visibility = ShowInputIndicator ? Visibility.Visible : Visibility.Collapsed;
            _requiredTextBlock.Visibility = IsRequired ? Visibility.Visible : Visibility.Collapsed;
            _requiredTextBlock.Text = "Required";
            _inputIndicatorBorder.BorderBrush = IsRequired ? _requiredBrush : _notRequiredBrush;

        }

        #region LabelText
        public static readonly DependencyProperty LabelTextProperty = DependencyProperty.Register("LabelText", typeof(string), typeof(LabeledContentControl), new PropertyMetadata(null, new PropertyChangedCallback(OnLabelTextChanged)));

        private static void OnLabelTextChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            LabeledContentControl labeledContentControl = o as LabeledContentControl;
            if (labeledContentControl != null)
                labeledContentControl.OnLabelTextChanged((string)e.OldValue, (string)e.NewValue);
        }

        public double LabelTextSize
        {
            get { return (double)GetValue(LabelTextSizeProperty); }
            set { SetValue(LabelTextSizeProperty, value); }
        }
        // Using a DependencyProperty as the backing store for LabelTextSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelTextSizeProperty =
            DependencyProperty.Register("LabelTextSize", typeof(double), typeof(LabeledContentControl), new PropertyMetadata(null));

        private void OnLabelTextChanged(string oldValue, string newValue)
        {
            if (_labelTextBlock == null && !DesignMode.DesignModeEnabled) { return; }
            if (_labelTextBlock == null && DesignMode.DesignModeEnabled)
            {
                TextBlock textBlock = GetPart<TextBlock>(PartName.LabelTextBlock);
                if (textBlock == null) { return; }

                _labelTextBlock = textBlock;
            }

            _labelTextBlock.Text = newValue;
        }

        public string LabelText
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get
            {
                return (string)GetValue(LabelTextProperty);
            }
            set
            {
                SetValue(LabelTextProperty, value);
            }
        }
        #endregion

        #region ShowInputIndicator
        public static readonly DependencyProperty ShowInputIndicatorProperty = DependencyProperty.Register("ShowInputIndicator", typeof(bool), typeof(LabeledContentControl), new PropertyMetadata(false, new PropertyChangedCallback(OnShowInputIndicatorChanged)));

        private static void OnShowInputIndicatorChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            LabeledContentControl labeledContentControl = o as LabeledContentControl;
            if (labeledContentControl != null)
                labeledContentControl.OnShowInputIndicatorChanged((bool)e.OldValue, (bool)e.NewValue);
        }

        private void OnShowInputIndicatorChanged(bool oldValue, bool newValue)
        {
            if (_inputIndicatorGrid == null && !DesignMode.DesignModeEnabled) { return; }

            if (_inputIndicatorGrid == null && DesignMode.DesignModeEnabled)
            {
                Grid grid = GetPart<Grid>(PartName.InputIndicator);
                if (grid == null) { return; }

                _inputIndicatorGrid = grid;
            }
            _inputIndicatorGrid.Visibility = newValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public bool ShowInputIndicator
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get
            {
                return (bool)GetValue(ShowInputIndicatorProperty);
            }
            set
            {
                SetValue(ShowInputIndicatorProperty, value);
            }
        }
        #endregion

        #region IsRequired
        public static readonly DependencyProperty IsRequiredProperty = DependencyProperty.Register("IsRequired", typeof(bool), typeof(LabeledContentControl), new PropertyMetadata(false, new PropertyChangedCallback(OnIsRequiredChanged)));

        private static void OnIsRequiredChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            LabeledContentControl labeledContentControl = o as LabeledContentControl;
            if (labeledContentControl != null)
                labeledContentControl.OnIsRequiredChanged((bool)e.OldValue, (bool)e.NewValue);
        }

        internal void OnIsRequiredChanged(bool oldValue, bool newValue)
        {
            if (_requiredTextBlock == null && !DesignMode.DesignModeEnabled) { return; }
            if (_requiredTextBlock == null && DesignMode.DesignModeEnabled)
            {
                TextBlock textBlock = GetPart<TextBlock>(PartName.IsRequiredTextBlock);
                if (textBlock == null) { return; }

                _requiredTextBlock = textBlock;
            }

            if (_inputIndicatorBorder == null && !DesignMode.DesignModeEnabled) { return; }
            if (_inputIndicatorBorder == null && DesignMode.DesignModeEnabled)
            {
                Border border = GetPart<Border>(PartName.InputIndicatorBorder);
                if (border == null) { return; }

                _inputIndicatorBorder = border;
                _notRequiredBrush = border.BorderBrush;
            }
            _requiredTextBlock.Visibility = newValue ? Visibility.Visible : Visibility.Collapsed;
            _inputIndicatorBorder.BorderBrush = newValue ? _requiredBrush : _notRequiredBrush;
        }

        public bool IsRequired
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get
            {
                return (bool)GetValue(IsRequiredProperty);
            }
            set
            {
                SetValue(IsRequiredProperty, value);
            }
        }
        #endregion

        private T GetPart<T>(string partName) where T : DependencyObject
        {
            return GetTemplateChild(partName) as T;
        }
        
    }
}
