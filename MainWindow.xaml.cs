using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Diagnostics;
using System.Media;
using System.Security.Principal;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using TaskDialog = Microsoft.WindowsAPICodePack.Dialogs.TaskDialog;

namespace Windows_Sound_Changer;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    List<Item> items;
    List<Preset> Windows11, Windows10;
    Item item;

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
            new() { Name = "Instant Message Notification", KeyName = "Notification.IM" },
            new() { Name = "Low Battery Alarm", KeyName = "LowBatteryAlarm" },
            new() { Name = "Maximize", KeyName = "Maximize" },
            new() { Name = "Menu Command", KeyName = "MenuCommand" },
            new() { Name = "Menu Pop-up", KeyName = "MenuPopup" },
            new() { Name = "Message Nudge", KeyName = "MessageNudge" },
            new() { Name = "Minimize", KeyName = "Minimize" },
            new() { Name = "New Fax Notification", KeyName = "FaxBeep" },
            new() { Name = "New Mail Notification", KeyName = "Notification.Mail" },
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
            new() { Name = "Windows User Account Control", KeyName = "WindowsUAC" }
        };
        Windows11 = new()
        {
            new() { Name = "SystemAsterisk", FileName = @$"{Environment.CurrentDirectory}\sounds\Windows 11\Windows Background.wav" },
            new() { Name = "Notification.Reminder", FileName = @$"{Environment.CurrentDirectory}\sounds\Windows 11\Windows Notify Calendar.wav" },
            new() { Name = "Close", FileName = "" },
            new() { Name = "CriticalBatteryAlarm", FileName = @$"{Environment.CurrentDirectory}\sounds\Windows 11\Windows Battery Critical.wav" },
            new() { Name = "SystemHand", FileName = @$"{Environment.CurrentDirectory}\sounds\Windows 11\Windows Foreground.wav" },
            new() { Name = ".Default", FileName = @$"{Environment.CurrentDirectory}\sounds\Windows 11\Windows Background.wav" },
            new() { Name = "MailBeep", FileName = @$"{Environment.CurrentDirectory}\sounds\Windows 11\Windows Notify Email.wav" },
            new() { Name = "DeviceConnect", FileName = @$"{Environment.CurrentDirectory}\sounds\Windows 11\Windows Hardware Insert.wav" },
            new() { Name = "DeviceDisconnect", FileName = @$"{Environment.CurrentDirectory}\sounds\Windows 11\Windows Hardware Remove.wav" },
            new() { Name = "DeviceFail", FileName = @$"{Environment.CurrentDirectory}\sounds\Windows 11\Windows Hardware Fail.wav" },
            new() { Name = "SystemExclamation", FileName = @$"{Environment.CurrentDirectory}\sounds\Windows 11\Windows Exclamation.wav" },
            new() { Name = "Notification.IM", FileName = @$"{Environment.CurrentDirectory}\sounds\Windows 11\Windows Notify Messaging.wav" },
            new() { Name = "LowBatteryAlarm", FileName = @$"{Environment.CurrentDirectory}\sounds\Windows 11\Windows Battery Low.wav" },
            new() { Name = "Maximize", FileName = "" },
            new() { Name = "MenuCommand", FileName = "" },
            new() { Name = "MenuPopup", FileName = "" },
            new() { Name = "MessageNudge", FileName = @$"{Environment.CurrentDirectory}\sounds\Windows 11\Windows Message Nudge.wav" },
            new() { Name = "Minimize", FileName = "" },
            new() { Name = "FaxBeep", FileName = @$"{Environment.CurrentDirectory}\sounds\Windows 11\Windows Notify Email.wav" },
            new() { Name = "Notification.Mail", FileName = @$"{Environment.CurrentDirectory}\sounds\Windows 11\Windows Notify Email.wav" },
            new() { Name = "Notification.SMS" , FileName = @$"{Environment.CurrentDirectory}\sounds\Windows 11\Windows Notify Messaging.wav" },
            new() { Name = "Notification.Proximity", FileName = @$"{Environment.CurrentDirectory}\sounds\Windows 11\Windows Proximity Notification.wav" },
            new() { Name = "ProximityConnection", FileName = @$"{Environment.CurrentDirectory}\sounds\Windows 11\Windows Proximity Connection.wav" },
            new() { Name = "Notification.Default", FileName = @$"{Environment.CurrentDirectory}\sounds\Windows 11\Windows Notify System Generic.wav"},
            new() { Name = "Open", FileName = "" },
            new() { Name = "CCSelect", FileName = ""},
            new() { Name = "PrintComplete", FileName = @$"{Environment.CurrentDirectory}\sounds\Windows 11\Windows Print complete.wav" },
            new() { Name = "AppGPFault" , FileName = ""},
            new() { Name = "SystemQuestion", FileName = "" },
            new() { Name = "RestoreDown", FileName = "" },
            new() { Name = "RestoreUp" , FileName = "" },
            new() { Name = "SystemNotification", FileName = @$"{Environment.CurrentDirectory}\sounds\Windows 11\Windows Background.wav" },
            new() { Name = "ShowBand" , FileName = "" },
            new() { Name = "ChangeTheme" , FileName = "" },
            new() { Name = "WindowsUAC" , FileName = @$"{Environment.CurrentDirectory}\sounds\Windows 11\Windows User Account Control.wav" }
        };
        Windows10 = new()
        {
            new() { Name = "SystemAsterisk", FileName = @$"{Environment.CurrentDirectory}\sounds\Windows 10\Windows Background.wav" },
            new() { Name = "Notification.Reminder", FileName = @$"{Environment.CurrentDirectory}\sounds\Windows 10\Windows Notify Calendar.wav" },
            new() { Name = "Close", FileName = "" },
            new() { Name = "CriticalBatteryAlarm", FileName = @$"{Environment.CurrentDirectory}\sounds\Windows 10\Windows Battery Critical.wav" },
            new() { Name = "SystemHand", FileName = @$"{Environment.CurrentDirectory}\sounds\Windows 10\Windows Foreground.wav" },
            new() { Name = ".Default", FileName = @$"{Environment.CurrentDirectory}\sounds\Windows 10\Windows Background.wav" },
            new() { Name = "MailBeep", FileName = @$"{Environment.CurrentDirectory}\sounds\Windows 10\Windows Notify Email.wav" },
            new() { Name = "DeviceConnect", FileName = @$"{Environment.CurrentDirectory}\sounds\Windows 10\Windows Hardware Insert.wav" },
            new() { Name = "DeviceDisconnect", FileName = @$"{Environment.CurrentDirectory}\sounds\Windows 10\Windows Hardware Remove.wav" },
            new() { Name = "DeviceFail", FileName = @$"{Environment.CurrentDirectory}\sounds\Windows 10\Windows Hardware Fail.wav" },
            new() { Name = "SystemExclamation", FileName = @$"{Environment.CurrentDirectory}\sounds\Windows 10\Windows Exclamation.wav" },
            new() { Name = "Notification.IM", FileName = @$"{Environment.CurrentDirectory}\sounds\Windows 10\Windows Notify Messaging.wav" },
            new() { Name = "LowBatteryAlarm", FileName = @$"{Environment.CurrentDirectory}\sounds\Windows 10\Windows Battery Low.wav" },
            new() { Name = "Maximize", FileName = "" },
            new() { Name = "MenuCommand", FileName = "" },
            new() { Name = "MenuPopup", FileName = "" },
            new() { Name = "MessageNudge", FileName = @$"{Environment.CurrentDirectory}\sounds\Windows 10\Windows Message Nudge.wav" },
            new() { Name = "Minimize", FileName = "" },
            new() { Name = "FaxBeep", FileName = @$"{Environment.CurrentDirectory}\sounds\Windows 10\Windows Notify Email.wav" },
            new() { Name = "Notification.Mail", FileName = @$"{Environment.CurrentDirectory}\sounds\Windows 10\Windows Notify Email.wav" },
            new() { Name = "Notification.SMS" , FileName = @$"{Environment.CurrentDirectory}\sounds\Windows 10\Windows Notify Messaging.wav" },
            new() { Name = "Notification.Proximity", FileName = @$"{Environment.CurrentDirectory}\sounds\Windows 10\Windows Proximity Notification.wav" },
            new() { Name = "ProximityConnection", FileName = @$"{Environment.CurrentDirectory}\sounds\Windows 10\Windows Proximity Connection.wav" },
            new() { Name = "Notification.Default", FileName = @$"{Environment.CurrentDirectory}\sounds\Windows 10\Windows Notify System Generic.wav"},
            new() { Name = "Open", FileName = "" },
            new() { Name = "CCSelect", FileName = ""},
            new() { Name = "PrintComplete", FileName = @$"{Environment.CurrentDirectory}\sounds\Windows 10\Windows Print complete.wav" },
            new() { Name = "AppGPFault" , FileName = ""},
            new() { Name = "SystemQuestion", FileName = "" },
            new() { Name = "RestoreDown", FileName = "" },
            new() { Name = "RestoreUp" , FileName = "" },
            new() { Name = "SystemNotification", FileName = @$"{Environment.CurrentDirectory}\sounds\Windows 10\Windows Background.wav" },
            new() { Name = "ShowBand" , FileName = "" },
            new() { Name = "ChangeTheme" , FileName = "" },
            new() { Name = "WindowsUAC" , FileName = @$"{Environment.CurrentDirectory}\sounds\Windows 10\Windows User Account Control.wav"}
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
        if (string.IsNullOrEmpty(chosenSoundFile)) PlayButton.IsEnabled = false;
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
        initialSoundFile = registryKey.GetValue("").ToString();

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
        var dropDownItem = Dropdown.SelectedItem as ComboBoxItem;
        string value = dropDownItem.Content.ToString();

        switch (value)
        {
            case "Windows 11":
                foreach (var item in Windows11)
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
                foreach (var item in Windows10)
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
        ApplyButton.IsEnabled = true;
    }
    private void PlaySound(string soundFile)
    {
        SoundPlayer player = new();
        if (string.IsNullOrEmpty(soundFile))
        {
            HoverText.Text = "There is no sound to play.";
            return;
        }
        player.SoundLocation = soundFile;
        player.Play();
    }

    private static bool IsRunningAsAdmin()
    {
        using WindowsIdentity identity = WindowsIdentity.GetCurrent();
        WindowsPrincipal principal = new(identity);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }
}
public struct Preset
{
    public string Name { get; set; }
    public string FileName { get; set; }
}