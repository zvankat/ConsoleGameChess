using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessGameConsole;

// 2-й создаю консоль
// для тестирования кода
namespace ChessGameConsole
{
    // для консоли в свойствах "Шрифт"
    // выбрать "Lucida Console"
    // установить размер "20"
    class ProgramChess
    {
        static void Main(string[] args)
        {
            // для игры с компьютером - генератор случайных чисел
            Random randomMove = new Random();
            // повторно создаю класс для шахмат
            ChessGameConsole.ChessClass chess = new ChessClass();
            // временно убираю часть пешек для проверки ходов
            //GameShess.ChessClass chess = new ChessClass("rnbqkbnr/1p1111p1/8/8/8/8/1P1111P1/RNBQKBNR w KQkq - 0 1");
            List<string> listmoves; // список доступных ходов
            // бесконечный цикл
            while (true)
            {
                // получаю возможные ходы
                listmoves = chess.GetLotsOfMoves();
                // вывод в консоль шахмат
                Console.WriteLine(chess.fen);
                // сперва:   Console.WriteLine(ChessToASCII(chess));
                // делаю в цвете:
                PrintColorChess(ChessToASCII(chess));
                // вывожу есть ли шах?!
                Console.WriteLine(chess.PutInCheck() ? "ВНИМАНИЕ - ШАХ !!!" : "-");
                // вывожу на экран список всех возможных ходов игрока
                foreach (string moves in listmoves) 
                    Console.Write(moves + " | ");
                Console.WriteLine();
                Console.WriteLine("Введите свой ход >>>| ");
                // приглашение ввести свой ход
                Console.WriteLine();
                string move = Console.ReadLine();
                // если пустая строка - пропускаю
                if (move == "q") break;
                // если ход не указан (нажат просто ввод Enter)
                // генерация хода (listmoves.Count - количество возможных ходов)
                if (move == "") move = listmoves[randomMove.Next(listmoves.Count)];
                // если введено значение - делаю ход
                chess = chess.MoveFigure(move);
            }
        }

        // чтобы вывести шахматы в ASCII формате
        static string ChessToASCII(ChessClass newGameChess)
        {   // рисую край доски
            string textboard = "  +-----------------+\n";
            for (int y = 7; y >= 0; y--)
            {
                // добавляю единицу ( y - строка )
                textboard += y + 1;
                textboard += " | ";
                // и перебираю все клетки
                for (int x = 0; x < 8; x++)
                    // добавляю символ фигуры и пробел
                    textboard += newGameChess.GetFigureAtShessBoard(x, y) + " ";
                // закрываю строчку
                textboard += "|\n";
            }
            // закрываю доску в конце
            textboard += "  +-----------------+\n";
            // вывожу шахматные обозначения
            textboard += "    a b c d e f g h\n";
            return textboard;
        }

        static void PrintColorChess(string textboard)
        {
            // сохраняю цвет в переменную
            // 'ForegroundColor' - возвращает или задает цвет фона консоли
            ConsoleColor beforeForeColor = Console.ForegroundColor;
            // обхожу в цикле всю доску
            foreach (char x in textboard)
            {
                // для Черных (маленьких)
                if (x >= 'a' && x <= 'z')
                    // 'ConsoleColor' - задаю константы, которые определяют цвет
                    Console.ForegroundColor = ConsoleColor.Blue;
                // для Белых (заглавных)
                else if (x >= 'A' && x <= 'Z')
                    Console.ForegroundColor = ConsoleColor.White;
                // для прочих символов
                else
                    Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(x);
            }
            Console.ForegroundColor = beforeForeColor;
        }

    }
}
