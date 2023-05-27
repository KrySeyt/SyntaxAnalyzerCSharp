/*
 * S -> D = Y|while Y
 * D -> iZ
 * Z -> (YP|EPSILON
 * P -> {, Y})
 * Y -> DA|ZA
 * A ->  B|EPSILON
 * B -> E|T|do S
 * E -> + YA
 * T -> * YA
*/

using System.Diagnostics;

namespace RecursiveDescent;

internal class WrongSequenceException : Exception
{
}

internal class UncompatibleCharException : WrongSequenceException
{
}

internal class TooLongSequenceException : WrongSequenceException
{
}

internal class Analyzer
{
    private const char InputSequenceTerminator = '~';

    public Analyzer(string sequence)
    {
        Sequence = sequence + InputSequenceTerminator;
    }

    public int WrongCharId
    {
        get
        {
            if (ErrorFound)
                return CurrentCharId;
            return -1;
        }
    }

    public string Sequence { get; }
    private bool ErrorFound { get; set; }
    private char CurrentChar { get; set; }
    private int CurrentCharId { get; set; }

    private char NextChar()
    {
        if (CurrentCharId > Sequence.Length - 1) throw new TooLongSequenceException();

        CurrentCharId++;
        CurrentChar = Sequence[CurrentCharId];
        return CurrentChar;
    }

    private char PrevChar()
    {
        Debug.Assert(CurrentCharId > 0);
        CurrentCharId--;
        CurrentChar = Sequence[CurrentCharId];
        return CurrentChar;
    }

    private void T()
    {
        if (CurrentChar != '*')
        {
            PrevChar();
            return;
        }

        if (NextChar() != ' ')
            throw new UncompatibleCharException();

        NextChar();
        Y();
        NextChar();
        A();
    }

    private void B()
    {
        switch (CurrentChar)
        {
            case '+':
                E();
                break;
            case '*':
                T();
                break;
            case 'd':
                for (var i = 0; i < 2; i++)
                    if (NextChar() != "o "[i])
                        throw new UncompatibleCharException();
                NextChar();
                S();
                break;
            default:
                throw new UncompatibleCharException();
        }
    }

    private void A()
    {
        if (CurrentChar == ' ')
        {
            NextChar();
            B();
            return;
        }

        PrevChar();
    }

    private void Y()
    {
        if (CurrentChar == 'i')
        {
            D();
            NextChar();
            A();
        }
        else
        {
            Z();
            NextChar();
            A();
        }
    }

    private void E()
    {
        if (CurrentChar != '+')
        {
            PrevChar();
            return;
        }

        if (NextChar() != ' ')
            throw new UncompatibleCharException();

        NextChar();
        Y();
        NextChar();
        A();
    }

    private void P()
    {
        while (CurrentChar == ',')
        {
            if (NextChar() != ' ')
                throw new UncompatibleCharException();
            NextChar();
            Y();
            NextChar();
        }

        if (CurrentChar == ')') return;

        throw new UncompatibleCharException();
    }

    private void Z()
    {
        if (CurrentChar == '(')
        {
            NextChar();
            Y();
            NextChar();
            P();
            return;
        }

        PrevChar();
    }

    private void D()
    {
        if (CurrentChar != 'i')
            throw new UncompatibleCharException();
        NextChar();
        Z();
    }

    private void S()
    {
        switch (CurrentChar)
        {
            case 'i':
                D();
                for (var i = 0; i < 3; i++)
                    if (NextChar() != " = "[i])
                        throw new UncompatibleCharException();
                NextChar();
                Y();
                break;

            case 'w':
                for (var i = 0; i < 5; i++)
                    if (NextChar() != "hile "[i])
                        throw new UncompatibleCharException();
                NextChar();
                Y();
                break;
            default:
                throw new UncompatibleCharException();
        }
    }

    public bool CheckSequence()
    {
        ErrorFound = false;
        CurrentCharId = 0;
        CurrentChar = Sequence[CurrentCharId];
        try
        {
            S();
            if (NextChar() != InputSequenceTerminator)
                throw new TooLongSequenceException();
            return true;
        }
        catch (WrongSequenceException)
        {
            ErrorFound = true;
            return false;
        }
    }
}

internal class Program
{
    public static void Main()
    {
        Console.WriteLine("Введите входную цепочку: ");
        var inputSequence = Console.ReadLine();

        if (inputSequence is null)
            return;

        if (inputSequence.Length == 0)
        {
            Console.WriteLine("Строка пуста");
            return;
        }

        Analyzer analyzer = new(inputSequence);

        if (analyzer.CheckSequence())
            Console.WriteLine("Входная последовательность корректна");
        else
            Console.WriteLine(
                $"Ошибка: {analyzer.WrongCharId + 1} символ - \"{analyzer.Sequence[analyzer.WrongCharId]}\"");
    }
}