using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UiPath.Robot.Api;

namespace UiPathTeam.EasyProcessing.Activities.Design
{
    public partial class ProcessNameDialogBox : Window
    {
        public string SelectedProcessName { get; set; }

        public ProcessNameDialogBox()
        {
            InitializeComponent();
            okButton.IsEnabled = false;
            processNameListBox.IsEnabled = false;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var client = new RobotClient();
                var processes = await client.GetProcesses();
                foreach (var process in processes.OrderBy(p => p.Name))
                {
                    processNameListBox.Items.Add(process.Name);
                }
                if (processNameListBox.Items.Count > 0)
                {
                    processNameListBox.IsEnabled = true;
                    if (SelectedProcessName != null)
                    {
                        var selected = SelectedProcessName;
                        SelectedProcessName = null;
                        foreach (string name in processNameListBox.Items)
                        {
                            if (name == selected)
                            {
                                processNameListBox.SelectedValue = SelectedProcessName = name;
                                okButton.IsEnabled = true;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show(this, "Unable to get process names.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OkClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedProcessName = (string)processNameListBox.SelectedValue;
            okButton.IsEnabled = !string.IsNullOrEmpty(SelectedProcessName);
        }
    }
}
