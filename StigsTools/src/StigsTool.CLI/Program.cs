// See https://aka.ms/new-console-template for more information

using System.CommandLine;

Console.WriteLine("Hello, World!");

// var fileOption = new Option<FileInfo?>(
//   name: "--file",
//   description: "The file to read and display on the console.");
//
// var rootCommand = new RootCommand("Stigs Tools.");
//
// rootCommand.Add
//
// // .InvokeAsync(args);
//
//
//
// rootCommand.AddOption(fileOption);
//
// rootCommand.SetHandler((file) =>
//                        {
//                          ReadFile(file!);
//                        },
//                        fileOption);
//
// return await rootCommand.InvokeAsync(args);
//
//
//
// public static class CLIExtensions {
//   public static CLI AddVerb(this CLI @this, object[] args) {
//     foreach (var arg in args) {
//         if(arg.GetType() == typeof(Option<>))
//     }
//   }
//
//
//
// }