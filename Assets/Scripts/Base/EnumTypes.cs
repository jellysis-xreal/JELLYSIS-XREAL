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
        InLobby = 0,
        BeforeStageStart = 1,
        StageStart = 2,
        Decorate = 3,
        RotateLP = 4,
        DoPosing = 5
    }

    public enum BearType
    {
        GuestBear = 0,
        AnswerBear = 1,
        PlayerBear = 2,
        AutoBear = 3
    }

    public enum DecorateType
    {
        PutCream = 0,
        Draw = 1,
        CutAndShape = 2,
        ChangeColor = 3,
        Basic = 4
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