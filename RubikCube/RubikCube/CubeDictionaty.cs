using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubikCube
{
    class CubeDictionary
    {   //WHITE
        public const int WHITE_TOP_LEFT = 12;
        public const int WHITE_TOP_MID = 16;
        public const int WHITE_TOP_RIGHT = 4;
        public const int WHITE_MID_LEFT = 0;
        public const int WHITE_MID_MID = 13;
        public const int WHITE_MID_RIGHT = 2;
        public const int WHITE_BOTTOM_LEFT = 19;
        public const int WHITE_BOTTOM_MID = 5;
        public const int WHITE_BOTTOM_RIGHT = 14;
        //BLUE
        public const int BLUE_TOP_LEFT = 12;
        public const int BLUE_TOP_MID = 0;
        public const int BLUE_TOP_RIGHT = 19;
        public const int BLUE_MID_LEFT = 9;
        public const int BLUE_MID_MID = 3;
        public const int BLUE_MID_RIGHT = 24;
        public const int BLUE_BOTTOM_LEFT = 23;
        public const int BLUE_BOTTOM_MID = 11;
        public const int BLUE_BOTTOM_RIGHT = 22;
        //RED
        public const int RED_TOP_LEFT = 23;
        public const int RED_TOP_MID = 20;
        public const int RED_TOP_RIGHT = 17;
        public const int RED_MID_LEFT = 9;
        public const int RED_MID_MID = 25;
        public const int RED_MID_RIGHT = 7;
        public const int RED_BOTTOM_LEFT = 12;
        public const int RED_BOTTOM_MID = 16;
        public const int RED_BOTTOM_RIGHT = 4;
        //ORANGE
        public const int ORANGE_TOP_LEFT = 19;
        public const int ORANGE_TOP_MID = 5;
        public const int ORANGE_TOP_RIGHT = 14;
        public const int ORANGE_MID_LEFT = 24;
        public const int ORANGE_MID_MID = 10;
        public const int ORANGE_MID_RIGHT = 15;
        public const int ORANGE_BOTTOM_LEFT = 22;
        public const int ORANGE_BOTTOM_MID = 8;
        public const int ORANGE_BOTTOM_RIGHT = 6;
        //GREEN
        public const int GREEN_TOP_LEFT = 4;
        public const int GREEN_TOP_MID = 2;
        public const int GREEN_TOP_RIGHT = 14;
        public const int GREEN_MID_LEFT = 15;
        public const int GREEN_MID_MID = 1;
        public const int GREEN_MID_RIGHT = 7;
        public const int GREEN_BOTTOM_LEFT = 6;
        public const int GREEN_BOTTOM_MID = 18;
        public const int GREEN_BOTTOM_RIGHT = 17;
        //YELLOW
        public const int YELLOW_TOP_LEFT = 22;
        public const int YELLOW_TOP_MID = 8;
        public const int YELLOW_TOP_RIGHT = 6;
        public const int YELLOW_MID_LEFT = 11;
        public const int YELLOW_MID_MID = 21;
        public const int YELLOW_MID_RIGHT = 18;
        public const int YELLOW_BOTTOM_LEFT = 23;
        public const int YELLOW_BOTTOM_MID = 20;
        public const int YELLOW_BOTTOM_RIGHT = 17;
        
        // dictionary acording to index (location in array)
        //
        // 0 - BLUE WHITE 
        // 1 - GREEN
        // 2 - GREEN WHITE
        // 3 - BLUE 
        // 4 - RED GREEN WHITE
        // 5 - ORANGE WHITE
        // 6 - ORANGE GREEN YELLOW
        // 7 - RED GREEN 
        // 8 - ORANGE YELLOW  
        // 9 - BLUE RED
        // 10 - ORANGE
        // 11 - YELLOW BLUE 
        // 12 - RED WHITE BLUE 
        // 13 - WHITE
        // 14 - WHITE ORANGE GREEN
        // 15 - GREEN ORANGE
        // 16 - WHITE RED 
        // 17 - RED GREEN YELLOW
        // 18 - GREEN YELLOW 
        // 19 - WHITE BLUE ORANGE
        // 20 - YELLOW RED 
        // 21 - YELLOW
        // 22 - BLUE ORANGE YELLOW
        // 23 - RED YELLOW BLUE
        // 24 - ORANGE BLUE
        // 25 - RED 

        //dictionary acording to mesh name
        //
        // 1 - RED GREEN WHITE
        // 2 - RED GREEN 
        // 3 - RED GREEN YELLOW
        // 4 - GREEN WHITE
        // 5 - GREEN
        // 6 - GREEN YELLOW 
        // 7 - ORANGE GREEN WHITE
        // 8 - ORANGE GREEN
        // 9 - ORANGE GREEN YELLOW
        // 10 - WHITE RED 
        // 11 - RED 
        // 12 - YELLOW RED 
        // 13 - WHITE
        // 14 - YELLOW
        // 15 - ORANGE WHITE
        // 16 - ORANGE
        // 17 - ORANGE YELLOW  
        // 18 - RED WHITE BLUE 
        // 19 - BLUE RED
        // 20 - RED YELLOW BLUE
        // 21 - BLUE WHITE 
        // 22 - BLUE 
        // 23 - YELLOW BLUE 
        // 24 - WHITE BLUE ORANGE
        // 25 - ORANGE BLUE
        // 26 - BLUE ORANGE YELLOW
    }
}
