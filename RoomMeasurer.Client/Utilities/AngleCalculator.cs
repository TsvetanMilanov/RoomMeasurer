namespace RoomMeasurer.Client.Utilities
{
    using Windows.Devices.Sensors;

    public static class AngleCalculator
    {
        public static double CalculateAngle()
        {
            Compass compass = Compass.GetDefault();
            
            double angle = compass.GetCurrentReading().HeadingMagneticNorth;

            return angle;
        }
    }
}
