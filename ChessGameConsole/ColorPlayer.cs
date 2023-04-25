using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 4-м создаю перечисление для цвета игрока
namespace ChessGameConsole
{
    enum ColorPlayer 
    {
        // если цвет не определён
        none,
        // белый цвет фигур
        white,
        // черный цвет фигур
        black
    }

    // чтобы можно было менять цвет игрока
    // создаю статичный класс - это метод для перечисления
    static class ColorChange
    { 
        // пишу вспомогательную функцию - чтобы переключать цвет фигур игрока
        public static ColorPlayer FlipThisColor(this ColorPlayer colorPlayer)
        {
            // проверяю какой цвет игрока сейчас, чтобы его изменить
            if (colorPlayer == ColorPlayer.black) 
                return ColorPlayer.white;
            if (colorPlayer == ColorPlayer.white) 
                return ColorPlayer.black;
            // и если цвет не задан
            return ColorPlayer.none;

        }
    }

}