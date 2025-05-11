using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Diagnostics;
using System.Security.Principal;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using TaskDialog = Microsoft.WindowsAPICodePack.Dialogs.TaskDialog;

namespace Windows_Sound_Changer;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    List<Item> items;
    Item? item;

    public MainWindow()
    {
        InitializeComponent();

        // This checks if the program is running as administrator
        if (IsRunningAsAdmin())
        {
            Title += " (Administrator)";
            var dialog = new TaskDialog
            {
                Caption = "You are running as administrator!",
                InstructionText = "This program does not requires administrator privileges to change sounds.",
                Icon = TaskDialogStandardIcon.Shield,
                StandardButtons = TaskDialogStandardButtons.Ok,
            };
            PlaySound(@"C:\Windows\Media\Windows Exclamation.wav");
            TaskDialogResult result = dialog.Show();

            if (result == TaskDialogResult.No) Application.Current.Shutdown();
        }

        // Creates every item in the Windows tree. This does not include Microsoft Visual Studio tree, File explorer tree and Windows Speech Recognition tree.
        items = new()
        {
            new() { Name = "Asterisk", KeyName = "SystemAsterisk" },
            new() { Name = "Calender Reminder", KeyName = "Notification.Reminder"},
            new() { Name = "Close Program", KeyName = "Close" },
            new() { Name = "Critical Battery Alarm", KeyName = "CriticalBatteryAlarm" },
            new() { Name = "Critical Stop", KeyName = "SystemHand" },
            new() { Name = "Default Beep", KeyName = ".Default" },
            new() { Name = "Desktop Mail Notification", KeyName = "MailBeep" },
            new() { Name = "Device Connect", KeyName = "DeviceConnect" },
            new() { Name = "Device Disconnect", KeyName = "DeviceDisconnect" },
            new() { Name = "Device Failed to Connect", KeyName = "DeviceFail" },
            new() { Name = "Exclamation", KeyName = "SystemExclamation" },
            new() { Name = "Exit Windows", KeyName = "SystemExit"},
            new() { Name = "Instant Message Notification", KeyName = "Notification.IM" },
            new() { Name = "Low Battery Alarm", KeyName = "LowBatteryAlarm" },
            new() { Name = "Maximize", KeyName = "Maximize" },
            new() { Name = "Menu Command", KeyName = "MenuCommand" },
            new() { Name = "Menu Pop-up", KeyName = "MenuPopup" },
            new() { Name = "Message Nudge", KeyName = "MessageNudge" },
            new() { Name = "Minimize", KeyName = "Minimize" },
            new() { Name = "New Fax Notification", KeyName = "FaxBeep" },
            new() { Name = "New Mail Notification", KeyName = "MailBeep" },
            new() { Name = "New Text Message Notification", KeyName = "Notification.SMS" },
            new() { Name = "NFP Completion", KeyName = "Notification.Proximity" },
            new() { Name = "NFP Connection", KeyName = "ProximityConnection" },
            new() { Name = "Notification", KeyName = "Notification.Default"},
            new() { Name = "Open Program", KeyName = "Open" },
            new() { Name = "Select", KeyName = "CCSelect"},
            new() { Name = "Print Complete", KeyName = "PrintComplete" },
            new() { Name = "Program Error", KeyName = "AppGPFault" },
            new() { Name = "Question", KeyName = "SystemQuestion" },
            new() { Name = "Restore Down", KeyName = "RestoreDown" },
            new() { Name = "Restore Up", KeyName = "RestoreUp" },
            new() { Name = "System Notification", KeyName = "SystemNotification" },
            new() { Name = "Show Toolbar Band", KeyName = "ShowBand" },
            new() { Name = "Windows Change Theme", KeyName = "ChangeTheme" },
            new() { Name = "Windows Logoff", KeyName = "WindowsLogoff"},
            new() { Name = "Windows Logon", KeyName = "WindowsLogon"},
            new() { Name = "Windows User Account Control", KeyName = "WindowsUAC" }
        };

        // Assigns ID to act as an index for ListBox
        for (int i = 0; i < items.Count; i++) items[i].Id = i;

        // Sets the ListBox template to items just created
        ListBox.ItemsSource = items;

        // Lots of events
        ApplyButton.Click += ApplyButton_Click;
        PlayButton.Click += PlaySound_Click;
        OpenSoundSettingsButton.Click += OpenSoundSettings_Click;
        RevertButton.Click += RevertButton_Click;

        ApplyButton.MouseEnter += ApplyButton_MouseEnter;
        PlayButton.MouseEnter += PlayButton_MouseEnter;
        OpenSoundSettingsButton.MouseEnter += OpenSoundSettingsButton_MouseEnter;
        RevertButton.MouseEnter += RevertButton_MouseEnter;
        Dropdown.MouseEnter += Dropdown_MouseEnter;

        ApplyButton.MouseLeave += (sender, e) => { UpdateHoveringText("Hover over any button to see its description."); };
        PlayButton.MouseLeave += (sender, e) => { UpdateHoveringText("Hover over any button to see its description."); };
        OpenSoundSettingsButton.MouseLeave += (sender, e) => { UpdateHoveringText("Hover over any button to see its description."); };
        RevertButton.MouseLeave += (sender, e) => { UpdateHoveringText("Hover over any button to see its description."); };
        Dropdown.MouseLeave += (sender, e) => { UpdateHoveringText("Hover over any button to see its description."); };

        Dropdown.SelectionChanged += Dropdown_SelectionChanged;

        ListBox.SelectionChanged += ListBox_SelectionChanged;
        ListBox.SelectedIndex = 0;
        ApplyButton.IsEnabled = false;
    }

    // Lots of methods
    private void OpenSoundSettingsButton_MouseEnter(object sender, MouseEventArgs e) => UpdateHoveringText("Opens the Control Panel's sound settings and opens the Sound tab.");
    private void PlayButton_MouseEnter(object sender, MouseEventArgs e) => UpdateHoveringText("Plays the selected sound.");
    private void ApplyButton_MouseEnter(object sender, MouseEventArgs e) => UpdateHoveringText("Applies changes to your sound scheme.");
    private void RevertButton_MouseEnter(object sender, MouseEventArgs e) => UpdateHoveringText("Reverts all changes made. This will only revert changes that are not applied yet.");
    private void Dropdown_MouseEnter(object sender, MouseEventArgs e) => UpdateHoveringText("Choose sound scheme presets.");

    private void UpdateHoveringText(string text) => HoverText.Text = text;
    private void UpdateHoveringTextForEnteringSoundButtons(object sender, RoutedEventArgs e) => UpdateHoveringText("Choose a sound file from the computer.");
    private void UpdateHoveringTextForLeavingSoundButtons(object sender, RoutedEventArgs e) => UpdateHoveringText("Hover over any button to see its description.");

    string initialSoundFile = "", chosenSoundFile = "";
    readonly string registryPrefix = @"AppEvents\Schemes\Apps\.Default\";
    Dictionary<string, string> changesToApply = [];

    private void ApplyButton_Click(object sender, RoutedEventArgs e)
    {
        for (int i = 0; i < changesToApply.Count; i++)
        {
            foreach (var key in changesToApply.Keys)
            {
                using RegistryKey? registryKey = Registry.CurrentUser.OpenSubKey(@$"{registryPrefix}\{key}\.Current", true);
                registryKey?.SetValue("", changesToApply[key], RegistryValueKind.String);
            }
            items[i].IsModified = false;
        }
        ApplyButton.IsEnabled = false;
    }
    private void RevertButton_Click(object sender, RoutedEventArgs e)
    {
        foreach (var item in items)
        {
            item.ButtonText = "Choose Sound";
            item.IsModified = false;
            chosenSoundFile = initialSoundFile;
        }
        changesToApply.Clear();
        PlayButton.IsEnabled = false;
        ApplyButton.IsEnabled = false;
    }
    private void OpenSoundSettings_Click(object sender, RoutedEventArgs e) => Process.Start("control", "mmsys.cpl,,2");

    private void OpenFileDialog_Click(object sender, RoutedEventArgs e)
    {
        string modifiedSoundFile;
        var index = sender as Button;
        ListBox.SelectedIndex = int.Parse(index.Uid);

        OpenFileDialog fileDialog = new();
        fileDialog.Filter = "Microsoft Wave File (.wav) | *.wav";

        Item selectedItem = (Item)ListBox.SelectedItem;
        string filePrefix = @"AppEvents\Schemes\Apps\.Default\";

        bool? success = fileDialog.ShowDialog();

        if (success == true && sender is Button button && button.Tag is Item item)
        {
            using RegistryKey? registryKey = Registry.CurrentUser.OpenSubKey(@$"{filePrefix}\{selectedItem.KeyName}\.Current", true);

            item.ButtonText = fileDialog.SafeFileName;
            item.IsModified = true;

            modifiedSoundFile = fileDialog.FileName;
            chosenSoundFile = modifiedSoundFile;

            bool changeAdded = changesToApply.TryAdd(selectedItem.KeyName, modifiedSoundFile);
            if (!changeAdded) changesToApply[selectedItem.KeyName] = modifiedSoundFile;

            PlayButton.IsEnabled = true;
            ApplyButton.IsEnabled = true;
        }
    }
    private void PlaySound_Click(object sender, RoutedEventArgs e)
    {
        item = (Item)ListBox.SelectedItem;

        using RegistryKey? registryKey = Registry.CurrentUser.OpenSubKey(@$"{registryPrefix}\{item.KeyName}\.Current");

        PlaySound(chosenSoundFile);
    }
    private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        item = (Item)ListBox.SelectedItem;
        using RegistryKey? registryKey = Registry.CurrentUser.OpenSubKey(@$"{registryPrefix}\{item.KeyName}\.Current");
        if (registryKey.GetValue("") == null) initialSoundFile = "";
        else initialSoundFile = registryKey.GetValue("").ToString();

        if (initialSoundFile == "" && !item.IsModified)
        {
            chosenSoundFile = "";
            PlayButton.IsEnabled = false;
            return;
        }

        if (item.IsModified) chosenSoundFile = changesToApply[item.KeyName];
        else chosenSoundFile = initialSoundFile;

        if (string.IsNullOrEmpty(chosenSoundFile)) PlayButton.IsEnabled = false;
        else PlayButton.IsEnabled = true;
    }
    private void Dropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        RevertButton_Click("just revert all my changes gng", e);
        var dropDownItem = Dropdown.SelectedItem as ComboBoxItem;
        string value = dropDownItem.Content.ToString();

        switch (value)
        {
            case "-- Choose Preset --":
                RevertButton_Click("just revert my changes gng", e);
                break;
            case "Windows 11":
                foreach (var item in Preset.Windows11)
                    for (int i = 0; i < items.Count; i++)
                        if (item.Name == items[i].KeyName)
                        {
                            bool changeAdded = changesToApply.TryAdd(item.Name, item.FileName);
                            if (!changeAdded) changesToApply[item.Name] = item.FileName;
                            if (!string.IsNullOrEmpty(item.FileName)) items[i].ButtonText = item.FileName.Split(@"\").Last();

                            items[i].IsModified = true;
                        }
                chosenSoundFile = changesToApply[item.KeyName];
                break;
            case "Windows 10":
                foreach (var item in Preset.Windows10)
                    for (int i = 0; i < items.Count; i++)
                        if (item.Name == items[i].KeyName)
                        {
                            bool changeAdded = changesToApply.TryAdd(item.Name, item.FileName);
                            if (!changeAdded) changesToApply[item.Name] = item.FileName;
                            if (!string.IsNullOrEmpty(item.FileName)) items[i].ButtonText = item.FileName.Split(@"\").Last();

                            items[i].IsModified = true;
                        }
                chosenSoundFile = changesToApply[item.KeyName];
                break;
            case "Windows 7":
                foreach (var item in Preset.Windows7)
                    for (int i = 0; i < items.Count; i++)
                        if (item.Name == items[i].KeyName)
                        {
                            bool changeAdded = changesToApply.TryAdd(item.Name, item.FileName);
                            if (!changeAdded) changesToApply[item.Name] = item.FileName;
                            if (!string.IsNullOrEmpty(item.FileName)) items[i].ButtonText = item.FileName.Split(@"\").Last();

                            items[i].IsModified = true;
                        }
                chosenSoundFile = changesToApply[item.KeyName];
                break;
            case "Windows 7 Afternoon":
                foreach (var item in Preset.Windows7_Afternoon)
                    for (int i = 0; i < items.Count; i++)
                        if (item.Name == items[i].KeyName)
                        {
                            bool changeAdded = changesToApply.TryAdd(item.Name, item.FileName);
                            if (!changeAdded) changesToApply[item.Name] = item.FileName;
                            if (!string.IsNullOrEmpty(item.FileName)) items[i].ButtonText = item.FileName.Split(@"\").Last();

                            items[i].IsModified = true;
                        }
                chosenSoundFile = changesToApply[item.KeyName];
                break;
            case "Windows 7 Calligraphy":
                foreach (var item in Preset.Windows7_Calligraphy)
                    for (int i = 0; i < items.Count; i++)
                        if (item.Name == items[i].KeyName)
                        {
                            bool changeAdded = changesToApply.TryAdd(item.Name, item.FileName);
                            if (!changeAdded) changesToApply[item.Name] = item.FileName;
                            if (!string.IsNullOrEmpty(item.FileName)) items[i].ButtonText = item.FileName.Split(@"\").Last();

                            items[i].IsModified = true;
                        }
                chosenSoundFile = changesToApply[item.KeyName];
                break;
            case "Windows 7 Characters":
                foreach (var item in Preset.Windows7_Characters)
                    for (int i = 0; i < items.Count; i++)
                        if (item.Name == items[i].KeyName)
                        {
                            bool changeAdded = changesToApply.TryAdd(item.Name, item.FileName);
                            if (!changeAdded) changesToApply[item.Name] = item.FileName;
                            if (!string.IsNullOrEmpty(item.FileName)) items[i].ButtonText = item.FileName.Split(@"\").Last();

                            items[i].IsModified = true;
                        }
                chosenSoundFile = changesToApply[item.KeyName];
                break;
            case "Windows 7 Cityscape":
                foreach (var item in Preset.Windows7_Cityscape)
                    for (int i = 0; i < items.Count; i++)
                        if (item.Name == items[i].KeyName)
                        {
                            bool changeAdded = changesToApply.TryAdd(item.Name, item.FileName);
                            if (!changeAdded) changesToApply[item.Name] = item.FileName;
                            if (!string.IsNullOrEmpty(item.FileName)) items[i].ButtonText = item.FileName.Split(@"\").Last();

                            items[i].IsModified = true;
                        }
                chosenSoundFile = changesToApply[item.KeyName];
                break;
            case "Windows 7 Delta":
                foreach (var item in Preset.Windows7_Delta)
                    for (int i = 0; i < items.Count; i++)
                        if (item.Name == items[i].KeyName)
                        {
                            bool changeAdded = changesToApply.TryAdd(item.Name, item.FileName);
                            if (!changeAdded) changesToApply[item.Name] = item.FileName;
                            if (!string.IsNullOrEmpty(item.FileName)) items[i].ButtonText = item.FileName.Split(@"\").Last();

                            items[i].IsModified = true;
                        }
                chosenSoundFile = changesToApply[item.KeyName];
                break;
            case "Windows 7 Festival":
                foreach (var item in Preset.Windows7_Festival)
                    for (int i = 0; i < items.Count; i++)
                        if (item.Name == items[i].KeyName)
                        {
                            bool changeAdded = changesToApply.TryAdd(item.Name, item.FileName);
                            if (!changeAdded) changesToApply[item.Name] = item.FileName;
                            if (!string.IsNullOrEmpty(item.FileName)) items[i].ButtonText = item.FileName.Split(@"\").Last();

                            items[i].IsModified = true;
                        }
                chosenSoundFile = changesToApply[item.KeyName];
                break;
            case "Windows 7 Garden":
                foreach (var item in Preset.Windows7_Garden)
                    for (int i = 0; i < items.Count; i++)
                        if (item.Name == items[i].KeyName)
                        {
                            bool changeAdded = changesToApply.TryAdd(item.Name, item.FileName);
                            if (!changeAdded) changesToApply[item.Name] = item.FileName;
                            if (!string.IsNullOrEmpty(item.FileName)) items[i].ButtonText = item.FileName.Split(@"\").Last();

                            items[i].IsModified = true;
                        }
                chosenSoundFile = changesToApply[item.KeyName];
                break;
            case "Windows 7 Heritage":
                foreach (var item in Preset.Windows7_Heritage)
                    for (int i = 0; i < items.Count; i++)
                        if (item.Name == items[i].KeyName)
                        {
                            bool changeAdded = changesToApply.TryAdd(item.Name, item.FileName);
                            if (!changeAdded) changesToApply[item.Name] = item.FileName;
                            if (!string.IsNullOrEmpty(item.FileName)) items[i].ButtonText = item.FileName.Split(@"\").Last();

                            items[i].IsModified = true;
                        }
                chosenSoundFile = changesToApply[item.KeyName];
                break;
            case "Windows 7 Landscape":
                foreach (var item in Preset.Windows7_Landscape)
                    for (int i = 0; i < items.Count; i++)
                        if (item.Name == items[i].KeyName)
                        {
                            bool changeAdded = changesToApply.TryAdd(item.Name, item.FileName);
                            if (!changeAdded) changesToApply[item.Name] = item.FileName;
                            if (!string.IsNullOrEmpty(item.FileName)) items[i].ButtonText = item.FileName.Split(@"\").Last();

                            items[i].IsModified = true;
                        }
                chosenSoundFile = changesToApply[item.KeyName];
                break;
            case "Windows 7 Quirky":
                foreach (var item in Preset.Windows7_Quirky)
                    for (int i = 0; i < items.Count; i++)
                        if (item.Name == items[i].KeyName)
                        {
                            bool changeAdded = changesToApply.TryAdd(item.Name, item.FileName);
                            if (!changeAdded) changesToApply[item.Name] = item.FileName;
                            if (!string.IsNullOrEmpty(item.FileName)) items[i].ButtonText = item.FileName.Split(@"\").Last();

                            items[i].IsModified = true;
                        }
                chosenSoundFile = changesToApply[item.KeyName];
                break;
            case "Windows 7 Raga":
                foreach (var item in Preset.Windows7_Raga)
                    for (int i = 0; i < items.Count; i++)
                        if (item.Name == items[i].KeyName)
                        {
                            bool changeAdded = changesToApply.TryAdd(item.Name, item.FileName);
                            if (!changeAdded) changesToApply[item.Name] = item.FileName;
                            if (!string.IsNullOrEmpty(item.FileName)) items[i].ButtonText = item.FileName.Split(@"\").Last();

                            items[i].IsModified = true;
                        }
                chosenSoundFile = changesToApply[item.KeyName];
                break;
            case "Windows 7 Savanna":
                foreach (var item in Preset.Windows7_Savanna)
                    for (int i = 0; i < items.Count; i++)
                        if (item.Name == items[i].KeyName)
                        {
                            bool changeAdded = changesToApply.TryAdd(item.Name, item.FileName);
                            if (!changeAdded) changesToApply[item.Name] = item.FileName;
                            if (!string.IsNullOrEmpty(item.FileName)) items[i].ButtonText = item.FileName.Split(@"\").Last();

                            items[i].IsModified = true;
                        }
                chosenSoundFile = changesToApply[item.KeyName];
                break;
            case "Windows 7 Sonata":
                foreach (var item in Preset.Windows7_Sonata)
                    for (int i = 0; i < items.Count; i++)
                        if (item.Name == items[i].KeyName)
                        {
                            bool changeAdded = changesToApply.TryAdd(item.Name, item.FileName);
                            if (!changeAdded) changesToApply[item.Name] = item.FileName;
                            if (!string.IsNullOrEmpty(item.FileName)) items[i].ButtonText = item.FileName.Split(@"\").Last();

                            items[i].IsModified = true;
                        }
                chosenSoundFile = changesToApply[item.KeyName];
                break;
        }

        item = (Item)ListBox.SelectedItem;
        if (item.ButtonText != "Choose Sound") PlayButton.IsEnabled = true;
        if (dropDownItem.Content != "-- Choose Preset --") ApplyButton.IsEnabled = true;
    }
    MediaPlayer player = new();
    private void PlaySound(string soundFile)
    {
        if (string.IsNullOrEmpty(soundFile))
        {
            HoverText.Text = "There is no sound to play.";
            return;
        }
        player.Open(new Uri(soundFile));
        player.Play();
    }

    private static bool IsRunningAsAdmin()
    {
        using WindowsIdentity identity = WindowsIdentity.GetCurrent();
        WindowsPrincipal principal = new(identity);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }
}