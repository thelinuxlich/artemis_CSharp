using Artemis.Interface;

public class PositionComponent : IComponent, IInitialize {

	public void Initialize (params object[] args)
	{
		X = 0;
		Y = 0;
		Z = 0;
	}
		
	public int X { get; set; }
	public int Y { get; set; }
	public int Z { get; set; }


}
