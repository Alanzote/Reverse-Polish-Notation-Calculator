using System;
using System.Collections.Generic;
using System.Linq;

namespace CI.Calculator {

    /**
     * The Calculator Class.
     * Used to calculate expressions.
     */
    public class Calculator {

        // Our Operation Class.
        public class Operation {

            // Operation Container Class.
            public class Container {

                // The Value associated to this Container.
                private object _Value;

                // The Value Property.
                public object Value {
                    get => _Value;
                    set {
                        // Check for Type.
                        if (value.GetType() != typeof(int) && value.GetType() != typeof(Operation))
                            throw new ArgumentException("Invalid type for Container Value.", nameof(Value));

                        // Set Value.
                        _Value = value;
                    }
                }

                // Gets the Value as an Int.
                public int GetCalculatedValue() {
                    // Check for Int, return it.
                    if (IsInt())
                        return (int) Value;
                    
                    // It is operation, calculate it.
                    return ((Operation) Value).Calculate();
                }

                // Checks for the Value Type as Int.
                public bool IsInt() {
                    // Check for Type.
                    return Value is int;
                }

                // Implicit conversion to int.
                public static implicit operator int(Container Cont) => Cont.GetCalculatedValue();
            }

            // The Operation Type.
            public enum OpType : int {
                None = 0,
                Add = '+',
                Subtract = '-',
                Multiply = '*',
                Divide = '/',
                Exponent = '|',
                SquareRoot = '&'
            }

            // The Current Operation Type.
            public OpType Type = OpType.None;
            
            // The List of Numbers.
            public List<Container> Numbers = new List<Container>();

            // The Parent operation of this Operation.
            public Operation Parent;

            // Validates this Expression.
            public void Validate() {
                // Check for Null.
                if (!Numbers.Any())
                    throw new Exception("This expression has no numbers associated to it.");

                // Switch Op Type.
                switch (Type) {
                    // Check for None (parser error).
                    case OpType.None: throw new Exception("Invalid Operation Type None.");
                    // On Add, Subtract or Multiply.
                    case OpType.Add:
                    case OpType.Subtract:
                    case OpType.Multiply:
                    case OpType.Divide:  {
                        // These can have any number of numbers associated to it, so we do no validations.
                    } break;
                    case OpType.Exponent: {
                        // These can only have two numbers associated at max, so we validate it.
                        if (Numbers.Count > 2)
                            throw new Exception($"Can not use {Type} with more than 2 numbers.");
                    } break;
                    case OpType.SquareRoot: {
                        // For the Square Root, we can only have a single number.
                        if (Numbers.Count > 1)
                            throw new Exception("Can not use SquareRoot on multiple numbers.");
                    } break;
                }
            }

            // Calculates this Expression.
            public int Calculate() {
                // Validate the Expression First.
                Validate();

                // Get all Values as Numbers.
                int[] ConvertedValues = Numbers.Select(x => x.GetCalculatedValue()).ToArray();

                // We've reached here, which means that it is valid, therefore...
                int Result = 0;

                // Switch Type.
                switch (Type) {
                    // On Add.
                    case OpType.Add: {
                        // Result is the Sum of all Items in our Stack.
                        Result = ConvertedValues.Sum();
                    } break;
                    // On Subtract.
                    case OpType.Subtract: {
                        // Result is the First multiplied by 2, subtracted by the sum in our Stack.
                        Result = ConvertedValues.First() * 2 - ConvertedValues.Sum();
                    } break;
                    // On Multiply.
                    case OpType.Multiply: {
                        // Result is all values multiplied.
                        Result = ConvertedValues.Aggregate((X, Y) => X * Y);
                    } break;
                    // On Divide.
                    case OpType.Divide: {
                        // Result is all values (which there will only be two), divided.
                        Result = ConvertedValues.Aggregate((X, Y) => X / Y);
                    } break;
                    // On Exponent.
                    case OpType.Exponent: {
                        // Result is all values (which there will only be two), first exponent the last.
                        Result = (int) Math.Pow(ConvertedValues.First(), ConvertedValues.Last());
                    } break;
                    // On Square Root.
                    case OpType.SquareRoot: {
                        // Result is the square root of the first value.
                        Result = (int) Math.Sqrt(ConvertedValues.First());
                    } break;
                }

                // Return Result.
                return Result;
            }

            // Implicit conversion to int.
            public static implicit operator int(Operation Op) => Op.Calculate();
        }
    }
}
