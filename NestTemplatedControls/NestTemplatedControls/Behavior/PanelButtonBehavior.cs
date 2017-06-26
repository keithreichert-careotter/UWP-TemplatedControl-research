using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace NestTemplatedControls.Behavior
{
    public class PanelButtonBehavior : Behavior<Panel>
    {
        public static readonly DependencyProperty ClickCommandProperty = DependencyProperty.Register("ClickCommand", typeof(ICommand), typeof(PanelButtonBehavior), new PropertyMetadata(null));
        public ICommand ClickCommand
        {
            get { return (ICommand)GetValue(ClickCommandProperty); }
            set { SetValue(ClickCommandProperty, value); }
        }

        public static readonly DependencyProperty ClickCommandParameterProperty = DependencyProperty.Register("ClickCommandParameter", typeof(object), typeof(PanelButtonBehavior), new PropertyMetadata(null));
        public object ClickCommandParameter
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get { return GetValue(ClickCommandParameterProperty); }
            set { SetValue(ClickCommandParameterProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Tapped += AssociatedObject_Tapped;
        }


        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Tapped -= AssociatedObject_Tapped;
        }

        private void AssociatedObject_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (ClickCommand == null || !ClickCommand.CanExecute(ClickCommandParameter)) { return; }

            ClickCommand.Execute(ClickCommandParameter);
        }
    }
}
