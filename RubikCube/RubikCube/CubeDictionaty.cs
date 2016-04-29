namespace RubikCube
{
    /// <summary>
    /// A logical map of the cube and it's colors, with their matching values.
    /// Made for the programmer, dosn't have any value for the program itself
    /// </summary>
    class CubeDictionary
    {   //WHITE
        public const int WhiteTopLeft = 12;
        public const int WhiteTopMid = 16;
        public const int WhiteTopRight = 4;
        public const int WhiteMidLeft = 0;
        public const int WhiteMidMid = 13;
        public const int WhiteMidRight = 2;
        public const int WhiteBottomLeft = 19;
        public const int WhiteBottomMid = 5;
        public const int WhiteBottomRight = 14;
        //BLUE
        public const int BlueTopLeft = 12;
        public const int BlueTopMid = 0;
        public const int BlueTopRight = 19;
        public const int BlueMidLeft = 9;
        public const int BlueMidMid = 3;
        public const int BlueMidRight = 24;
        public const int BlueBottomLeft = 23;
        public const int BlueBottomMid = 11;
        public const int BlueBottomRight = 22;
        //RED
        public const int RedTopLeft = 23;
        public const int RedTopMid = 20;
        public const int RedTopRight = 17;
        public const int RedMidLeft = 9;
        public const int RedMidMid = 25;
        public const int RedMidRight = 7;
        public const int RedBottomLeft = 12;
        public const int RedBottomMid = 16;
        public const int RedBottomRight = 4;
        //ORANGE
        public const int OrangeTopLeft = 19;
        public const int OrangeTopMid = 5;
        public const int OrangeTopRight = 14;
        public const int OrangeMidLeft = 24;
        public const int OrangeMidMid = 10;
        public const int OrangeMidRight = 15;
        public const int OrangeBottomLeft = 22;
        public const int OrangeBottomMid = 8;
        public const int OrangeBottomRight = 6;
        //GREEN
        public const int GreenTopLeft = 4;
        public const int GreenTopMid = 2;
        public const int GreenTopRight = 14;
        public const int GreenMidLeft = 15;
        public const int GreenMidMid = 1;
        public const int GreenMidRight = 7;
        public const int GreenBottomLeft = 6;
        public const int GreenBottomMid = 18;
        public const int GreenBottomRight = 17;
        //YELLOW
        public const int YellowTopLeft = 22;
        public const int YellowTopMid = 8;
        public const int YellowTopRight = 6;
        public const int YellowMidLeft = 11;
        public const int YellowMidMid = 21;
        public const int YellowMidRight = 18;
        public const int YellowBottomLeft = 23;
        public const int YellowBottomMid = 20;
        public const int YellowBottomRight = 17;
        
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
