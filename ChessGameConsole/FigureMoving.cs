using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 7-й создаю класс 'фигура делает ход'
namespace ChessGameConsole
{
    class FigureMoving
    {
        // узнаю что это за фигура 'FIGURE'
        public ChessFigures figure { get; private set; }
        // где фигура расположена 'FROM'
        public SquareBoard from { get; private set; }
        // куда фигура идет 'TO'
        public SquareBoard to { get; private set; }
        // отдельно для пешки, на случай преврашения 'PROMOTION'
        public ChessFigures promotion { get; private set; }

        // конструктор FigureMoving (фигура, откуда-куда, есть ли превращение?)
        public FigureMoving (FigureOnSquare fos, SquareBoard to, ChessFigures promotion = ChessFigures.none)
        {
            // записываю какая была фигура
            this.figure = fos.figure;
            // откуда фигура пришла
            this.from = fos.square;
            // куда перемещается
            this.to = to;
            // и превращение если таковое есть
            this.promotion = promotion;
        }

        // вспомогательный конструктор (движение фигуры в текстовом варианте)
        public FigureMoving(string move)
        {
            // пример - "Pe2e4" или "Pe7e8Q" (ход)
            //           01234       012345
            this.figure = (ChessFigures)move[0];
            // для координаты 'from'
            // 'Substring' - с 1-й позиции на длину 2
            this.from = new SquareBoard(move.Substring(1, 2));
            // с 3-й позиции на длину 2
            this.to = new SquareBoard(move.Substring(3, 2));
            // если длина строки равна 6 - значит возможно превращение 'PROMOTION'
            this.promotion = (move.Length == 6) ? (ChessFigures)move[5] : ChessFigures.none;
        }

        // дополнительные функции для проверок
        // разница по координатам 'to.x - from.x'
        public int Difference_X { get { return to.x - from.x; } }
        // разница по координатам 'to.y - from.y'
        public int Difference_Y { get { return to.y - from.y; } }

        // для получения модуля разницы
        // 'mscorlib' - public static int Abs(int value)
        public int ModulusDifference_X { get { return Math.Abs(Difference_X); } }
        public int ModulusDifference_Y { get { return Math.Abs(Difference_Y); } }

        // определяю знак куда идёт фигура, в какую сторону?
        // знак числа - Sign
        public int SignOfFigure_X { get { return Math.Sign(Difference_X); } }
        public int SignOfFigure_Y { get { return Math.Sign(Difference_Y); } }

        // чтобы сформировать ходы ввиде строчки GetLotsOfMoves
        public override string ToString()
        {
            // возвращаю: фигуру, откуда
            string text = (char)figure + from.NameCoordinates + to.NameCoordinates;
            if (promotion != ChessFigures.none)
                // возвращаю фигуру в которую превратилась
                text += (char)promotion;
            return text;
        }

    }
}
