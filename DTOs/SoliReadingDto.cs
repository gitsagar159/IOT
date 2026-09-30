namespace IOT.DTOs
{
    public class SoliReadingDto
    {
    }

    public class SoilSensorReadItemDto
    {
        public string SensorName { get; set; } = string.Empty;
        public double MoistureValue { get; set; }
    }

    public class MultiSensorPayloadDto
    {

        public List<SoilSensorReadItemDto> Readings { get; set; } = new();
    }

}
