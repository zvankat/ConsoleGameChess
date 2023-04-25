using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 5-ой создаю структуру шахматных клеток на доске
namespace ChessGameConsole
{
    // структура шахматных клеток доски
    struct SquareBoard  
    {
        // на случай ошибочных координат (если координата отсутствует)
        public static SquareBoard none = new SquareBoard(-1, -1);
        // задаю координаты для клетки - (x, y)
        public int x { get; private set; }
        public int y { get; private set; }

        // указываю конструктор для этой структуры
        public SquareBoard (int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        // пишу конструктор для ввода с клавиатуры
        // по координатам - двумя символами
        public SquareBoard (string e2)
        {
            // условия для квадрата шахматной доски
            // проверка длины для части хода, проверка по вертикали и горизонтали квадрата доски 
            // от '1' до '8' и от 'a' до 'h'
            if ((e2.Length == 2) &&
                (e2[0] >= 'a') && (e2[0] <= 'h') &&
                (e2[1] >= '1') && (e2[1] <= '8'))
            {
                // вычисляю
                x = e2[0] - 'a';
                y = e2[1] - '1';
            }
            // в случае некорректных координат
            else // если координата отсутствует
            {
                this = none;
            }
        }

        // пишу метод для проверки
        // находится ли фигура на шахматной доске
        public bool FigureOnBoard()
        {   // проверка каждой координаты
            return (x >= 0) && (x < 8) && (y >= 0) && y < 8 ;
        }

        // для возврата наименования возможного хода
        // первая координата - 'a' + x
        public string NameCoordinates { get { return((char)('a' + x)).ToString() + (y + 1).ToString(); } }

        // для оператора равенства "=="
        public static bool operator == (SquareBoard a, SquareBoard b)
        {
            return a.x == b.x && a.y == b.y;
        }

        // для оператора неравенста "!="
        public static bool operator != (SquareBoard a, SquareBoard b)
        {
            // по закону де Моргана (правила де Мо́ргана)
            //return a.x != b.x && a.y != b.y;
            // или: return !(a == b);
            return !(a == b);
        }

        // для всех квадратов на шахматной доске
        public static IEnumerable<SquareBoard> EnumerationAllSquares()
        {
            // в двух циклах перебираю все клетки
            for (int y = 0; y < 8; y++)
                for (int x = 0; x < 8; x++)
                    // возвращаю очередную клетку
                    yield return new SquareBoard(x, y);
        }
 
    }
}
