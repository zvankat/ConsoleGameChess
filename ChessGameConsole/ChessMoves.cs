using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 9-й класс для движений фигур
namespace ChessGameConsole
{
    class ChessMoves
    {
        // движение фигуры
        FigureMoving figmov;
        // доступ к доске
        ChessBoard board;

        // пишу конструктор 'ChessMoves'
        public ChessMoves(ChessBoard board)
        {
            // создаю доску
            this.board = board;
        }

        // возможно ли сейчас движение фигуры
        public bool CanNowFigureMoving(FigureMoving figmov)
        {
            // беру фигуру
            this.figmov = figmov;
            // возвращаю возможен ли ход
            return
                // может ли фигура пойти с данной клетки 'FROM'
                CanNowFigureMoveFrom() &&
                // может ли фигура пойди на клетку 'TO'
                CanNowFigureMoveTo() &&
                // может ли идти данная фигура
                CanThisFigureMoving();
        }

        // может ли фигура пойти с данной клетки 'FROM'
        bool CanNowFigureMoveFrom()
        {
            // фигура должна быть на доске и
            // фигура должна быть заданного цвета
            return figmov.from.FigureOnBoard() &&
                   figmov.figure.GetThisFigureColor() == board.moveColor;
        }

        // может ли фигура пойди на клетку 'TO'
        bool CanNowFigureMoveTo()
        {
            // фигура куда иду должна быть на доске
            // должна быть цвета игрока, который ходит
            return figmov.to.FigureOnBoard() &&
                   // нужно убедится что фигура именно меняет координаты (двигается)
                   figmov.from != figmov.to &&
                   // с доски получаю фигуру и получаю её цвет
                   // всё хорошо если он не равен тому цвету игрока кто сейчас ходит
                   board.GetFigureOnSquare(figmov.to).GetThisFigureColor() != board.moveColor;
                   // т.е. свои фигуры есть нельзя
        }

        // может ли идти данная фигура
        bool CanThisFigureMoving()
        {
            // умеет ли фигура ходит по правилам
            // какая это фигура?
            switch (figmov.figure)
            {
                // может ли король ходить
                case ChessFigures.whiteKing:
                case ChessFigures.blackKing:
                    return CanNowThisKingMove();
                // может ли королева/ферзь ходить
                case ChessFigures.whiteQueen:
                case ChessFigures.blackQueen:
                    return CanNowPossibleStraight();
                // может ли ладья ходить
                case ChessFigures.whiteRook:
                case ChessFigures.blackRook:
                    // возможно движение только горизонтально или вертикально
                    return (figmov.SignOfFigure_X == 0 || figmov.SignOfFigure_Y == 0)
                        && CanNowPossibleStraight();
                // может ли слон/офицер ходить
                case ChessFigures.whiteBishop:
                case ChessFigures.blackBishop:
                    // возможно движение только не горизонтально или не вертикально
                    return (figmov.SignOfFigure_X != 0 && figmov.SignOfFigure_Y != 0)
                        && CanNowPossibleStraight();
                // может ли конь ходить
                case ChessFigures.whiteKnight:
                case ChessFigures.blackKnight:
                    return CanNowThisKnightMove();
                // может ли пешка ходить
                case ChessFigures.whitePawn:
                case ChessFigures.blackPawn:
                    return CanPawnNowMove();
                // по умолчанию
                default: return false;
            }
        }

        private bool CanPawnNowMove()
        {
            // проверяю что пешка не на первой и не на последней клетке
            if (figmov.from.y < 1 || figmov.from.y > 6)
            {
                return false;
            }
            // белая пешка идёт вверх, черная идёт вниз
            int stepPawnY = figmov.figure.GetThisFigureColor() == ColorPlayer.white ? 1 : -1;
            // возвращаю что пешка может
            return
                // может пойти на одну клетку
                CanPawnOneStep(stepPawnY) ||
                // пойти на две клетки
                CanPawnTwoJumpStep(stepPawnY) ||
                // может ли пешка съесть фигуру
                CanPawnEatTheFigure(stepPawnY);
        }

        private bool CanPawnEatTheFigure(int stepPawnY)
        {
            // если клетка куда идет пешка не пустая
            if (board.GetFigureOnSquare(figmov.to) != ChessFigures.none)
            {
                // обязательно должно быть смещение на 1
                if (figmov.ModulusDifference_X == 1)
                {
                    // и в правильном направлении
                    if (figmov.Difference_Y == stepPawnY)
                    {
                        return true;
                    }
                }
            }
            return false; // в противном случае
        }

        private bool CanPawnTwoJumpStep(int stepPawnY)
        {
            // вначале "прыжок" - проверка куда пешка пошла
            // на пустую клетку - хорошо
            if (board.GetFigureOnSquare(figmov.to) == ChessFigures.none)
            {
                if (figmov.Difference_X == 0) // движение прямо
                {
                    // движение этой пешки должно быть:
                    if (figmov.Difference_Y == 2 * stepPawnY)
                    {
                        // координата У клетки обязательно
                        // для белой 1, для черной 6
                        if (figmov.from.y == 1 || figmov.from.y == 6)
                        {
                            // и нет прыжка через какую-нибудь фигуру
                            // плюс эта клетка тоже пустая
                            if (board.GetFigureOnSquare(new SquareBoard(figmov.from.x, figmov.from.y + stepPawnY)) == ChessFigures.none)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false; // в противном случае
        }

        private bool CanPawnOneStep(int stepPawnY)
        {
            // один шаг - проверка куда пешка пошла
            // на пустую клетку - хорошо
            if (board.GetFigureOnSquare(figmov.to) == ChessFigures.none)
            {
                if (figmov.Difference_X == 0) // движение прямо
                {
                    // действительно только на один шаг
                    if (figmov.Difference_Y == stepPawnY)
                    {
                        return true;
                    }
                }
            }
            return false; // в противном случае
        }

        // возможно ли движение королевы прямо (т.е. по линии)
        private bool CanNowPossibleStraight()
        {
            SquareBoard atSquare = figmov.from;
            do
            {
                // происходит смещение на единицу на 1-у клетку
                atSquare = new SquareBoard(atSquare.x + figmov.SignOfFigure_X, atSquare.y + figmov.SignOfFigure_Y);
                // удалось ли дойти до последней клетки
                if (atSquare == figmov.to)
                    return true; // если ДА
            }
            while (atSquare.FigureOnBoard() && // если был выход за пределы доски
                        // какая фигура там куда иду
                   board.GetFigureOnSquare(atSquare) == ChessFigures.none);
            return false; // значит НЕТ
        }

        // может ли король ходить
        private bool CanNowThisKingMove()
        {
            // ищу разницу координат
            // должно быть либо 0, либо 1
            if (figmov.ModulusDifference_X <= 1 && figmov.ModulusDifference_Y <= 1)
            {
                return true;
            }
            return false;
        }

        // может ли конь ходить
        private bool CanNowThisKnightMove()
        {
            // нужно вычислить модуль между разницей координат
            // по х должен прыгнуть на единицу, а по У на двойку
            if (figmov.ModulusDifference_X == 1 && figmov.ModulusDifference_Y == 2)
            {
                return true;
            }
            // или наоборот
            if (figmov.ModulusDifference_X == 2 && figmov.ModulusDifference_Y == 1)
            {
                return true;
            }
            // других возможностей нет
            return false;
        }
    }
}
