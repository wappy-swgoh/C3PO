﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using Newtonsoft.Json;
using DSharpPlus;

namespace C_3PO_bot
{
    internal sealed class Program
    {
        public static void Main(string[] args)
            => new Program().Run(args).GetAwaiter().GetResult();

        private async Task Run(string[] args)
        {
            /*
            var cfg = new C3POBotConfig();
            var json = string.Empty;
            if (!File.Exists("config.json"))
            {
                json = JsonConvert.SerializeObject(cfg);
                File.WriteAllText("config.json", json, new UTF8Encoding(false));
                Console.WriteLine("Config file was not found, a new one was generated. Fill it with proper values and rerun this program");
                Console.ReadKey();

                return;
            }

            json = File.ReadAllText("config.json", new UTF8Encoding(false));
            cfg = JsonConvert.DeserializeObject<C3POBotConfig>(json);

            var tskl = new List<Task>();
            for (var i = 0; i < cfg.ShardCount; i++)
            {
                var bot = new C3POBot(cfg, i);
                tskl.Add(bot.RunAsync());
                await Task.Delay(7500);
            }

            await Task.WhenAll(tskl);
            */
            var dcfg = new DiscordConfig
            {
                AutoReconnect = true,
                DiscordBranch = Branch.Stable,
                LargeThreshold = 250,
                LogLevel = LogLevel.Unnecessary,
                Token = "MzEyNDkwMzU4MTI5MDMzMjE3.C_b06A.dwytB2wEycnwI2slNToh3uKkZpg",
                TokenType = TokenType.Bot,
                UseInternalLogHandler = false
            };
            var bot = new DiscordShardedClient(dcfg);
            bot.DebugLogger.LogMessageReceived += DebugLogger_LogMessageReceived;
            await bot.StartAsync();
            bot.UseInteractivity();
            var modules = bot.UseCommandsNext(new CommandsNextConfiguration { Prefix = "#" });
            foreach (var m in modules.Values)
            {
                m.RegisterCommands<C3POBotCommands>();
                m.RegisterCommands<C3POBotNextCommands>();
                m.RegisterCommands<C3POBotEvalCommands>();
            }

            await Task.Delay(-1);
        }

        private void DebugLogger_LogMessageReceived(object sender, DebugLogMessageEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("[{0:yyyy-MM-dd HH:mm:ss zzz}] ", e.Timestamp.ToLocalTime());

            var tag = e.Application;
            if (tag.Length > 12)
                tag = tag.Substring(0, 12);
            if (tag.Length < 12)
                tag = tag.PadLeft(12, ' ');
            Console.Write("[{0}] ", tag);

            //Console.ForegroundColor = ConsoleColor.Yellow;
            //Console.Write("[{0}] ", string.Concat("SHARD ", this.Discord.ShardId.ToString("00")));

            switch (e.Level)
            {
                case LogLevel.Critical:
                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;

                case LogLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;

                case LogLevel.Info:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;

                case LogLevel.Debug:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;

                case LogLevel.Unnecessary:
                default:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;
            }
            Console.Write("[{0}] ", e.Level.ToString().PadLeft(11));

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(e.Message);
        }
    }
}