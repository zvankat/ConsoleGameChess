using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 6-й создаю класс 'фигура на клетке'
namespace ChessGameConsole
{
    class FigureOnSquare
    {
        // какая фигура
        public ChessFigures figure { get; private set; }
        // на какой клетке доски
        public SquareBoard square { get; private set; }

        // конструктор FigureOnSquare для хранения какая фигура на какой клетке
        public FigureOnSquare(ChessFigures figure, SquareBoard square)
        { 
            // какая фигура, какая клетка
            this.figure = figure;
            this.square = square;
        }

    }
}