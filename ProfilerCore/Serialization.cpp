#include "Common.h"
#include "Serialization.h"

namespace Profiler
{
	std::string OutputDataStream::GetData()
	{
		flush();
		return str();
	}

	Profiler::OutputDataStream OutputDataStream::Empty;

	OutputDataStream &operator << ( OutputDataStream &stream, const char* val )
	{
		uint32 length = (uint32)strlen(val);
		stream << length;
		stream.write( val, length );
		return stream;
	}

	OutputDataStream &operator << ( OutputDataStream &stream, int val )
	{
		stream.write( (char*)&val, sizeof(int) );
		return stream;
	}

	OutputDataStream &operator << ( OutputDataStream &stream, int64 val )
	{
		stream.write( (char*)&val, sizeof(int64) );
		return stream;
	}

	OutputDataStream &operator << ( OutputDataStream &stream, char val )
	{
		stream.write( (char*)&val, sizeof(char) );
		return stream;
	}

	OutputDataStream &operator << ( OutputDataStream &stream, byte val )
	{
		stream.write( (char*)&val, sizeof(byte) );
		return stream;
	}
	
	OutputDataStream & operator<<(OutputDataStream &stream, size_t val)
	{
		BRO_VERIFY(val <= 0xFFFFFFFF, "Can't serialize size_t greater than uint32", return stream);
		stream.write( (char*)&val, sizeof(uint32) );
		return stream;
	}

	OutputDataStream & operator<<(OutputDataStream &stream, uint32 val)
	{
		stream.write( (char*)&val, sizeof(uint32) );
		return stream;
	}

	OutputDataStream & operator<<(OutputDataStream &stream, const std::string& val)
	{
		stream << val.size();
		if (!val.empty())
			stream.write(&val[0], sizeof(val[0]) * val.size());
		return stream;
	}

	OutputDataStream & operator<<(OutputDataStream &stream, const std::wstring& val)
	{
		size_t count = val.size() * sizeof(wchar_t);
		stream << count;
		if (!val.empty())
			stream.write((char*)(&val[0]), count);
		return stream;
	}

	OutputDataStream & operator<<(OutputDataStream &stream, uint64 val)
	{
		stream.write((char*)&val, sizeof(val));
		return stream;
	}

	InputDataStream &operator >> ( InputDataStream &stream, int32 &val )
	{
		stream.read( (char*)&val, sizeof(int) );
		return stream;
	}

	InputDataStream &operator >> ( InputDataStream &stream, int64 &val )
	{
		stream.read( (char*)&val, sizeof(int64) );
		return stream;
	}

	InputDataStream & operator>>( InputDataStream &stream, byte &val )
	{
		stream.read( (char*)&val, sizeof(byte) );
		return stream;
	}

	InputDataStream & operator>>( InputDataStream &stream, uint32 &val )
	{
		stream.read( (char*)&val, sizeof(uint32) );
		return stream;
	}

	InputDataStream & operator>>( InputDataStream &stream, uint64 &val )
	{
		stream.read( (char*)&val, sizeof(uint64) );
		return stream;
	}

	InputDataStream::InputDataStream( const char *buffer, int length ) :
	std::stringstream( ios_base::in | ios_base::out )
	{
		write( buffer, length );
		seekg(ios_base::beg);
	}



}