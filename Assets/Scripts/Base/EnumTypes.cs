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
    
    public enum BearColorType
    {
        Choco,
        Strawberry,
        Mint,
        Orange
    }

}