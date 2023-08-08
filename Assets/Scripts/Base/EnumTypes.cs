/* [ Enum Types ]
* Global하게 사용되어야 하는 공통 데이터 타입 정의함
*/

namespace EnumTypes
{
    public enum GameType
    {
        Single,
        Multi,
        Free
    }
    
    public enum GameState
    {
        None = 0,
        Loading = 1,
        Running = 2,
        End = 3,
        Restart = 4
    }

    public enum StageState
    {
        BeforeStageStart = 0,
        RotateLP = 1,
        Decorate = 2,
        DoPosing = 3
    }

    public enum BearType
    {
        GuestBear = 0,
        CorrectBear = 1,
        PlayerBear = 2
    }

    public enum DecorateType
    {
        PutCream = 0,
        Draw = 1,
        CutAndShape = 2,
        ChangeColor = 3
    }

    public enum BearColorType
    {
        Blue = 0,
        Green = 1,
        Orange = 2,
        PastelBlue = 3,
        // PastelGreen = 4,
        // PastelOrange = 5,
        // PastelPurple = 6,
        PastelYellow = 4, 
        Pink = 5,
        Purple = 6,
        Red = 7,
        White = 8,
        Yellow = 9
    }

}