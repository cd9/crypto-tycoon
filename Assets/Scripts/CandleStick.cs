
public class CandleStick
{
	private int low, high, open, close;

	public int Low{
		get{
			return low;
		}
		set{
			low = value;
		}
	}

	public int High{
		get{
			return high;
		}
		set{
			high = value;
		}
	}

	public int Open{
		get{
			return open;
		}
		set{
			open = value;
		}
	}

	public int Close{
		get{
			return close;
		}
		set{
			close = value;
		}
	}

	public CandleStick(int low, int high, int open, int close){
		this.low = low;
		this.high = high;
		this.open = open;
		this.close = close;
	}
}

