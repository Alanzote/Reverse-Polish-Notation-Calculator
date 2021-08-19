using System;
using System.Collections.Generic;

namespace CI.Calculator {

    /**
     * The Program Class.
     * A Program.
     */
    class Program {

        // The Tests.
        private static readonly Dictionary<string, int> Tests = new Dictionary<string, int> {
            { "(2 3 +)", 5 },
            { "(2 3 4 *)", 24 },
            { "(4 2 2 /)", 1 },
            { "(4 2 |)", 16 },
            { "(4 &)", 2 },
            { "((4 2 +) 3 *)", 18 },
            { "((3 4 +) (4 2 /) *)", 14 },
            { "((3 4 +) (4 (1 1 +) /) *)", 14 }
        };

        // Main Method.
        static void Main(string[] Args) {
            // Header.
            Console.WriteLine("-- Reverse Polish Notation Calculator --");
            Console.WriteLine("By: Alan Renato Bunese");
            Console.WriteLine("For: TDE 1 - Construção de Interpretadores");
            Console.WriteLine("-- Begin Tests --");

            // For Each Test.
            foreach (KeyValuePair<string, int> Pair in Tests) {
                // Try...
                try {
                    // Write Key.
                    Console.Write($"{Pair.Key} = ");
                    
                    // Parse.
                    int Parsed = Parser.Parse(Pair.Key);

                    // Write Value.
                    Console.Write($"{Parsed} = {Pair.Value}. {(Parsed == Pair.Value ? "OK" : "NOT OK.")}");
                } catch (Exception Ex) {
                    // Write Error.
                    Console.Write(Ex.Message);
                }

                // Write Line.
                Console.WriteLine();
            }

            // Notify End Tests.
            Console.WriteLine("-- End Tests --");

            // Loop Forever.
            while (true) {
                // Request Expression.
                Console.Write("Input Expression: ");

                // The String we are reading.
                string Expression = Console.ReadLine();

                Console.SetCursorPosition(0, Console.CursorTop - 1);

                // Write.
                Console.Write($"Input Expression: {Expression} = ");

                // Try...
                try {
                    // Attempt to Parse.
                    int Parsed = Parser.Parse(Expression);

                    // Write the Value.
                    Console.WriteLine(Parsed);
                } catch (Exception Ex) {
                    // There was an error, write the error.
                    Console.WriteLine(Ex.Message);
                }
            }
        }
    }
}
