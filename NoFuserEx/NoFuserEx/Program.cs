﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace NoFuserEx {
    class Program {
        static void Main(string[] args) {
            Logger.Information();

            if (args.Length == 0) {
                Logger.Help();
                Logger.Exit();
            }

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var file = GetOptions(args);
            var assemblyManager = new AssemblyManager(file);
            assemblyManager.LoadAssembly();
            var deobfuscator = new Deobfuscator.DeobfuscatorManager(assemblyManager);
            deobfuscator.Start();
            assemblyManager.SaveAssembly();
            Options.RestoreOptions();

            stopWatch.Stop();
            Logger.WriteLine(string.Empty);
            Logger.Info(
                $"Elapsed time: {stopWatch.Elapsed.Minutes}:{stopWatch.Elapsed.Seconds}:{stopWatch.Elapsed.Milliseconds}");


            Logger.Exit();
        }

        static string GetOptions(IEnumerable<string> args) {
            string file = null;
            Logger.Verbose("Checking options...");
            foreach (var arg in args) {
                switch (arg) {
                    case "--force-deob":
                        Logger.VeryVerbose("Force deobfuscation option detected.");
                        Options.ForceDeobfuscation = true;
                        break;
                    case "--dont-unpack":
                        Logger.VeryVerbose("Don't unpack module option detected.");
                        Options.NoUnpack = true;
                        break;
                    case "--dont-tamper":
                        Logger.VeryVerbose("Don't decrypt anti-tampering option detected.");
                        Options.NoTamper = true;
                        break;
                    case "--dont-strings":
                        Logger.VeryVerbose("Don't decrypt strings option detected.");
                        Options.NoStrings = true;
                        break;
                    case "--dont-cflow":
                        Logger.VeryVerbose("Don't deobfuscate control flow option detected.");
                        Options.NoControlFlow = true;
                        break;
                    case "--dont-proxy-calls":
                        Logger.VeryVerbose("Don't fix proxy calls option detected.");
                        Options.NoProxyCalls = true;
                        break;
                    case "--dont-remove-junk-methods":
                        Logger.VeryVerbose("Don't remove junk methods option detected.");
                        Options.NoRemoveJunkMethods = true;
                        break;
                    case "--dont-resources":
                        Logger.VeryVerbose("Don't decrypt resouces option detected.");
                        Options.NoResources = true;
                        break;
                    case "--dont-rename":
                        Logger.VeryVerbose("Don't rename option detected.");
                        Options.NoRename = true;
                        break;
                    case "-v":
                        Options.Verbose = true;
                        break;
                    case "-vv":
                        Options.Verbose = true;
                        Options.VeryVerbose = true;
                        break;
                    default:
                        if (!File.Exists(arg))
                            break;
                        var extension = Path.GetExtension(arg);
                        switch (extension) {
                            case ".exe":
                            case ".dll":
                            case ".netmodule":
                                Logger.VeryVerbose($"Extension file: {extension}");
                                file = arg;
                                break;
                            default:
                                throw new Exception("Invalid file extension!");
                        }
                        break;
                }
            }
            return file;
        }
    }
}
