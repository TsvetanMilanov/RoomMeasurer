namespace RoomMeasurer.Client.Utilities
{
    using Windows.Devices.Sensors;

    public static class AngleCalculator
    {
        public static double CalculateAngle()
        {
            Compass compass = Compass.GetDefault();

            if (compass == null)
            {
                return 0;
            }
            
            double angle = compass.GetCurrentReading().HeadingMagneticNorth;

            return angle;
        }
    }
}
