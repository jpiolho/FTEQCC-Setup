using System;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Net;
using WixSharp;
using File = WixSharp.File;

namespace FTEQCC
{
    internal class Program
    {
        private const string ProductGuid = "72d135dc-1ffc-4fb0-8d5d-b09c534aa0b0";
        private const string ExtractedConsoleExecutableName = "fteqcc64.exe";
        private const string ExtractedGuiExecutableName = "fteqccgui64.exe";

        private const string ConsoleExecutableName = "fteqcc.exe";
        private const string GuiExecutableName = "fteqccgui.exe";
        private const string IconName = "icon.ico";

        static void Main(string[] args)
        {
            string downloadPathGui = "Files/temp/gui.zip";
            string downloadPathConsole = "Files/temp/console.zip";
            string extractPath = "Files/X64/";

            Console.WriteLine("Parsing version");
            var version = Version.Parse(Environment.GetEnvironmentVariable("SETUP_VERSION"));

            // Clear directory first
            Console.WriteLine("Clearing directories");
            if (Directory.Exists(extractPath))
            {
                Console.WriteLine("Clearing extraction folder");
                Directory.Delete(extractPath, true);
            }
            Directory.CreateDirectory(extractPath);

            // Extract files
            Console.WriteLine("Extracting zips");
            ExtractZipFile(downloadPathGui, extractPath);
            ExtractZipFile(downloadPathConsole, extractPath);

            // Extract icon
            Console.WriteLine("Extracting icon");
            ExtractIcon(Path.Combine(extractPath, ExtractedGuiExecutableName), IconName);

            // Rename files
            Console.WriteLine("Renaming files");
            RenameFile(Path.Combine(extractPath, ExtractedGuiExecutableName), GuiExecutableName);
            RenameFile(Path.Combine(extractPath, ExtractedConsoleExecutableName), ConsoleExecutableName);

            Console.WriteLine("Building Wix definitions");
            var project = new Project("FTEQCC",
                            new Dir(@"%ProgramFiles64Folder%\FTEQCC",
                                new File($"Files/X64/{GuiExecutableName}"),
                                new File($"Files/X64/{ConsoleExecutableName}"),
                                new File("Files/X64/fteqcc_manual.txt")
                            ),
                            new Dir(@"%Desktop%",
                                new ExeFileShortcut("FTEQCC GUI", $"[INSTALLDIR]{GuiExecutableName}", "")
                                {
                                    IconFile = IconName
                                }
                            ),
                            new Dir(@"%ProgramMenu%\FTEQCC",
                                new ExeFileShortcut("FTEQCC GUI", $"[INSTALLDIR]{GuiExecutableName}", "")
                                {
                                    IconFile = IconName
                                }
                            ),
                            new EnvironmentVariable("Path", @"[INSTALLDIR]")
                            {
                                Id = "Path_INSTALLDIR",
                                Action = EnvVarAction.set,
                                Part = EnvVarPart.last,
                                Permanent = false,
                                System = true
                            }
            )
            {
                Description = "The most advanced QuakeC compiler",
                Platform = Platform.x64,
                ControlPanelInfo =
                {
                    ProductIcon = IconName
                },
                Version = version,
                GUID = new Guid(ProductGuid)
            };

            Compiler.BuildMsi(project);
        }

        static void RenameFile(string file, string newName)
        {
            Console.WriteLine($"Renaming file: {file} | {newName}");
            System.IO.File.Move(file, Path.Combine(Path.GetDirectoryName(file), newName));
        }

        static void ExtractIcon(string exe, string destinationPath)
        {
            using (var icon = Icon.ExtractAssociatedIcon(exe))
            using (var fs = new FileStream(destinationPath, FileMode.OpenOrCreate))
            {
                icon.Save(fs);
            }
        }

        static void ExtractZipFile(string zipPath, string extractPath)
        {
            Directory.CreateDirectory(extractPath);

            using (var archive = ZipFile.OpenRead(zipPath))
            {
                foreach (ZipArchiveEntry file in archive.Entries)
                {
                    string completeFileName = Path.Combine(extractPath, file.FullName);
                    string directory = Path.GetDirectoryName(completeFileName);

                    if (!Directory.Exists(directory))
                        Directory.CreateDirectory(directory);

                    if (file.Name != "")
                        file.ExtractToFile(completeFileName, true);
                }
            }
        }
    }
}
