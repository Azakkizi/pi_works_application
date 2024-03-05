using System;

class Program {

    static string ReversedSentence(string input) {
        string[] words = input.Split(' ');

        Array.Reverse(words);

        string reversedString = string.Join(" ", words);

        return reversedString;
    }

    static void Main() {
        string input1 = "The weather is so sunny nowadays";
        string output1 = ReversedSentence(input1);
        Console.WriteLine(output1);

        string input2 = "Life is so beautiful";
        string output2 = ReversedSentence(input2);
        Console.WriteLine(output2);
    }

}