#pragma once
#include "Common.h"
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
namespace Profiler
{
	int64 GetTime();

	int64 GetFrequency();
	int64 GetTimeMilliSeconds();
	int64 GetTimeMicroSeconds();

	void SpinSleep(uint32 microSeconds);
}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

