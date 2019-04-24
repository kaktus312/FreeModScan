namespace FreeModScan
{
    internal class DigitalInput : Register
    {
        RegType _type = RegType.DIGITAL_INPUT;    //тип регистра
        uint _offset = 200001;
        string _title = "Default Digital Input Register Title";       //Имя регистра
    }
}