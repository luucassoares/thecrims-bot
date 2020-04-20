﻿using System;
using System.Threading.Tasks;
using thecrims_bot.services;
using thecrims_bot.models;
using thecrims_bot.parser;
using thecrims_bot.console;

namespace thecrims_bot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //TCParser p = new TCParser();
            //p.parseUser("a");

            //TCComands command = new TCComands();
            //command.showInfo();

            User user = new User();
            TCServices service = new TCServices();

            Console.WriteLine("  _______ _           _____      _                 ____        _   ");
            Console.WriteLine(" |__   __| |         / ____|    (_)               |  _ \\      | |  ");
            Console.WriteLine("    | |  | |__   ___| |     _ __ _ _ __ ___  ___  | |_) | ___ | |_ ");
            Console.WriteLine("    | |  | '_ \\ / _ \\ |    | '__| | '_ \\` _\\/ __| |  _ < / _ \\| __|");
            Console.WriteLine("    | |  | | | |  __/ |____| |  | | | | | | \\__ \\ | |_) | (_) | |_ ");
            Console.WriteLine("    |_|  |_| |_|\\___|\\_____|_|  |_|_| |_| |_|___/ |____/ \\___/ \\__|");
            Console.WriteLine("                                                                   ");
            Console.WriteLine("                                                                   ");

            Console.WriteLine("Bem vindo ao The Crims Bot!");
            Console.WriteLine("Realize o login");
            Console.Write("Usuário -> ");
            string username = "lukintest";
            Console.Write("Senha -> ");
            string password = "1cfaf1125d6d";

            await service.LoginAsync(username, password);

            await service.getUser();

            if (service.logged)
            {

                while (true)
                {
                    await service.Rob();
                }


            }

        }
    }
}
