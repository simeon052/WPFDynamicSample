using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFDynamicSample
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private static WPFDynamicSample.Models.SampleClass sample = new Models.SampleClass();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = new Button() { Content = "Added Button" };
            button.Command = new CtrlCommand<Button>(button, (b, p) =>
            {
                System.Diagnostics.Debug.WriteLine($"pressed {p?.ToString() ?? string.Empty} {b?.Content.ToString() ?? string.Empty}");
                b.Content = "Pressed button";
            }
            );
            Stack4Control.Children.Add(button);

        }

        private void AddClassButton_Click(object sender, RoutedEventArgs e)
        {
            var t = sample.GetType();

            var members = t.GetMembers(
                BindingFlags.Public |
                BindingFlags.Instance |
                BindingFlags.DeclaredOnly
                );
            foreach (var m in members)
            {
                System.Diagnostics.Debug.WriteLine($"[{m.Name}] - [{m.MemberType}]");
                switch (m.MemberType)
                {
                    case MemberTypes.Method:
                        var mi1 = t.GetMethod(m.Name);

                        System.Diagnostics.Debug.WriteLine($"  {mi1?.ToString() ?? "GetMethod is failed"}");
                        System.Diagnostics.Debug.WriteLine($"    {mi1?.ReturnType.ToString() ?? string.Empty}");
                        foreach (var pi in mi1?.GetParameters() ?? null)
                        {
                            //                            System.Diagnostics.Debug.WriteLine($"      {pi.ToString()}");
                            System.Diagnostics.Debug.WriteLine($"     Name: {pi.Name}");
                            System.Diagnostics.Debug.WriteLine($"     Type: {pi.ParameterType.ToString()}");
                            System.Diagnostics.Debug.WriteLine($"     Member: {pi.Member}");
                            System.Diagnostics.Debug.WriteLine($"       [{pi.ToString()}]");


                            if (pi.ParameterType.ToString().Equals("System.String")){
                                var property = t.GetProperty(GuessPropertyName(mi1.Name));
                                if (property != null)
                                {
                                    var edit = new TextBox() { Text = (property?.GetValue(sample) ?? string.Empty) as string };
                                    edit.TextChanged += (s, ev) =>
                                    {
                                        property.SetValue(sample, edit.Text);
                                        System.Diagnostics.Debug.WriteLine($"==> {sample.StringProperty}");
                                    };
                                    Stack4Control.Children.Add(new TextBlock() { Text = property.Name });
                                    Stack4Control.Children.Add(edit);
                                }
                                break;
                            }

                            if (pi.ParameterType.IsEnum)
                            {
                                var property = t.GetProperty(GuessPropertyName(mi1.Name));
                                if (property != null)
                                {
                                    var comboBox = new ComboBox();

                                    foreach (var a in Enum.GetValues(pi.ParameterType))
                                    {
                                        comboBox.Items.Add(a);
                                    }

                                    comboBox.SelectionChanged += (s_e, ea_e) =>
                                    {
                                        property.SetValue(sample, comboBox.SelectedItem);
                                        System.Diagnostics.Debug.WriteLine($"==> {sample.EnumProperty}");
                                    };

                                    Stack4Control.Children.Add(new TextBlock() { Text = property.Name });
                                    Stack4Control.Children.Add(comboBox);
                                }
                                break;
                            }

                            if (pi.ParameterType.ToString().Equals("System.Boolean"))
                            {
                                var property = t.GetProperty(GuessPropertyName(mi1.Name));
                                if (property != null)
                                {
                                    var checkBox = new CheckBox() { IsChecked = (bool)(property?.GetValue(sample) ?? false) };
                                    checkBox.Click  += (s, ev) =>
                                    {
                                        property.SetValue(sample, checkBox.IsChecked);
                                        System.Diagnostics.Debug.WriteLine($"==> {sample.BoolProperty}");
                                    };
                                    Stack4Control.Children.Add(new TextBlock() { Text = property.Name });
                                    Stack4Control.Children.Add(checkBox);
                                }
                                break;
                            }

                        }


                        break;

                    default:
                        System.Diagnostics.Debug.WriteLine($"  --");
                        break;
                }
        }


        string GuessPropertyName(string methodName)
        {
            string propertyName = methodName;

            if (methodName.Contains("Set"))
            {
                propertyName = methodName.Remove(0, 3);
            }

            return propertyName;
        }
    }


        private class CtrlCommand<T> : ICommand
        {
            private CtrlCommand() { }

            public CtrlCommand(T ctrlObj, Action<T, object> action)
            {
                this.actionDelegate = action;
                this.ctrlObj = ctrlObj;
            }
            public event EventHandler CanExecuteChanged;

            protected Action<T, object> actionDelegate { get; set; }
            
            protected Object ctrlObj { get; set; }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                actionDelegate?.Invoke((T)this.ctrlObj , parameter);
            }
        }

    }
}
