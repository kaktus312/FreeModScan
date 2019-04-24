namespace FreeModScan
{
    internal class HoldingRegister : Register
    {
        RegType _type = RegType.HOLDING_REGISTER;    //тип регистра
        uint _offset = 300001;
        string _title = "Default Holding Register Title";       //Имя регистра
    }
}