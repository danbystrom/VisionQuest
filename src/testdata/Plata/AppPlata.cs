namespace Plata
{
	class AppSpecifics
	{
		public static readonly string Name = "Plåta 2012";

	    public static readonly string Version = "2012-10-10"
#if DEBUG
	    + " debug"
#endif
        ;
	}
}
