using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGameConsole
{
    class ChessBoard
    {
        public string fen { get; private set; }
        // определяю массив всех фигур
        ChessFigures[,] figures;
        // определяю цвет того чей ход
        public  ColorPlayer moveColor { get; private set; }
        // какой номер хода
        public int moveNumber { get; private set; }

        // конструктор который принимает строчку "fen" и распарсит её
        public ChessBoard(string fen)
        {
            this.fen = fen;
            // создание матрицы из всех фигур
            figures = new ChessFigures[8, 8];
            // инициализирую все фигуры на доске
            Init();
        }

        void Init()
        {
            // "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1" из 6 частей
            //  0                                           1 2    3 4 5
            // 'Split' - разбиваю фен на части
            string[] parts = fen.Split();
            // проверка количества частей
            if (parts.Length != 6) return;
            // инициализирую фигуры
            InitFiguresOnBoard(parts[0]);
            // указание цвета по части 1-й фена
            moveColor = parts[1] == "b" ? ColorPlayer.black : ColorPlayer.white;
            // какой номер хода (в пятой части фена)
            moveNumber = int.Parse(parts[5]);
            // ставлю фигуры на клетки
            // ранее SetFigureOnSquare(new SquareBoard("a1"), ChessFigures.whiteKing); // король
            // ранее SetFigureOnSquare(new SquareBoard("h8"), ChessFigures.blackKing); // король
            // кто ходит
            // ранее moveColor = ColorPlayer.white;
        }

        void InitFiguresOnBoard (string data)
        {
            for (int j = 8; j >= 2; j--)
                // меняю число 8 на 7
                // public String Replace(String oldValue, String newValue);
                data = data.Replace(j.ToString(), (j - 1).ToString() + "1");
            // меняю единицы на точки (это для доски)
            data = data.Replace("1", ".");
            // делю по слэшу
            string[] lines = data.Split('/');
            // расставляю все фигуры
            for (int y = 7; y >= 0; y--)
                for (int x = 0; x < 8; x++)
                    figures[x, y] = lines[7 - y][x] == '.' ? ChessFigures.none : (ChessFigures)lines[7 - y][x];
        }

        // генерации нового фена после хода
        void GenerateNewFEN()
        {
            // получаю все фигуры, цвета и номера хода
            // для цвета: если был белый - стал черный  
            fen = FenFigures() + " " + 
                (moveColor == ColorPlayer.white ? "w" : "b") 
                + " - - 0 " + moveNumber.ToString();
        }

        string FenFigures()
        {
            // использую "StringBuilder"
            // public sealed class StringBuilder : ISerializable
            StringBuilder sbilder = new StringBuilder();
            // сверху
            for (int y = 7; y >= 0; y--)
            {
                for (int x = 0; x < 8; x++)
                    // добавляю либо фигуру, либо точку (1)
                    sbilder.Append(figures[x, y] == ChessFigures.none ? '1' : (char)figures[x, y]);
                if (y > 0) // чтобы не делать лишних
                    sbilder.Append('/'); // дошел до конца строчки - ставлю '/'
            }
            // заменяю все единицы
            string one_eight = "11111111";
            for (int j = 8; j >= 2; j--)
                sbilder.Replace(one_eight.Substring(0, j), j.ToString());
            return sbilder.ToString();
        }

        // перебор всех фигур на доске
        public IEnumerable<FigureOnSquare> EnumerationAllFigures()
        {
            // перебираю все квадраты на шахматной доске
            foreach (SquareBoard square in SquareBoard.EnumerationAllSquares())
                // если на квадрате фигура такого же цвета
                if (GetFigureOnSquare(square).GetThisFigureColor() == moveColor)
                    // тогда возвращаю её
                    yield return new FigureOnSquare(GetFigureOnSquare(square), square);
            
        }

        // для получения любой фигуры на доске (на клетке)
        public ChessFigures GetFigureOnSquare(SquareBoard square)
        {
            // если клетка на доске
            if (square.FigureOnBoard())
                // возвращаю координаты
                return figures[square.x, square.y];
            // если за доской
            return ChessFigures.none;
        }

        // для установки фигуры на клетку доски
        // вызывается только из Init
        void SetFigureOnSquare(SquareBoard square, ChessFigures figure)
        {
            // проверяю находится ли фигура на доске
            if (square.FigureOnBoard())
            {
                // если да - записываю фигуру
                figures[square.x, square.y] = figure;
            }
        }

        // пишу метод для движения фигур
        public ChessBoard Move (FigureMoving figmov)
        {
            // создаю новую доску
            ChessBoard newBoard = new ChessBoard(fen);
            // расставляю фигуры, откуда ушла фигура там пусто
            newBoard.SetFigureOnSquare(figmov.from, ChessFigures.none);
            // на место куда пошла появляется фигура
            // и есть ли замена фигуры? (promotion)
            newBoard.SetFigureOnSquare(figmov.to, figmov.promotion == ChessFigures.none ? figmov.figure : figmov.promotion);
            // увеличиваю номер хода (это черные фигуры-игрок)
            if (moveColor == ColorPlayer.black)
            {
                newBoard.moveNumber++; // номер хода
            }
            // и меняю цвет игрока для следующего хода (другой игрок)
            newBoard.moveColor = moveColor.FlipThisColor();
            // для генерации нового фена после хода
            newBoard.GenerateNewFEN();
            // и возвращаю полученные значения
            return newBoard; // новая доска с учетом хода
        }

        // проверка на шах/мат
        public bool PutInCheck()
        {
            // новая доска после хода
            ChessBoard afterWalk = new ChessBoard(fen);
            // меняю цвет игрока
            afterWalk.moveColor = moveColor.FlipThisColor();
            // шах зависит от того можно ли съесть короля
            return afterWalk.PlayerCanEatKing();
        }

        // логическая функция - "может ли текущий игрок съесть короля"
        private bool PlayerCanEatKing()
        {
            // ищу координату короля которому угрожают
            SquareBoard threatKing = FindThisTargetKing();
            // все возможные ходы для доски
            ChessMoves moves = new ChessMoves(this);
            // перебираю список всех фигур на доске
            foreach (FigureOnSquare figsq in EnumerationAllFigures())
            {
                // все ходы фигур должны вести к королю
                FigureMoving figmov = new FigureMoving(figsq, threatKing);
                // если туда можно пойти - это угроза королю
                if (moves.CanNowFigureMoving(figmov))
                    // значит короля можно съесть
                    return true;
            }
            return false;
        }

        private SquareBoard FindThisTargetKing()
        {
            // какой король под угрозой
            // если ход черных - ищу белого короля - и т.д.
            ChessFigures threatKing = moveColor == ColorPlayer.black ? ChessFigures.whiteKing : ChessFigures.blackKing;
            // поиск по клеткам шахматной доски
            foreach (SquareBoard square in SquareBoard.EnumerationAllSquares())
            {
                // если на указанной клетке - искомый король
                if (GetFigureOnSquare(square) == threatKing)
                {
                    // возвращаю эту клетку
                    return square;
                }
            }
            // значит ничего не найдено
            return SquareBoard.none;
        }

        // проверка на шах/мат уже после хода
        public bool AfterMovePutInCheck(FigureMoving figmov)
        {
            // делаю этот ход
            ChessBoard afterWalk = Move(figmov);
            // и возвращаю значение  - остается ли король под угрозой?
            return afterWalk.PlayerCanEatKing();
        }
    }
}