using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGameConsole
{
    public class ChessClass
    {
        // создаю конструктор для запуска
        // передаю фэн-параметры с - https://en.wikipedia.org/wiki/Forsyth%E2%80%93Edwards_Notation 
        // такой FEN для исходной позиции: rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1 
        // задаю начальную позицию по умолчанию

        // переменная для начальной позиции
        // делаю её публичной - даю возможность обращаться к ней
        public string fen { get; private set; }
        // для создания шахматной доски после хода
        ChessBoard board;
        // для реализации ходов фигур 
        ChessMoves moves;
        // для списка всех возможных ходов всех фигур
        List<FigureMoving> lotsOfMoves;

        // начальная позиция фигур на доске
        // этот класс не будет меняться никогда
        public ChessClass(string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")
        {
            // передаю начальную позицию на шахматной доске
            this.fen = fen; // fen - международная нотация
            // вначале создаю доску через 'fen'
            board = new ChessBoard(fen);
            // создаю ходы на основании доски которая есть сейчас
            moves = new ChessMoves(board);
        }

        // внутренний конструктор
        // для создания новой шахматной доски после хода
        ChessClass(ChessBoard board)
        {
            this.board = board;
            // устанавливаю новый фен доски после хода
            this.fen = board.fen;
            // ходы на основании доски которая есть
            moves = new ChessMoves(board);
        }

        // пишу функцию чтобы делать ход
        // "Фигура" - "ИзКлетки" - "ВКлетку" - "ВКогоПревратиться"
        // пример - "Pe2e4" или "Pe7e8Q" - "пешка с Е2 на Е4" ...
        public ChessClass MoveFigure(string movefigure)
        {
            // передаю туда текущий ход
            FigureMoving figmov = new FigureMoving(movefigure);
            // сперва проверка на возможность данного хода фигурой
            if (!moves.CanNowFigureMoving(figmov))
            {
                return this;
            }
            // если шах - нельзя так ходить
            if (board.AfterMovePutInCheck(figmov))
            {
                return this;
            }
            //  создаю новую доску с новым ходом
            ChessBoard newboard = board.Move(figmov);
            // создаю новую шахматную композицию после хода 
            // на новой шахматной доске (не от 'fen'-а, а от новой шахматной доски)
            ChessClass nextShessClass = new ChessClass(newboard);
            // возвращаю "новые" шахматы-фигуры
            return nextShessClass;
        }

        // для определения какой-либо фигуры в клетках шахматной доски
        // по координатам на доске
        public char GetFigureAtShessBoard(int x, int y)
        {
            // какая фигура где находится - сперва было "return '.'";
            SquareBoard square = new SquareBoard(x, y);
            // смотрю какая фигура
            ChessFigures f = board.GetFigureOnSquare(square);
            // если нет фигуры возвращаю точку, иначе фигуру (символ фигуры)
            return f == ChessFigures.none ? '.' : (char)f;
        }

        // пишу функцию для поиска всех возможных ходов
        void FindLotsOfMoves()
        {
            // задаю пустой список ходов
            lotsOfMoves = new List<FigureMoving>();
            // далее перебираю все фигуры на квадратах доски
            // того же цвета
            foreach (FigureOnSquare figsqr in board.EnumerationAllFigures())
                // далее перебираю все клетки доски
                foreach (SquareBoard to in SquareBoard.EnumerationAllSquares())
                // пересечение этих множеств даст список возможных ходов
                {
                    FigureMoving figmov = new FigureMoving(figsqr, to);
                    // если ход может быть выполнен добавляю его в список
                    if (moves.CanNowFigureMoving(figmov))
                    {
                        // нет ли угрозы шаха, если нет
                        if (!board.AfterMovePutInCheck(figmov))
                        {
                            // если нет шаха
                            lotsOfMoves.Add(figmov); // добавляю в список
                        }
                    }
                }
        }

        // пишу функцию поиск всех возможных ходов
        public List<string> GetLotsOfMoves()
        {
            // сперва ищу ходы
            FindLotsOfMoves();
            // создаю список ходов
            // количество равно количеству элементов
            List<string> list = new List<string>();
            // для каждого элемента из списка
            foreach (FigureMoving figmov in lotsOfMoves)
            {
                // добавляю
                list.Add(figmov.ToString());
            }
            return list;
        }

        // делаю проверку - есть ли шах
        public bool PutInCheck()
        {
            return board.PutInCheck();
        }

    }
}