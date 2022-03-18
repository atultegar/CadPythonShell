﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using WixSharp;
using WixSharp.CommonTasks;
using WixSharp.Controls;

string rootDirectory = Path.GetPathRoot(Environment.SystemDirectory);
string installationDir = Path.Combine(rootDirectory, @"ProgramData\\Autodesk\\ApplicationPlugins\\CADPythonShell.bundle\\");
const string projectName = "CADPythonShell";
const string outputName = "CADPythonShell";
const string outputDir = "output";
const string version = "1.0.1";

var fileName = new StringBuilder().Append(outputName).Append("-").Append(version);
var project = new Project
{
    Name = projectName,
    OutDir = outputDir,
    Platform = Platform.x64,
    Description = "Project Support Developer Write Python In Autocad And Civil3D",
    UI = WUI.WixUI_InstallDir,
    Version = new Version(version),
    OutFileName = fileName.ToString(),
    InstallScope = InstallScope.perUser,
    MajorUpgrade = MajorUpgrade.Default,
    GUID = new Guid("E96F5137-040E-426E-A53C-EB492753C25E"),
    BackgroundImage = @"Installer\Resources\Icons\BackgroundImage.png",
    BannerImage = @"Installer\Resources\Icons\BannerImage.png",
    ControlPanelInfo =
    {
        Manufacturer = "Autodesk",
        HelpLink = "https://github.com/chuongmep/CadPythonShell/issues",
        Comments = "Project Support Developer Write Python In Autocad And Civil3D",
        ProductIcon = @"Installer\Resources\Icons\ShellIcon.ico"
    },
    Dirs = new Dir[]
    {
        new InstallDir(installationDir, GenerateWixEntities())
    }
};

MajorUpgrade.Default.AllowSameVersionUpgrades = true;
project.RemoveDialogsBetween(NativeDialogs.WelcomeDlg, NativeDialogs.InstallDirDlg);
project.BuildMsi();

WixEntity[] GenerateWixEntities()
{
    Console.WriteLine("Start Create Installer");
    var versionRegex = new Regex(@"\d+");
    var versionStorages = new List<WixEntity>();
    if (args.Length == 0) Console.WriteLine("Have some Problem with args build installer");
    double count = 0;
    foreach (var directory in args)
    {
        Console.WriteLine($"Working with Directory: {directory}");
        var directoryInfo = new DirectoryInfo(directory);
        var fileVersion = versionRegex.Match(directoryInfo.Name).Value;
        var files = new Files($@"{directory}\*.*");
        versionStorages.Add(files);
        var assemblies = Directory.GetFiles(directory, "*", SearchOption.AllDirectories);
        Console.WriteLine($"Start add some files to msi: ");
        foreach (var assembly in assemblies)
        {
            Console.WriteLine($"'{assembly}'");
            count++;
        }
    }

    Console.WriteLine($"Added {count} files to msi");
    return versionStorages.ToArray();
}