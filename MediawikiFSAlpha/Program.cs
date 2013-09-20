using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dokan;
using WikiTools.Access;

namespace MediawikiFSAlpha
{
    class Program
    {
        private static string serverUrl;
        private static string serverUser;
        private static string serverPassword;

        static void Main(string[] args)
        {

            Console.WriteLine("Welcome to MediawikiFS (alpha)");
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("Created by Vedmaka <god.vedmaka@gmail.com>");
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("");

            if (args == null || args.Length == 0)
            {
                welcome();
            }
            else
            {
                serverUrl = args[0];
                if (args.Length > 1)
                {
                    serverUser = args[1];
                }
                if (args.Length > 2)
                {
                    serverPassword = args[2];
                }
                Console.WriteLine("Automounting.. for "+serverUrl);
                mountFS();
                
            }

        }

        static void welcome()
        {
            Console.WriteLine("Please specify mediawiki url (eg: server.com): ");
            serverUrl = Console.ReadLine();
            Console.WriteLine("Please specify login (leave empty for guest):");
            serverUser = Console.ReadLine();
            if (!String.IsNullOrEmpty(serverUser))
            {
                Console.WriteLine("Please specify password:");
                serverPassword = Console.ReadLine();
            }

            Console.WriteLine("Connecting to " + serverUrl + " ..");

            Wiki w;

            try
            {
                w = new Wiki("http://" + serverUrl);
                if (!String.IsNullOrEmpty(serverUser))
                {
                    if (!w.Login(serverUser, serverPassword))
                    {
                        Console.WriteLine("ERROR: Can not login with specified credentials.");
                        Console.ReadLine();
                        return;
                    }
                }
            }
            catch (System.Net.WebException)
            {
                Console.WriteLine("ERROR: Can not connect to specified host.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Mediawiki connected, version: " + w.Capabilities.Version);
            Console.WriteLine("Is logged in: " + w.IsLoggedIn());

            Console.WriteLine("Ready to mount, press ENTER key ..");
            Console.ReadLine();

            Console.WriteLine("Mounting remote FS to R:");

            w = null;
            mountFS();
        }

        static void mountFS()
        {
            DokanOptions opt = new DokanOptions();
            opt.MountPoint = "r:\\";
            opt.DebugMode = false;
            //opt.UseStdErr = true;
            opt.VolumeLabel = "MediawikiFS";
            //opt.UseKeepAlive = true;
            //opt.UseAltStream = true;
            opt.RemovableDrive = true;
            int status = DokanNet.DokanMain(opt, new MWFS(serverUrl, serverUser, serverPassword));
            switch (status)
            {
                case DokanNet.DOKAN_DRIVE_LETTER_ERROR:
                    Console.WriteLine("Drvie letter error");
                    break;
                case DokanNet.DOKAN_DRIVER_INSTALL_ERROR:
                    Console.WriteLine("Driver install error");
                    break;
                case DokanNet.DOKAN_MOUNT_ERROR:
                    Console.WriteLine("Mount error");
                    break;
                case DokanNet.DOKAN_START_ERROR:
                    Console.WriteLine("Start error");
                    break;
                case DokanNet.DOKAN_ERROR:
                    Console.WriteLine("Unknown error");
                    break;
                case DokanNet.DOKAN_SUCCESS:
                    Console.WriteLine("Success");
                    Console.WriteLine("Mounted. Close window to unmount.");
                    //Process.Start("explorer.exe", @"r:\");
                    break;
                default:
                    Console.WriteLine("Unknown status: %d", status);
                    break;

            }
        }

    }
}
