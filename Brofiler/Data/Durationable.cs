﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Profiler.Data
{
	public class Durable
	{
		private long start;
		public long Start
		{
			get { return start; }
			set { start = value; }
		}

		private long finish;
		public long Finish
		{
			get { return finish; }
			set { finish = value; }
		}

		private static double freq = 1;
		public static void InitFrequency(long frequency)
		{
			freq = 1000.0 / (double)frequency;
		}

		public double Duration
		{
			get { return TicksToMs(finish - start); }
		}

		public static double TicksToMs(long duration)
		{
			return freq * duration;
		}

		public void ReadDurable(BinaryReader reader)
		{
			start = reader.ReadInt64();
			finish = reader.ReadInt64();
		}

		public Durable(long s, long f)
		{
			this.start = s;
			this.finish = f;
		}

		public Durable() { }
	}
}
