[Setup]
AppName=Windows Sound Changer
AppVersion=1.0.1
AppPublisher=bronzemelon
AppPublisherURL=https://github.com/bronzemelon

WizardStyle=modern
DefaultDirName={userpf}\Windows Sound Changer
DefaultGroupName=Windows Sound Changer
UninstallDisplayIcon={app}\Windows Sound Changer.exe
LicenseFile=".\bin\Release\net9.0-windows\license.txt"

Compression=lzma2
SolidCompression=yes
PrivilegesRequired=lowest

[Files]
Source: ".\bin\Release\net9.0-windows\sounds\Windows 11\*.wav"; DestDir: "{app}\sounds\Windows 11"
Source: ".\bin\Release\net9.0-windows\sounds\Windows 10\*.wav"; DestDir: "{app}\sounds\Windows 10"
Source: ".\bin\Release\net9.0-windows\sounds\Windows 7\*.wav"; DestDir: "{app}\sounds\Windows 7"
Source: ".\bin\Release\net9.0-windows\sounds\Windows 7\Afternoon\*.wav"; DestDir: "{app}\sounds\Windows 7\Afternoon"
Source: ".\bin\Release\net9.0-windows\sounds\Windows 7\Calligraphy\*.wav"; DestDir: "{app}\sounds\Windows 7\Calligraphy"
Source: ".\bin\Release\net9.0-windows\sounds\Windows 7\Characters\*.wav"; DestDir: "{app}\sounds\Windows 7\Characters"
Source: ".\bin\Release\net9.0-windows\sounds\Windows 7\Cityscape\*.wav"; DestDir: "{app}\sounds\Windows 7\Cityscape"
Source: ".\bin\Release\net9.0-windows\sounds\Windows 7\Delta\*.wav"; DestDir: "{app}\sounds\Windows 7\Delta"
Source: ".\bin\Release\net9.0-windows\sounds\Windows 7\Festival\*.wav"; DestDir: "{app}\sounds\Windows 7\Festival"
Source: ".\bin\Release\net9.0-windows\sounds\Windows 7\Garden\*.wav"; DestDir: "{app}\sounds\Windows 7\Garden"
Source: ".\bin\Release\net9.0-windows\sounds\Windows 7\Heritage\*.wav"; DestDir: "{app}\sounds\Windows 7\Heritage"
Source: ".\bin\Release\net9.0-windows\sounds\Windows 7\Landscape\*.wav"; DestDir: "{app}\sounds\Windows 7\Landscape"
Source: ".\bin\Release\net9.0-windows\sounds\Windows 7\Quirky\*.wav"; DestDir: "{app}\sounds\Windows 7\Quirky"
Source: ".\bin\Release\net9.0-windows\sounds\Windows 7\Raga\*.wav"; DestDir: "{app}\sounds\Windows 7\Raga"
Source: ".\bin\Release\net9.0-windows\sounds\Windows 7\Savanna\*.wav"; DestDir: "{app}\sounds\Windows 7\Savanna"
Source: ".\bin\Release\net9.0-windows\sounds\Windows 7\Sonata\*.wav"; DestDir: "{app}\sounds\Windows 7\Sonata"
Source: ".\bin\Release\net9.0-windows\Microsoft.WindowsAPICodePack.dll"; DestDir: "{app}"
Source: ".\bin\Release\net9.0-windows\Microsoft.WindowsAPICodePack.Shell.dll"; DestDir: "{app}"
Source: ".\bin\Release\net9.0-windows\Windows Sound Changer.deps.json"; DestDir: "{app}"
Source: ".\bin\Release\net9.0-windows\Windows Sound Changer.dll"; DestDir: "{app}"
Source: ".\bin\Release\net9.0-windows\Windows Sound Changer.exe"; DestDir: "{app}"
Source: ".\bin\Release\net9.0-windows\Windows Sound Changer.pdb"; DestDir: "{app}"
Source: ".\bin\Release\net9.0-windows\Windows Sound Changer.runtimeconfig.json"; DestDir: "{app}"

[Icons]
Name: "{group}\Windows Sound Changer"; Filename: "{app}\Windows Sound Changer.exe"
