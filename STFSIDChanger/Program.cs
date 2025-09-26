using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using X360.STFS;
using System.IO;
namespace STFSIDChanger
{
    class Program
    {
        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length / 2).Select(x => Convert.ToByte(hex.Substring(x * 2, 2), 16)).ToArray();
        }
        static void howTo()
        {
            Console.WriteLine("Arg 1: directory");
            Console.WriteLine("Arg 2: profileid, deviceid, or both");
            Console.WriteLine("Arg 3: value1");
            Console.WriteLine("Arg 4 (if arg2 is `both`): value2");
            Console.WriteLine("Example 1: STFSIDChanger.exe \"savegames\\\" profileid E000000000000000");
            Console.WriteLine("Example 2: STFSIDChanger.exe \"savegames\\\" deviceid 0000000000000000000000000000000000000000");
            Console.WriteLine("Example 3: STFSIDChanger.exe \"savegames\\\" both E000000000000000 0000000000000000000000000000000000000000");
        }
        static void Main(string[] args)
        {
            if (!File.Exists("KV.bin"))
            {
                File.WriteAllBytes("KV.bin", Properties.Resources.KV);
            }
            RSAParams kv = new RSAParams("KV.bin");
            if (!kv.Valid)
            {
                Console.WriteLine("Cannot load KV");
                return;
            }
            if (args.Length == 3 || args.Length == 4)
            {
                if (Directory.Exists(args[0]))
                { 
                    string[] files = Directory.GetFiles(args[0]);
                    for (int i = 0; i < files.Length; i++)
                    {
                        try
                        {
                            Console.WriteLine("Attempting to parse " + files[i] + " as an STFS package");
                            STFSPackage stfs = new STFSPackage(files[i], null);
                            switch (args[1].ToLower())
                            {
                                case "profileid":
                                    if (args[2].Length != 16) Console.WriteLine("Invalid argument length for profileid");
                                    else
                                    {
                                        string og = stfs.Header.ProfileID.ToString("X");
                                        stfs.Header.ProfileID = long.Parse(args[2], NumberStyles.HexNumber);
                                        Console.WriteLine(og + " -> " + args[2]);
                                    }
                                    break;
                                case "deviceid":
                                    if (args[2].Length != 40) Console.WriteLine("Invalid argument length for deviceid");
                                    else
                                    {
                                        string og = BitConverter.ToString(stfs.Header.DeviceID).Replace("-", "");
                                        stfs.Header.DeviceID = StringToByteArray(args[2]);
                                        Console.WriteLine(og + " -> " + args[2]);
                                    }
                                    break;
                                case "both":
                                    if (args[2].Length != 16 || args[3].Length != 40) Console.WriteLine("Invalid argument length for profileid or deviceid");
                                    else
                                    {
                                        string og = stfs.Header.ProfileID.ToString("X");
                                        string og2 = BitConverter.ToString(stfs.Header.DeviceID).Replace("-", "");
                                        stfs.Header.ProfileID = long.Parse(args[2], NumberStyles.HexNumber);
                                        stfs.Header.DeviceID = StringToByteArray(args[3]);
                                        Console.WriteLine(og + " -> " + args[2]);
                                        Console.WriteLine(og2 + " -> " + args[3]);
                                    }
                                    break;
                                default:
                                    howTo();
                                    break;
                            }
                            Console.WriteLine("Flushing package");
                            stfs.FlushPackage(kv);
                            Console.WriteLine("Done");
                            stfs.CloseIO();
                            Console.WriteLine("Package closed");
                            Console.WriteLine();
                        }
                        catch(Exception ex) { Console.WriteLine("Error: " + ex.Message);  continue; }
                    }
                }
            }
            else howTo();
        }
    }
}
