public class MarketData
{
	private string name;
	private string acronym;
	private int size;
	private float[] priceData;

	public MarketData(string name, string acryonym, int size, float[] priceData){
		this.name = name;
		this.acronym = acronym;
		this.size = size;
		this.priceData = priceData;
	}

	public float priceAt(int index){
		return priceData[index];
	}
}

