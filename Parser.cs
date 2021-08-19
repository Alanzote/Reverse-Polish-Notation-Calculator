using System;
using System.Linq;

namespace CI.Calculator {

    /**
     * The Parser.
     * Reads a string and creates a calculator.
     */
    public class Parser {

        // Parses a Calculator Expression.
        public static int Parse(string Expression) {
            // Check for Expression Length.
            if (string.IsNullOrEmpty(Expression) || Expression.Length <= 1)
                throw new ArgumentException("Invalid input string: Not Enough Characters.", nameof(Expression));

            // Break the String, character by character.
            char[] ToParse = Expression.ToCharArray();

            // If the first character isn't a '(', it is an incorrect format.
            if (ToParse.First() != '(')
                throw new InvalidOperationException($"Invalid character '{ToParse.First()}' at 1.");

            // The Current Level we are at.
            int Level = 0;

            // The Current Operation we are Processing.
            Calculator.Operation CurrentOperation = null;

            // The String we are currently parsing.
            string ParseString = string.Empty;

            // Loop all Characters.
            for (int i = 0; i < ToParse.Length; i++) {
                // The Current Character.
                char Current = ToParse[i];

                // Switch Current.
                switch (Current) {
                    // Check for Open Operation '('.
                    case '(': {
                        // Increment Level.
                        Level++;

                        // Create new Current Operation.
                        CurrentOperation = new Calculator.Operation {
                            // Make sure parent is set.
                            Parent = CurrentOperation
                        };
                    } break;

                    // Check for End Operation ')'.
                    case ')': {
                        // Decrement Level.
                        Level--;

                        // Check for Parent.
                        if (CurrentOperation.Parent != null) {
                            // Push Current Operation to Parent's Stack.
                            CurrentOperation.Parent.Numbers.Add(new Calculator.Operation.Container {
                                // Set Container Value.
                                Value = CurrentOperation
                            });

                            // Set Current Operation to Parent.
                            CurrentOperation = CurrentOperation.Parent;
                        }
                    } break;

                    // Check for Number Input.
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9': {
                        // If our Operation Type is not none, we've got a problem!
                        if (CurrentOperation.Type != Calculator.Operation.OpType.None)
                            throw new InvalidOperationException($"Invalid character '{Current}' at {i + 1}, expected ')'.");

                        // Add to Parse String.
                        ParseString += Current;
                    } break;

                    // Check for a Space.
                    case ' ': {
                        // If we can have a last character and the last character is a ')', continue to next.
                        if (i - 1 >= 0 && ToParse[i - 1] == ')')
                            continue;

                        // Check for Parsed String.
                        if (string.IsNullOrEmpty(ParseString))
                            throw new InvalidOperationException($"Invalid Space at {i + 1}.");

                        // We've got a valid number, let's create a new Container.
                        Calculator.Operation.Container NewContainer = new Calculator.Operation.Container {
                            // Set Value.
                            Value = int.Parse(ParseString)
                        };

                        // Reset Parse String.
                        ParseString = string.Empty;

                        // Push to the Operation Stack.
                        CurrentOperation.Numbers.Add(NewContainer);
                    } break;

                    // Check for Operations.
                    case (char) Calculator.Operation.OpType.Add:
                    case (char) Calculator.Operation.OpType.Subtract:
                    case (char) Calculator.Operation.OpType.Multiply:
                    case (char) Calculator.Operation.OpType.Divide:
                    case (char) Calculator.Operation.OpType.Exponent:
                    case (char) Calculator.Operation.OpType.SquareRoot: {
                        // Check for Operation.
                        if (CurrentOperation.Type != Calculator.Operation.OpType.None)
                            throw new InvalidOperationException($"Invalid character '{Current}' at {i + 1}, expected ')'.");

                        // Set Operation.
                        CurrentOperation.Type = (Calculator.Operation.OpType) Current;
                    } break;

                    // Default, throw exception.
                    default: throw new InvalidOperationException($"Invalid character '{Current}' at {i + 1}.");
                }
            }

            // Check for Level.
            if (Level != 0)
                throw new InvalidOperationException($"Missing ')' at {ToParse.Length}.");

            // Return the Calculation of the Main Container.
            return CurrentOperation.Calculate();
        }
    }
}
