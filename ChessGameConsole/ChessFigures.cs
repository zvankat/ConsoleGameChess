using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 3-м создаю перечисление (класс) шахматных фигур
namespace ChessGameConsole
{
    // перечисляю шахматные фигуры
    // согласно международным правилам
    enum ChessFigures
    {   // перечисление всех фигур

        // нет фигуры на клетке
        none,

        // для белых фигур Заглавные буквы
        // белый король
        whiteKing = 'K',
        // белая королева
        whiteQueen = 'Q',
        // белая ладья
        whiteRook = 'R',
        // белый конь
        whiteBishop = 'B',
        // белый слон
        whiteKnight = 'N',
        // белая пешка
        whitePawn = 'P',

        // для черных фигур Маленькие буквы
        // черный король
        blackKing = 'k',
        // черная королева
        blackQueen = 'q',
        // черная ладья
        blackRook = 'r',
        // черный конь
        blackBishop = 'b',
        // черный слон
        blackKnight = 'n',
        // черная пешка
        blackPawn = 'p'

    }

    // пишу функцию для расширения
    static class FigureMethods
    {
        // статичная функция для возврата цвета
        // для текущей фигуры
        public static ColorPlayer GetThisFigureColor(this ChessFigures figure)
        {
            // если фигура не указана - цвета тоже нет
            if (figure == ChessFigures.none)
                return ColorPlayer.none;
            // проверяю и возвращаю цвет фигуры
            return (figure == ChessFigures.whiteKing ||
                    figure == ChessFigures.whiteQueen ||
                    figure == ChessFigures.whiteRook ||
                    figure == ChessFigures.whiteBishop ||
                    figure == ChessFigures.whiteKnight ||
                    figure == ChessFigures.whitePawn)
                ? ColorPlayer.white
                : ColorPlayer.black;
        }
    }


}