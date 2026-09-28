#region copyright LGPL nanoLogika
//  Copyright 2026, nanoLogika GmbH.
//  All rights reserved. 
//  This source code is licensed under the "LGPL v3 or any later version" license. 
//  See LICENSE file in the project root for full license information.
#endregion


using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;


namespace VariableEvaluator {

    public static class Evaluator {

        /// <summary>
        /// Parses simple assignment code and evaluates expressions into a dictionary.
        /// Supports +, -, *, / and previously defined variables.
        /// </summary>
        public static Dictionary<string, double> Evaluate(string code) {
            var result = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);

            code = code.Replace("\n", string.Empty).Replace("\r", string.Empty);

            var lines = code.Split(';', StringSplitOptions.RemoveEmptyEntries);

            foreach (var rawLine in lines) {
                var line = rawLine.Trim();
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts = line.Split('=');
                if (parts.Length != 2) {
                    throw new InvalidOperationException($"Invalid line: {line}");
                }

                var variable = parts[0].Trim();
                var expression = parts[1].Trim();

                var value = EvaluateExpression(expression, result);
                result[variable] = value;
            }

            return result;
        }

        private static double EvaluateExpression(string expression, Dictionary<string, double> variables) {
            // Replace variables with values
            foreach (var kvp in variables) {
                expression = Regex.Replace(
                    expression,
                    $"\\b{kvp.Key}\\b",
                    kvp.Value.ToString(CultureInfo.InvariantCulture));
            }

            // Use DataTable for basic math evaluation
            var table = new DataTable();
            var result = table.Compute(expression, string.Empty);

            return Convert.ToDouble(result, CultureInfo.InvariantCulture);
        }
    }
}

// Example usage:
// var code = @"
// w = 200;
// h = 200;
// d1 = 20;
// d0 = 20;
// xl = -w/2;
// xr = w/2;
// xil = xl+d1;
// xir = xr-d1;
// yu = 0;
// yiu = yu + d0;
// yo = h;";
// var dict = Evaluator.Evaluate(code);
// foreach (var kv in dict)
//     Console.WriteLine($"{kv.Key} = {kv.Value}");
