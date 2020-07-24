namespace TwAdsClass
{
  public class PlcVariableClass
    {
        public string Test { get; set; }
        public string[] ControlsName { get; set; }

        public string[] Address { get; set; }

        public int[] HandId { get; set; }

        public string[] Type { get; set; }

        public object[] value { get; set; }
    }
}
